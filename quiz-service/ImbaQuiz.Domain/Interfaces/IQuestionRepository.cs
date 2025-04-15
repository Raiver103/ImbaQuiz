using ImbaQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Domain.Interfaces
{ 
    public interface IQuestionRepository : IRepository<Question, int>
    {
        Task<List<Question>> GetQuestionsByQuizIdAsync(int quizId, CancellationToken cancellationToken);
    }

}
