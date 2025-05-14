using ImbaQuiz.Application.DTOs;

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
