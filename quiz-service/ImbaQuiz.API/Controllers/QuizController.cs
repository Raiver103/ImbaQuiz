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
                var quizzes = await _quizService.GetAllAsync(cancellationToken); 
                return Ok(quizzes);
            }

            [HttpGet("{id}")]
            public async Task<OkObjectResult> GetQuiz(int id, CancellationToken cancellationToken)
            { 
                var quiz = await _quizService.GetByIdAsync(id, cancellationToken); 
                return Ok(quiz);
            }

            [HttpPost]
            public async Task<CreatedAtActionResult> CreateQuiz([FromBody] QuizDTO quizDto, CancellationToken cancellationToken)
            { 
                var createdQuiz = await _quizService.CreateAsync(quizDto, cancellationToken); 
                return CreatedAtAction(nameof(GetQuiz), new { id = createdQuiz.Id, cancellationToken = cancellationToken }, createdQuiz); 
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
