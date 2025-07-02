using Microsoft.AspNetCore.Mvc;
using Antivirus.Models;
using Antivirus.Services;
using Antivirus.DTOs;
using Antivirus.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Antivirus.config;

namespace Antivirus.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;
        private readonly IUserService _userService;

        public AuthController(AppDbContext context, AuthService authService, IUserService userService)
        {
            _context = context;
            _authService = authService;
            _userService = userService;
        }

        // === Registro usuario normal ===
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsersCreateDTO userDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Correo único
            if (_context.Users.Any(u => u.Email == userDto.Email))
                return BadRequest(new { message = "El correo electrónico ya está registrado." });

            var user = await _userService.CreateUserAsync(userDto, false);
            return Ok(new { message = "Usuario registrado exitosamente.", user });
        }

        // === Registro admin ===
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UsersCreateDTO userDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Correo único
            if (_context.Users.Any(u => u.Email == userDto.Email))
                return BadRequest(new { message = "El correo electrónico ya está registrado." });

            var user = await _userService.CreateUserAsync(userDto, true);
            return Ok(new { message = "Administrador registrado exitosamente.", user });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UsersCreateDTO loginDto)
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

            if (userRole == null)
                return Unauthorized(new { message = "El usuario no tiene un rol asignado." });

            var token = _authService.GenerateJwtToken(user);

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
                role = userRole
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return Ok(new { message = "Sesión cerrada exitosamente." });
        }

        [HttpGet("admins")]
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
                              DateBirth = u.DateBirth
                          }).ToList();

            return Ok(admins);
        }

        [HttpGet("admins/{id}")]
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
                             DateBirth = u.DateBirth
                         }).FirstOrDefault();

            if (admin == null) return NotFound(new { message = "Administrador no encontrado." });

            return Ok(admin);
        }

        // ========== EDICIÓN Y ELIMINACIÓN ==========

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] UsersCreateDTO updateDto)
        {
            var loggedUser = await GetLoggedUser(HttpContext);
            var userToUpdate = await _context.Users.FindAsync(id);

            if (userToUpdate == null)
                return NotFound(new { message = "Usuario no encontrado." });

            if (await _userService.IsAdminAsync(loggedUser.Id))
            {
                if (await _userService.IsAdminAsync(userToUpdate.Id) && loggedUser.Id != userToUpdate.Id)
                    return Forbid("Un admin no puede editar a otro admin.");
            }
            else
            {
                if (loggedUser.Id != userToUpdate.Id)
                    return Forbid("No tienes permisos para editar a otro usuario.");
            }

            // Permitir campos editables
            userToUpdate.Name = updateDto.Name ?? userToUpdate.Name;
            userToUpdate.LastName = updateDto.LastName ?? userToUpdate.LastName;
            userToUpdate.DateBirth = updateDto.DateBirth ?? userToUpdate.DateBirth;
            if (!string.IsNullOrEmpty(updateDto.Password))
                userToUpdate.Password = PasswordHasher.HashPassword(updateDto.Password);

            await _context.SaveChangesAsync();

            return Ok(new UsersReadDTO
            {
                Id = userToUpdate.Id,
                Name = userToUpdate.Name,
                LastName = userToUpdate.LastName,
                Email = userToUpdate.Email,
                DateBirth = userToUpdate.DateBirth
            });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var loggedUser = await GetLoggedUser(HttpContext);
            var userToDelete = await _context.Users.FindAsync(id);

            if (userToDelete == null)
                return NotFound(new { message = "Usuario no encontrado." });

            if (!await _userService.IsAdminAsync(loggedUser.Id))
                return Forbid("Solo un administrador puede eliminar usuarios.");
            if (await _userService.IsAdminAsync(userToDelete.Id))
                return Forbid("No puedes eliminar a un usuario administrador.");
            if (loggedUser.Id == userToDelete.Id)
                return Forbid("No puedes eliminarte a ti mismo.");

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ========== Utilidad privada ==========
        private async Task<User> GetLoggedUser(HttpContext httpContext)
        {
            var email = httpContext.User.Identity.Name;
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
