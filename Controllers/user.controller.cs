using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using First_Backend.Data;
using First_Backend.Models;
using First_Backend.Dtos;
using ServicesAbstraction;

namespace First_Backend.Controllers
{
    [ApiController] 
    [Route("api/user")]
    public class UserController(IUserService _userService): ControllerBase
    {
        

        // Signs up a new user
        [HttpPost("signup")]
        public async Task<ActionResult<UserDto>> CreateUser(User user)
        {
            var userDto = await _userService.CreateUser(user);
            return CreatedAtAction(nameof(CreateUser), new { id = userDto.Id }, userDto);
        }

        // Get all users in the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        // Get user by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);

            return Ok(user);
        }

        // Delete a user from the database
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
           
            await _userService.DeleteUser(id);

            return Ok(new { message = "User has been deleted successfully" });
        }
    }
}