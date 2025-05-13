using ImbaQuiz.Application.DTOs;

namespace ImbaQuiz.Application.Interfaces
{
    public interface IQuizService
    {
        Task<IEnumerable<QuizDTO>> GetAllAsync(CancellationToken cancellationToken);
        Task<QuizDTO> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<QuizDTO> CreateAsync(QuizDTO quizDto, CancellationToken cancellationToken);
        Task<QuizDTO> UpdateAsync(int id, QuizDTO quizDto, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<PaginatedResult<QuizDTO>> GetPaginatedAsync(int pageNumber, int pageSize, string? userId, CancellationToken cancellationToken);
    }
}
