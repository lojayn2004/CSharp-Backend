

using Exceptions;
using First_Backend.Data;
using First_Backend.Dtos;
using First_Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ServicesAbstraction;

namespace Services
{
    public class UserService(MyDbContext _context) : IUserService
    {
        public async Task<UserDto> CreateUser(User user)
        {
            bool isNameTaken = await _context.Users.AnyAsync(u => u.Name == user.Name);
            if (isNameTaken)
                throw new BadRequestException("Username already exists");

            

            bool isEmailTaken = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (isEmailTaken)
                throw new BadRequestException("Email already exists");
            

           
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        
            _context.Users.Add(user);
            
          
            await _context.SaveChangesAsync();

           
            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
            return userDto;
        }

        public async Task DeleteUser(int id)
        {
             // Find the user to be deleted
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new NotFoundExcpetion("User is not registered yet!");
            

            // Delete from DB and save changes
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await _context.Users
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                })
                .ToListAsync();
            
            return users;
        }

        public async Task<UserDto> GetUserById(int id)
        {
             var user = await _context.Users
                .Where(u => u.Id== id)
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                })
                .FirstOrDefaultAsync();
            
            if (user == null)
              throw new NotFoundExcpetion("User is not registered yet!");

            return user;
        }
    }

}