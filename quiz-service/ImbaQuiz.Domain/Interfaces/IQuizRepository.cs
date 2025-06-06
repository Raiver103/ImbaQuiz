using ImbaQuiz.Application.Common;
using ImbaQuiz.Domain.Entities;

namespace ImbaQuiz.Domain.Interfaces
{ 
    public interface IQuizRepository : IRepository<Quiz, int>
    { 
        Task<PaginatedResult<Quiz>> GetPaginatedAsync(int pageNumber, int pageSize, string? userId, CancellationToken cancellationToken);
    } 
}
