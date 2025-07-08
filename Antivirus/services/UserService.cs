using Antivirus.DTOs;
using Antivirus.Models;
using Microsoft.EntityFrameworkCore;
using Antivirus.Data;
using Antivirus.config;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antivirus.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsersReadDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(u => new UsersReadDTO
            {
                Id = u.Id,
                Name = u.Name,
                LastName = u.LastName,
                Email = u.Email,
                DateBirth = u.DateBirth,
                ImageUrl = u.ImageUrl
            });
        }

        public async Task<UsersReadDTO?> GetUserByIdAsync(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;
            return new UsersReadDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                DateBirth = user.DateBirth,
                ImageUrl = user.ImageUrl
            };
        }

        public async Task<UsersReadDTO> CreateUserAsync(UsersCreateDTO userDto, bool isAdmin = false)
        {
            var user = new User
            {
                Name = userDto.Name,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = PasswordHasher.HashPassword(userDto.Password),
                DateBirth = userDto.DateBirth,
                ImageUrl = userDto.ImageUrl
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var roleId = isAdmin ? 2 : 1;
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = roleId
            };
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return new UsersReadDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                DateBirth = user.DateBirth,
                ImageUrl = user.ImageUrl
            };
        }

        // NUEVO: UpdateUserByEmailAsync (ya NO permite cambiar email)
        public async Task<UsersReadDTO?> UpdateUserByEmailAsync(string email, UsersUpdateDTO userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            if (userDto.Name != null)
                user.Name = userDto.Name;

            if (userDto.LastName != null)
                user.LastName = userDto.LastName;

            if (userDto.DateBirth != null)
                user.DateBirth = userDto.DateBirth;

            if (userDto.ImageUrl != null)
                user.ImageUrl = userDto.ImageUrl;

            if (!string.IsNullOrEmpty(userDto.Password))
                user.Password = PasswordHasher.HashPassword(userDto.Password);

            // No se actualiza el email, simplemente lo ignoramos

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UsersReadDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                DateBirth = user.DateBirth,
                ImageUrl = user.ImageUrl
            };
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsAdminAsync(long userId)
        {
            return await _context.UserRoles.AnyAsync(ur =>
                ur.UserId == userId &&
                _context.Roles.Any(r => r.Id == ur.RoleId && r.Name.ToLower() == "admin")
            );
        }
    }
}
