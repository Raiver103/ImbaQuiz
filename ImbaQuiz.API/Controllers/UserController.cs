using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ImbaQuiz.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController(IUserService _userService) : ControllerBase
    { 

        [HttpGet]
        public async Task<OkObjectResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetUser(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user is null) { 
                throw new NotFoundException($"Quiz with id {id} not found.");
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<CreatedAtActionResult> CreateUser([FromBody] UserDTO userDto)
        { 
            var createdUser = await _userService.CreateAsync(userDto); 
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser); 
        }

        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateUser(string id, [FromBody] UserDTO userDto)
        {
            await _userService.UpdateAsync(id, userDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteUser(string id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    } 
}
