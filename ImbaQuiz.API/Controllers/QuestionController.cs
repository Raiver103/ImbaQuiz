using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ImbaQuiz.API.Controllers
{ 
    [ApiController]
    [Route("api/questions")]
    public class QuestionsController(IQuestionService _questionService) : ControllerBase
    {  
        [HttpGet]
        public async Task<OkObjectResult> GetAllQuestions()
        {
            var questions = await _questionService.GetAllAsync();
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetQuestion(int id)
        {
            var question = await _questionService.GetByIdAsync(id); 
            if (question is null){ 
                throw new NotFoundException($"Question with id {id} not found.");
            }
            return Ok(question);
        }

        [HttpPost]
        public async Task<CreatedAtActionResult> CreateQuestion([FromBody] QuestionDTO questionDto)
        {
            var createdQuestion = await _questionService.CreateAsync(questionDto);
            return CreatedAtAction(nameof(GetQuestion), new { id = createdQuestion.Id }, createdQuestion);
        }

        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateQuestion(int id, [FromBody] QuestionDTO questionDto)
        {
            await _questionService.UpdateAsync(id, questionDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteQuestion(int id)
        {
            await _questionService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("byQuiz/{quizId}")]
        public async Task<OkObjectResult> GetQuestionsByQuizId(int quizId)
        {
            var questions = await _questionService.GetByQuizIdAsync(quizId);
            if (questions is null || !questions.Any())
            {   
                throw new NotFoundException($"Questions are not found.");
            }
            return Ok(questions);
        }

    }
}
