using Antivirus.DTOs;

namespace Antivirus.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UsersReadDTO>> GetAllUsersAsync();
        Task<UsersReadDTO?> GetUserByIdAsync(long id);
        Task<UsersReadDTO> CreateUserAsync(UsersCreateDTO userDto, bool isAdmin = false);
        Task<UsersReadDTO?> UpdateUserAsync(long id, UsersCreateDTO userDto);
        Task<bool> DeleteUserAsync(long id);
        Task<bool> IsAdminAsync(long userId);
    }
}
