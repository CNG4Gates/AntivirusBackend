using Antivirus.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Antivirus.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UsersReadDTO>> GetAllUsersAsync();
        Task<UsersReadDTO?> GetUserByIdAsync(long id);
        Task<UsersReadDTO> CreateUserAsync(UsersCreateDTO userDto, bool isAdmin = false);
        Task<UsersReadDTO?> UpdateUserByEmailAsync(string email, UsersUpdateDTO userDto); // <--- CORRECTO
        Task<bool> DeleteUserAsync(long id);
        Task<bool> IsAdminAsync(long userId);
    }
}
