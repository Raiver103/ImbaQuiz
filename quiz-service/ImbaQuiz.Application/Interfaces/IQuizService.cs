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
        Task<IEnumerable<QuizDTO>> GetAllAsync(CancellationToken cancellationToken);
        Task<QuizDTO> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<QuizDTO> CreateAsync(QuizDTO quizDto, CancellationToken cancellationToken);
        Task<QuizDTO> UpdateAsync(int id, QuizDTO quizDto, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
