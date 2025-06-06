using ImbaQuiz.Domain.Entities;

namespace ImbaQuiz.Domain.Interfaces
{ 
    public interface IQuestionRepository : IRepository<Question, int>
    {
        Task<List<Question>> GetQuestionsByQuizIdAsync(int quizId, CancellationToken cancellationToken);
    }

}
