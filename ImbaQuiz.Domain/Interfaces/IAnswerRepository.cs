using ImbaQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Domain.Interfaces
{ 
    public interface IAnswerRepository : IRepository<Answer, int>
    {
        Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId);
    }

}
