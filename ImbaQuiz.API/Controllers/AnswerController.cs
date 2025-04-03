using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ImbaQuiz.API.Controllers
{ 
    [ApiController]
    [Route("api/answers")]
    public class AnswersController(IAnswerService _answerService) : ControllerBase
    { 
        [HttpGet]
        public async Task<OkObjectResult> GetAllAnswers()
        {
            var answers = await _answerService.GetAllAsync();
            return Ok(answers);
        }

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetAnswer(int id)
        {
            var answer = await _answerService.GetByIdAsync(id);
            if (answer is null) {
                throw new NotFoundException($"Answer with id {id} not found.");
            }
            return Ok(answer);
        }

        [HttpPost]
        public async Task<CreatedAtActionResult> CreateAnswer([FromBody] AnswerDTO answerDto)
        {
            var createdAnswer = await _answerService.CreateAsync(answerDto);
            return CreatedAtAction(nameof(GetAnswer), new { id = createdAnswer.Id }, createdAnswer);
        }

        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateAnswer(int id, [FromBody] AnswerDTO answerDto)
        {
            await _answerService.UpdateAsync(id, answerDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteAnswer(int id)
        {
            await _answerService.DeleteAsync(id);
            return NoContent();
        }
 
        [HttpGet("by-question/{questionId}")]
        public async Task<OkObjectResult> GetAnswersByQuestionId(int questionId)
        {
            var answers = await _answerService.GetByQuestionIdAsync(questionId);  
            if (answers is null) {
                throw new NotFoundException($"Answers are not found.");
            }
            return Ok(answers);
        }
    } 
}
