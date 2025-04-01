using ImbaQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Domain.Interfaces
{ 
    public interface IAnswerRepository
    {
        Task<IEnumerable<Answer>> GetAllAsync();
        Task<Answer> GetByIdAsync(int id);
        Task<Answer> CreateAsync(Answer answer);
        Task<Answer> UpdateAsync(Answer answer);
        Task DeleteAsync(int id);
        Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId);
    }

}
