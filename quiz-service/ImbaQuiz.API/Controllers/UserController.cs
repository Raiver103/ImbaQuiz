using ImbaQuiz.Application.DTOs;
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
            _logSender.SendLog("Fetching all users");
            var users = await _userService.GetAllAsync(cancellationToken);
            _logSender.SendLog($"Fetched {users.Count()} users");
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetUser(string id, CancellationToken cancellationToken)
        {
            _logSender.SendLog($"Fetching user with id {id}");
            var user = await _userService.GetByIdAsync(id, cancellationToken);
            if (user is null)
            {
                _logSender.SendLog($"User with id {id} not found.");
                throw new NotFoundException($"User with id {id} not found.");
            }
            _logSender.SendLog($"User with id {id} found.");
            return Ok(user);
        }

        [HttpPost]
        public async Task<CreatedAtActionResult> CreateUser([FromBody] UserDTO userDto, CancellationToken cancellationToken)
        {
            _logSender.SendLog("Creating a new user");
            var createdUser = await _userService.CreateAsync(userDto, cancellationToken);
            _logSender.SendLog($"Created user with id {createdUser.Id}");
            return CreatedAtAction("GetUser", new { id = createdUser.Id }, createdUser); // Используем строку "GetUser"
        }

        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateUser(string id, [FromBody] UserDTO userDto, CancellationToken cancellationToken)
        {
            _logSender.SendLog($"Updating user with id {id}");
            await _userService.UpdateAsync(id, userDto, cancellationToken);
            _logSender.SendLog($"Updated user with id {id}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteUser(string id, CancellationToken cancellationToken)
        {
            _logSender.SendLog($"Deleting user with id {id}");
            await _userService.DeleteAsync(id, cancellationToken);
            _logSender.SendLog($"Deleted user with id {id}");
            return NoContent();
        }
    }
}
