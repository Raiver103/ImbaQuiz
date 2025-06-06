using ImbaQuiz.Domain.Entities;

namespace ImbaQuiz.Domain.Interfaces
{ 
    public interface IAnswerRepository : IRepository<Answer, int>
    {
        Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId, CancellationToken cancellationToken);
    } 
}
