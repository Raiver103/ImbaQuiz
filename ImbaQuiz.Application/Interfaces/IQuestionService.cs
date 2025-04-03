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
        Task<IEnumerable<QuestionDTO>> GetAllAsync(CancellationToken cancellationToken);
        Task<QuestionDTO> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<QuestionDTO> CreateAsync(QuestionDTO questionDto, CancellationToken cancellationToken);
        Task<QuestionDTO> UpdateAsync(int id, QuestionDTO questionDto, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<List<QuestionDTO>> GetByQuizIdAsync(int quizId, CancellationToken cancellationToken);

    }

}
