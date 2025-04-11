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
            _logSender.SendLog("Fetching all questions");
            var questions = await _questionService.GetAllAsync(cancellationToken);
            _logSender.SendLog($"Fetched {questions.Count()} questions");
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetQuestion(int id, CancellationToken cancellationToken)
        {
            _logSender.SendLog($"Fetching question with id {id}");
            var question = await _questionService.GetByIdAsync(id, cancellationToken);
            if (question is null)
            {
                _logSender.SendLog($"Question with id {id} not found.");
                throw new NotFoundException($"Question with id {id} not found.");
            }
            _logSender.SendLog($"Question with id {id} found.");
            return Ok(question);
        }

        [HttpPost]
        public async Task<CreatedAtActionResult> CreateQuestion([FromBody] QuestionDTO questionDto, CancellationToken cancellationToken)
        {
            _logSender.SendLog("Creating a new question");
            var createdQuestion = await _questionService.CreateAsync(questionDto, cancellationToken);
            _logSender.SendLog($"Created question with id {createdQuestion.Id}");
            return CreatedAtAction("GetQuestion", new { id = createdQuestion.Id }, createdQuestion); // Используем строку "GetQuestion"
        }

        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateQuestion(int id, [FromBody] QuestionDTO questionDto, CancellationToken cancellationToken)
        {
            _logSender.SendLog($"Updating question with id {id}");
            await _questionService.UpdateAsync(id, questionDto, cancellationToken);
            _logSender.SendLog($"Updated question with id {id}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteQuestion(int id, CancellationToken cancellationToken)
        {
            _logSender.SendLog($"Deleting question with id {id}");
            await _questionService.DeleteAsync(id, cancellationToken);
            _logSender.SendLog($"Deleted question with id {id}");
            return NoContent();
        }

        [HttpGet("by-quiz/{quizId}")]
        public async Task<OkObjectResult> GetQuestionsByQuizId(int quizId, CancellationToken cancellationToken)
        {
            _logSender.SendLog($"Fetching questions for quiz with id {quizId}");
            var questions = await _questionService.GetByQuizIdAsync(quizId, cancellationToken);
            if (questions is null || !questions.Any())
            {
                _logSender.SendLog($"Questions for quiz with id {quizId} not found.");
                throw new NotFoundException("Questions are not found.");
            }
            _logSender.SendLog($"Fetched {questions.Count()} questions for quiz with id {quizId}");
            return Ok(questions);
        }
    }
}
