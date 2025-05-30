﻿using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Domain.Exceptions;
using ImbaQuiz.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc; 

namespace ImbaQuiz.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController(IUserService _userService, ILogSender _logSender) : ControllerBase
    {
        [HttpGet]
        public async Task<OkObjectResult> GetAllUsers(CancellationToken cancellationToken)
        { 
            var users = await _userService.GetAllAsync(cancellationToken); 
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetUser(string id, CancellationToken cancellationToken)
        { 
            var user = await _userService.GetByIdAsync(id, cancellationToken); 
            return Ok(user);
        }

        [HttpPost]
        public async Task<CreatedAtActionResult> CreateUser([FromBody] UserDTO userDto, CancellationToken cancellationToken)
        { 
            var createdUser = await _userService.CreateAsync(userDto, cancellationToken); 
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id, cancellationToken = cancellationToken }, createdUser); 
        }

        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateUser(string id, [FromBody] UserDTO userDto, CancellationToken cancellationToken)
        { 
            await _userService.UpdateAsync(id, userDto, cancellationToken); 
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteUser(string id, CancellationToken cancellationToken)
        { 
            await _userService.DeleteAsync(id, cancellationToken); 
            return NoContent();
        }
    }
}
