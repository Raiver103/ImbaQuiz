using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Domain.Exceptions;
using ImbaQuiz.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImbaQuiz.API.Controllers
{
    [ApiController]
    [Route("api/questions")]
    public class QuestionsController(IQuestionService _questionService) : ControllerBase
    {
        /// <summary>
        /// Получить все вопросы.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список всех вопросов.</returns>
        [HttpGet]
        public async Task<OkObjectResult> GetAllQuestions(CancellationToken cancellationToken)
        {
            var questions = await _questionService.GetAllAsync(cancellationToken);
            return Ok(questions);
        }

        /// <summary>
        /// Получить вопрос по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор вопроса.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Найденный вопрос.</returns>
        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetQuestion(int id, CancellationToken cancellationToken)
        {
            var question = await _questionService.GetByIdAsync(id, cancellationToken);
            return Ok(question);
        }

        /// <summary>
        /// Создать новый вопрос.
        /// </summary>
        /// <param name="questionDto">Данные нового вопроса.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Созданный вопрос.</returns>
        [HttpPost]
        public async Task<CreatedAtActionResult> CreateQuestion([FromBody] QuestionDTO questionDto, CancellationToken cancellationToken)
        {
            var createdQuestion = await _questionService.CreateAsync(questionDto, cancellationToken);
            return CreatedAtAction(nameof(GetQuestion), new { id = createdQuestion.Id }, createdQuestion);
        }

        /// <summary>
        /// Обновить существующий вопрос.
        /// </summary>
        /// <param name="id">Идентификатор вопроса.</param>
        /// <param name="questionDto">Обновлённые данные вопроса.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateQuestion(int id, [FromBody] QuestionDTO questionDto, CancellationToken cancellationToken)
        {
            await _questionService.UpdateAsync(id, questionDto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Удалить вопрос по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор вопроса.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteQuestion(int id, CancellationToken cancellationToken)
        {
            await _questionService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Получить все вопросы по идентификатору викторины.
        /// </summary>
        /// <param name="quizId">Идентификатор викторины.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список вопросов викторины.</returns>
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
