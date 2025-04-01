using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ImbaQuiz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDto)
        {
            try
            {
                if (userDto == null)
                {
                    return BadRequest("User data is null.");
                }

                // Логирование принятых данных
                Console.WriteLine($"Received user: {userDto.Id}, {userDto.Email}, {userDto.Name}");

                

                // Создание нового пользователя
                var createdUser = await _userService.CreateAsync(userDto);

                // Если CreateAsync возвращает null, возвращаем ошибку
                if (createdUser == null)
                {
                    return StatusCode(500, "User could not be created.");
                }

                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDTO userDto)
        {
            await _userService.UpdateAsync(id, userDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    } 
}
