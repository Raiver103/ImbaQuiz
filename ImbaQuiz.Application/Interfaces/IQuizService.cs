using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Application.Interfaces
{
    public interface IQuizService
    {
        Task<IEnumerable<QuizDTO>> GetAllAsync();
        Task<QuizDTO> GetByIdAsync(int id);
        Task<QuizDTO> CreateAsync(QuizDTO quizDto);
        Task<QuizDTO> UpdateAsync(int id, QuizDTO quizDto);
        Task DeleteAsync(int id);
    }
}
