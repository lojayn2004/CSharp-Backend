using First_Backend.Dtos;
using Microsoft.IdentityModel.Tokens;
using ServicesAbstraction;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using First_Backend.Models;
using First_Backend.Data;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Dtos;

namespace Services
{
    public class AuthService(MyDbContext _context, IConfiguration _configuration) : IAuthService
    {
        public async Task<LoginResultDto> Login(LoginRequest loginRequest)
        {
            // Find the user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
            if (user == null)
                throw  new UnAuthorizedException("Invalid email or password.");
            

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
                throw new  UnAuthorizedException("Invalid email or password");
            
            var loginResult = new LoginResultDto()
            {
                Email = loginRequest.Email,
                Token = GenerateJwtToken(user)
            };
            return loginResult;
        }


        // Generate JWT Token
        public string GenerateJwtToken(User user)
        {
            // Configuring JWT Authentication
            var jwtKey = _configuration["Jwt:Key"]; // JWT Key
            var jwtIssuer = _configuration["Jwt:Issuer"]; // JWT Issuer
            var jwtAudience = _configuration["Jwt:Audience"]; // JWT Audience

            // Check if the JWT config values are missing
            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("JWT configuration values are missing");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // SHA 256

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // 30 mins expiry
                signingCredentials: credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}