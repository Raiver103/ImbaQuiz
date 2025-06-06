using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Domain.Exceptions;
using ImbaQuiz.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImbaQuiz.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController(IUserService _userService) : ControllerBase
    {
        /// <summary>
        /// Получить список всех пользователей.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Список пользователей.</returns>
        [HttpGet]
        public async Task<OkObjectResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllAsync(cancellationToken);
            return Ok(users);
        }

        /// <summary>
        /// Получить пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Информация о пользователе.</returns>
        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetUser(string id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);
            return Ok(user);
        }

        /// <summary>
        /// Создать нового пользователя.
        /// </summary>
        /// <param name="userDto">Данные пользователя.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Созданный пользователь.</returns>
        [HttpPost]
        public async Task<CreatedAtActionResult> CreateUser([FromBody] UserDTO userDto, CancellationToken cancellationToken)
        {
            var createdUser = await _userService.CreateAsync(userDto, cancellationToken);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Обновить данные пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <param name="userDto">Обновлённые данные пользователя.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateUser(string id, [FromBody] UserDTO userDto, CancellationToken cancellationToken)
        {
            await _userService.UpdateAsync(id, userDto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Удалить пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteUser(string id, CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
