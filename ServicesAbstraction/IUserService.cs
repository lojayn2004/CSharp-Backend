

using First_Backend.Dtos;
using First_Backend.Models;

namespace ServicesAbstraction
{
    public interface IUserService
    {
        Task<UserDto> CreateUser(User dto);

        Task<IEnumerable<UserDto>> GetAllUsers();

        Task<UserDto> GetUserById(int id);

        Task DeleteUser(int id);

    };
    
}