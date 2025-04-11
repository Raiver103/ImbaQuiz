using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces; 
using ImbaQuiz.Domain.Exceptions;
using ImbaQuiz.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc; 

namespace ImbaQuiz.API.Controllers
{ 
    [ApiController]
    [Route("api/quizzes")]
    public class QuizzesController(IQuizService _quizService, ILogSender _logSender) : ControllerBase
    {
        [HttpGet]
        public async Task<OkObjectResult> GetAllQuizzes(CancellationToken cancellationToken)
        {
            _logSender.SendLog("Fetching all quizzes");
            var quizzes = await _quizService.GetAllAsync(cancellationToken);
            _logSender.SendLog($"Fetched {quizzes.Count()} quizzes");
            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetQuiz(int id, CancellationToken cancellationToken)
        {
            _logSender.SendLog($"Fetching quiz with id {id}");
            var quiz = await _quizService.GetByIdAsync(id, cancellationToken);
            if (quiz is null)
            {
                _logSender.SendLog($"Quiz with id {id} not found.");
                throw new NotFoundException($"Quiz with id {id} not found.");
            }
            _logSender.SendLog($"Quiz with id {id} found.");
            return Ok(quiz);
        }

        [HttpPost]
        public async Task<CreatedAtActionResult> CreateQuiz([FromBody] QuizDTO quizDto, CancellationToken cancellationToken)
        {
            _logSender.SendLog("Creating a new quiz");
            var createdQuiz = await _quizService.CreateAsync(quizDto, cancellationToken);
            _logSender.SendLog($"Created quiz with id {createdQuiz.Id}");
            return CreatedAtAction("GetQuiz", new { id = createdQuiz.Id }, createdQuiz); // Используем строку "GetQuiz"
        }

        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateQuiz(int id, [FromBody] QuizDTO quizDto, CancellationToken cancellationToken)
        {
            _logSender.SendLog($"Updating quiz with id {id}");
            await _quizService.UpdateAsync(id, quizDto, cancellationToken);
            _logSender.SendLog($"Updated quiz with id {id}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteQuiz(int id, CancellationToken cancellationToken)
        {
            _logSender.SendLog($"Deleting quiz with id {id}");
            await _quizService.DeleteAsync(id, cancellationToken);
            _logSender.SendLog($"Deleted quiz with id {id}");
            return NoContent();
        }
    }
}
