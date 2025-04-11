using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces; 
using ImbaQuiz.Domain.Exceptions;
using ImbaQuiz.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc; 

namespace ImbaQuiz.API.Controllers
{ 
    [ApiController]
    [Route("api/questions")]
    public class QuestionsController(IQuestionService _questionService, ILogSender _logSender) : ControllerBase
    {
        [HttpGet]
        public async Task<OkObjectResult> GetAllQuestions(CancellationToken cancellationToken)
        { 
            var questions = await _questionService.GetAllAsync(cancellationToken); 
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetQuestion(int id, CancellationToken cancellationToken)
        { 
            var question = await _questionService.GetByIdAsync(id, cancellationToken);
            if (question is null)
            { 
                throw new NotFoundException($"Question with id {id} not found.");
            } 
            return Ok(question);
        }

        [HttpPost]
        public async Task<CreatedAtActionResult> CreateQuestion([FromBody] QuestionDTO questionDto, CancellationToken cancellationToken)
        { 
            var createdQuestion = await _questionService.CreateAsync(questionDto, cancellationToken); 
            return CreatedAtAction("GetQuestion", new { id = createdQuestion.Id }, createdQuestion); 
        }

        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateQuestion(int id, [FromBody] QuestionDTO questionDto, CancellationToken cancellationToken)
        { 
            await _questionService.UpdateAsync(id, questionDto, cancellationToken); 
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteQuestion(int id, CancellationToken cancellationToken)
        { 
            await _questionService.DeleteAsync(id, cancellationToken); 
            return NoContent();
        }

        [HttpGet("by-quiz/{quizId}")]
        public async Task<OkObjectResult> GetQuestionsByQuizId(int quizId, CancellationToken cancellationToken)
        { 
            var questions = await _questionService.GetByQuizIdAsync(quizId, cancellationToken);
            if (questions is null || !questions.Any())
            { 
                throw new NotFoundException("Questions are not found.");
            } 
            return Ok(questions);
        }
    }
}
