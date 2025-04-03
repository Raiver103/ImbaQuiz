using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ImbaQuiz.API.Controllers
{ 
    [ApiController]
    [Route("api/quizzes")]
    public class QuizzesController(IQuizService _quizService) : ControllerBase
    { 

        [HttpGet]
        public async Task<OkObjectResult> GetAllQuizzes()
        {
            var quizzes = await _quizService.GetAllAsync();
            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetQuiz(int id)
        {
            var quiz = await _quizService.GetByIdAsync(id);
            if (quiz is null) { 
                throw new NotFoundException($"Quiz with id {id} not found.");
            }
            return Ok(quiz);
        }

        [HttpPost]
        public async Task<CreatedAtActionResult> CreateQuiz([FromBody] QuizDTO quizDto)
        {
            var createdQuiz = await _quizService.CreateAsync(quizDto);
            return CreatedAtAction(nameof(GetQuiz), new { id = createdQuiz.Id }, createdQuiz);
        }

        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateQuiz(int id, [FromBody] QuizDTO quizDto)
        {
            await _quizService.UpdateAsync(id, quizDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteQuiz(int id)
        {
            await _quizService.DeleteAsync(id);
            return NoContent();
        }
    }
}
