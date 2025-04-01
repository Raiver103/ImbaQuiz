using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Application.Interfaces
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionDTO>> GetAllAsync();
        Task<QuestionDTO> GetByIdAsync(int id);
        Task<QuestionDTO> CreateAsync(QuestionDTO questionDto);
        Task<QuestionDTO> UpdateAsync(int id, QuestionDTO questionDto);
        Task DeleteAsync(int id);
        Task<List<QuestionDTO>> GetByQuizIdAsync(int quizId);

    }

}
