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
        Task<IEnumerable<AnswerDTO>> GetAllAsync(CancellationToken cancellationToken);
        Task<AnswerDTO> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<AnswerDTO> CreateAsync(AnswerDTO answerDto, CancellationToken cancellationToken);
        Task<AnswerDTO> UpdateAsync(int id, AnswerDTO answerDto, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<AnswerDTO>> GetByQuestionIdAsync(int questionId, CancellationToken cancellationToken);
    }


}
