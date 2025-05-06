using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Domain.Exceptions;
using ImbaQuiz.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImbaQuiz.API.Controllers
{
    [ApiController]
    [Route("api/quizzes")]
    public class QuizzesController(IQuizService _quizService) : ControllerBase
    {
        /// <summary>
        /// Получить все викторины.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Список всех викторин.</returns>
        [HttpGet]
        public async Task<OkObjectResult> GetAllQuizzes(CancellationToken cancellationToken)
        {
            var quizzes = await _quizService.GetAllAsync(cancellationToken);
            return Ok(quizzes);
        }

        /// <summary>
        /// Получить викторину по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор викторины.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Объект викторины.</returns>
        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetQuiz(int id, CancellationToken cancellationToken)
        {
            var quiz = await _quizService.GetByIdAsync(id, cancellationToken);
            return Ok(quiz);
        }

        /// <summary>
        /// Создать новую викторину.
        /// </summary>
        /// <param name="quizDto">Данные викторины.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Созданная викторина.</returns>
        [HttpPost]
        public async Task<CreatedAtActionResult> CreateQuiz([FromBody] QuizDTO quizDto, CancellationToken cancellationToken)
        {
            var createdQuiz = await _quizService.CreateAsync(quizDto, cancellationToken);
            return CreatedAtAction(nameof(GetQuiz), new { id = createdQuiz.Id }, createdQuiz);
        }

        /// <summary>
        /// Обновить существующую викторину.
        /// </summary>
        /// <param name="id">Идентификатор викторины.</param>
        /// <param name="quizDto">Обновлённые данные викторины.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateQuiz(int id, [FromBody] QuizDTO quizDto, CancellationToken cancellationToken)
        {
            await _quizService.UpdateAsync(id, quizDto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Удалить викторину по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор викторины.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteQuiz(int id, CancellationToken cancellationToken)
        {
            await _quizService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Gets a paginated list of quizzes for a specific user.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of quizzes per page.</param>
        /// <param name="userId">The user ID to filter the quizzes by (optional).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A paginated result containing quizzes.</returns>
        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] string? userId,
            CancellationToken cancellationToken)
        {
            var result = await _quizService.GetPaginatedAsync(pageNumber, pageSize, userId, cancellationToken);
            return Ok(result);
        }
    }
}
