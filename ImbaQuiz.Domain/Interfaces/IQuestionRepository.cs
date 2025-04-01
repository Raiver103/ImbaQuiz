using ImbaQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Domain.Interfaces
{ 
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>> GetAllAsync();
        Task<Question> GetByIdAsync(int id);
        Task<Question> CreateAsync(Question question);
        Task<Question> UpdateAsync(Question question);
        Task DeleteAsync(int id);
        Task<List<Question>> GetQuestionsByQuizIdAsync(int quizId);
    }

}
