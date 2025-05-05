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
        /// <summary>
        /// Получить все ответы.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список всех ответов.</returns>
        [HttpGet]
        public async Task<OkObjectResult> GetAllAnswers(CancellationToken cancellationToken)
        { 
            var answers = await _answerService.GetAllAsync(cancellationToken); 
            return Ok(answers);
        }

        /// <summary>
        /// Получить ответ по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор ответа.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Ответ с указанным идентификатором.</returns>
        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetAnswer(int id, CancellationToken cancellationToken)
        { 
            var answer = await _answerService.GetByIdAsync(id, cancellationToken); 
            return Ok(answer);
        }

        /// <summary>
        /// Создать новый ответ.
        /// </summary>
        /// <param name="answerDto">Данные нового ответа.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Созданный ответ.</returns>
        [HttpPost]
        public async Task<CreatedAtActionResult> CreateAnswer([FromBody] AnswerDTO answerDto, CancellationToken cancellationToken)
        { 
            var createdAnswer = await _answerService.CreateAsync(answerDto, cancellationToken); 
            return CreatedAtAction(nameof(GetAnswer), new { id = createdAnswer.Id }, createdAnswer);
        }

        /// <summary>
        /// Обновить существующий ответ.
        /// </summary>
        /// <param name="id">Идентификатор ответа для обновления.</param>
        /// <param name="answerDto">Обновлённые данные ответа.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Код ответа 204 (No Content).</returns>
        [HttpPut("{id}")]
        public async Task<NoContentResult> UpdateAnswer(int id, [FromBody] AnswerDTO answerDto, CancellationToken cancellationToken)
        { 
            await _answerService.UpdateAsync(id, answerDto, cancellationToken); 
            return NoContent();
        }

        /// <summary>
        /// Удалить ответ.
        /// </summary>
        /// <param name="id">Идентификатор ответа для удаления.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Код ответа 204 (No Content).</returns>
        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteAnswer(int id, CancellationToken cancellationToken)
        { 
            await _answerService.DeleteAsync(id, cancellationToken); 
            return NoContent();
        }

        /// <summary>
        /// Получить все ответы по идентификатору вопроса.
        /// </summary>
        /// <param name="questionId">Идентификатор вопроса.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список ответов, связанных с вопросом.</returns>
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
