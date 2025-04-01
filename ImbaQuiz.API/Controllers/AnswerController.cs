using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ImbaQuiz.API.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswersController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnswers()
        {
            var answers = await _answerService.GetAllAsync();
            return Ok(answers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnswer(int id)
        {
            var answer = await _answerService.GetByIdAsync(id);
            if (answer == null) return NotFound();
            return Ok(answer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnswer([FromBody] AnswerDTO answerDto)
        {
            var createdAnswer = await _answerService.CreateAsync(answerDto);
            return CreatedAtAction(nameof(GetAnswer), new { id = createdAnswer.Id }, createdAnswer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnswer(int id, [FromBody] AnswerDTO answerDto)
        {
            await _answerService.UpdateAsync(id, answerDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            await _answerService.DeleteAsync(id);
            return NoContent();
        }

        // Новый метод для получения всех ответов по вопросу
        [HttpGet("byQuestion/{questionId}")]
        public async Task<IActionResult> GetAnswersByQuestionId(int questionId)
        {
            var answers = await _answerService.GetByQuestionIdAsync(questionId); // Получаем ответы для конкретного вопроса
            if (answers == null || !answers.Any())
            {
                return NotFound($"No answers found for question with ID {questionId}");
            }
            return Ok(answers);
        }
    }

}
