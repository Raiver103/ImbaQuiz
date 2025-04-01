using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Application.Interfaces
{
    public interface IAnswerService
    {
        Task<IEnumerable<AnswerDTO>> GetAllAsync();
        Task<AnswerDTO> GetByIdAsync(int id);
        Task<AnswerDTO> CreateAsync(AnswerDTO answerDto);
        Task<AnswerDTO> UpdateAsync(int id, AnswerDTO answerDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<AnswerDTO>> GetByQuestionIdAsync(int questionId);
    }

}
