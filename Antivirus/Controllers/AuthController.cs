using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Antivirus.DTOs;
using Antivirus.Services;
using Antivirus.Data;
using Antivirus.Models;
using Microsoft.EntityFrameworkCore;
using Antivirus.config;
using Microsoft.Extensions.Configuration;

namespace Antivirus.Controllers
{
    [Route("api/admins")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AdminsController(IUserService userService, AppDbContext context, IConfiguration config)
        {
            _userService = userService;
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UsersCreateDTO userDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (_context.Users.Any(u => u.Email == userDto.Email))
                return BadRequest(new { message = "El correo electrónico ya está registrado." });

            var user = await _userService.CreateUserAsync(userDto, true);

            // Asignar rol Admin automáticamente
            var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == "admin" || r.Id == 2);
            if (adminRole == null)
                return BadRequest(new { message = "No se encontró el rol Admin en la base de datos." });

            var exists = await _context.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == adminRole.Id);
            if (!exists)
            {
                _context.UserRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = adminRole.Id
                });
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Administrador registrado exitosamente.", user });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserDto loginDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Email);
            if (user == null)
                return Unauthorized(new { message = "Usuario no encontrado." });

            var hashedPassword = PasswordHasher.HashPassword(loginDto.Password);
            if (user.Password != hashedPassword)
                return Unauthorized(new { message = "Contraseña incorrecta." });

            var userRole = _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                .FirstOrDefault();

            if (userRole == null || userRole.ToLower() != "admin")
                return Unauthorized(new { message = "El usuario no tiene rol de admin." });

            var token = new AuthService(_config).GenerateJwtToken(user);

            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/",
            });

            return Ok(new
            {
                message = "Inicio de sesión exitoso.",
                token = token,
                role = userRole,
                user = new
                {
                    id = user.Id,
                    name = user.Name,
                    lastName = user.LastName,
                    email = user.Email,
                    imageUrl = user.ImageUrl,
                    dateBirth = user.DateBirth
                }
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return Ok(new { message = "Sesión cerrada exitosamente." });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllAdmins()
        {
            var adminRole = _context.Roles.FirstOrDefault(r => r.Name.ToLower() == "admin" || r.Id == 2);
            if (adminRole == null) return NotFound(new { message = "Rol de administrador no encontrado." });

            var admins = (from ur in _context.UserRoles
                          join u in _context.Users on ur.UserId equals u.Id
                          where ur.RoleId == adminRole.Id
                          select new UsersReadDTO
                          {
                              Id = u.Id,
                              Email = u.Email,
                              Name = u.Name,
                              LastName = u.LastName,
                              DateBirth = u.DateBirth,
                              ImageUrl = u.ImageUrl
                          }).ToList();

            return Ok(admins);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetAdminById(long id)
        {
            var adminRole = _context.Roles.FirstOrDefault(r => r.Name.ToLower() == "admin" || r.Id == 2);
            if (adminRole == null) return NotFound(new { message = "Rol de administrador no encontrado." });

            var admin = (from ur in _context.UserRoles
                         join u in _context.Users on ur.UserId equals u.Id
                         where ur.RoleId == adminRole.Id && ur.UserId == id
                         select new UsersReadDTO
                         {
                             Id = u.Id,
                             Email = u.Email,
                             Name = u.Name,
                             LastName = u.LastName,
                             DateBirth = u.DateBirth,
                             ImageUrl = u.ImageUrl
                         }).FirstOrDefault();

            if (admin == null) return NotFound(new { message = "Administrador no encontrado." });

            return Ok(admin);
        }

        // Update por correo, NO por id. El correo NO se puede actualizar.
        [Authorize]
        [HttpPut("email/{email}")]
        public async Task<IActionResult> UpdateAdminByEmail(string email, [FromBody] UsersUpdateDTO updateDto)
        {
            // Ya no se valida que el autenticado sea el mismo, cualquiera puede editar
            var updated = await _userService.UpdateUserByEmailAsync(email, updateDto);
            if (updated == null)
                return NotFound(new { message = "Administrador no encontrado." });

            return Ok(updated);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(long id)
        {
            var userToDelete = await _context.Users.FindAsync(id);

            if (userToDelete == null)
                return NotFound(new { message = "Administrador no encontrado." });

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
