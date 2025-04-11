using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces; 
using ImbaQuiz.Domain.Exceptions;
using ImbaQuiz.infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImbaQuiz.API.Controllers
{ 
    [ApiController]
    [Route("api/answers")]
    public class AnswersController(IAnswerService _answerService, ILogSender _logSender) : ControllerBase
    { 
        [HttpGet]
        public async Task<OkObjectResult> GetAllAnswers(CancellationToken cancellationToken)
        {
            _logSender.SendLog("Fetching all answers");
            var answers = await _answerService.GetAllAsync(cancellationToken);
            _logSender.SendLog($"Fetched {answers.ToList().Count} answers");
            return Ok(answers);
        }

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetAnswer(int id, CancellationToken cancellationToken)
        {
            _logSender.SendLog($"Fetching answer with id {id}");
            var answer = await _answerService.GetByIdAsync(id, cancellationToken);
            if (answer is null) {
                _logSender.SendLog($"Answer with id {id} not found.");
                throw new NotFoundException($"Answer with id {id} not found.");
            }
            _logSender.SendLog($"Answer with id {id} found.");
            return Ok(answer);
        }

        [HttpPost]
        public async Task<CreatedAtActionResult> CreateAnswer([FromBody] AnswerDTO answerDto, CancellationToken cancellationToken)
        {
            _logSender.SendLog("Creating a new answer");
            var createdAnswer = await _answerService.CreateAsync(answerDto, cancellationToken);
            _logSender.SendLog($"Created answer with id {createdAnswer.Id}");
            return CreatedAtAction(nameof(GetAnswer), new { id = createdAnswer.Id }, createdAnswer);
        }

        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateAnswer(int id, [FromBody] AnswerDTO answerDto, CancellationToken cancellationToken)
        {
            _logSender.SendLog($"Updating answer with id {id}");
            await _answerService.UpdateAsync(id, answerDto, cancellationToken);
            _logSender.SendLog($"Updated answer with id {id}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteAnswer(int id, CancellationToken cancellationToken)
        {
            _logSender.SendLog($"Deleting answer with id {id}");
            await _answerService.DeleteAsync(id, cancellationToken);
            _logSender.SendLog($"Deleted answer with id {id}");
            return NoContent();
        }

        [HttpGet("by-question/{questionId}")]
        public async Task<OkObjectResult> GetAnswersByQuestionId(int questionId, CancellationToken cancellationToken)
        {
            _logSender.SendLog($"Fetching answers for question with id {questionId}");
            var answers = await _answerService.GetByQuestionIdAsync(questionId, cancellationToken);
            if (answers is null) {
                _logSender.SendLog($"Answers for question with id {questionId} not found.");
                throw new NotFoundException("Answers are not found.");
            }
            _logSender.SendLog($"Fetched {answers.ToList().Count} answers for question with id {questionId}");
            return Ok(answers);
        }
    }

}
