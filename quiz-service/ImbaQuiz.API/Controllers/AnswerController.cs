using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces; 
using ImbaQuiz.Domain.Exceptions; 
using Microsoft.AspNetCore.Mvc;
using ImbaQuiz.Domain.Interfaces;

namespace ImbaQuiz.API.Controllers
{ 
    [ApiController]
    [Route("api/answers")]
    public class AnswersController(IAnswerService _answerService, ILogSender _logSender) : ControllerBase
    { 
        [HttpGet]
        public async Task<OkObjectResult> GetAllAnswers(CancellationToken cancellationToken)
        { 
            var answers = await _answerService.GetAllAsync(cancellationToken); 
            return Ok(answers);
        }

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetAnswer(int id, CancellationToken cancellationToken)
        { 
            var answer = await _answerService.GetByIdAsync(id, cancellationToken); 
            return Ok(answer);
        }

        [HttpPost]
        public async Task<CreatedAtActionResult> CreateAnswer([FromBody] AnswerDTO answerDto, CancellationToken cancellationToken)
        { 
            var createdAnswer = await _answerService.CreateAsync(answerDto, cancellationToken); 
            return CreatedAtAction(nameof(GetAnswer), new { id = createdAnswer.Id }, createdAnswer);
        }

        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateAnswer(int id, [FromBody] AnswerDTO answerDto, CancellationToken cancellationToken)
        { 
            await _answerService.UpdateAsync(id, answerDto, cancellationToken); 
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteAnswer(int id, CancellationToken cancellationToken)
        { 
            await _answerService.DeleteAsync(id, cancellationToken); 
            return NoContent();
        }

        [HttpGet("by-question/{questionId}")]
        public async Task<OkObjectResult> GetAnswersByQuestionId(int questionId, CancellationToken cancellationToken)
        { 
            var answers = await _answerService.GetByQuestionIdAsync(questionId, cancellationToken);
            if (answers is null) { 
                throw new NotFoundException("Answers are not found.");
            } 
            return Ok(answers);
        }
    }

}
