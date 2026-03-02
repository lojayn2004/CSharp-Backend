using Microsoft.AspNetCore.Mvc;
using First_Backend.Dtos;
using ServicesAbstraction;

namespace First_Backend.Controllers
{
    [ApiController] 
    [Route("api/auth")]
    public class AuthController(IAuthService _authService): ControllerBase
    {
    
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            return Ok(await _authService.Login(loginRequest));
        }

        
    }
}