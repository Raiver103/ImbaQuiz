using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ImbaQuiz.API.Controllers
{ 
    [ApiController]
    [Route("api/quizzes")]
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizService _quizService;
 
        public QuizzesController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<OkObjectResult> GetAllQuizzes(CancellationToken cancellationToken)
        {
            var quizzes = await _quizService.GetAllAsync(cancellationToken);
            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetQuiz(int id, CancellationToken cancellationToken)
        {
            var quiz = await _quizService.GetByIdAsync(id, cancellationToken);
            if (quiz is null) 
            { 
                throw new NotFoundException($"Quiz with id {id} not found.");
            }
            return Ok(quiz);
        }

        [HttpPost]
        public async Task<CreatedAtActionResult> CreateQuiz([FromBody] QuizDTO quizDto, CancellationToken cancellationToken)
        {
            var createdQuiz = await _quizService.CreateAsync(quizDto, cancellationToken);
            return CreatedAtAction(nameof(GetQuiz), new { id = createdQuiz.Id }, createdQuiz);
        }

        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateQuiz(int id, [FromBody] QuizDTO quizDto, CancellationToken cancellationToken)
        {
            await _quizService.UpdateAsync(id, quizDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteQuiz(int id, CancellationToken cancellationToken)
        {
            await _quizService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
