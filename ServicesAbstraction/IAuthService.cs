using Dtos;
using First_Backend.Dtos;

namespace ServicesAbstraction
{
    public interface IAuthService
    {

        Task<LoginResultDto> Login(LoginRequest loginRequest);
    }
    
}