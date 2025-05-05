using ImbaQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Domain.Interfaces
{ 
    public interface IQuizRepository : IRepository<Quiz, int>
    { 
        Task<PaginatedResult<Quiz>> GetPaginatedAsync(int pageNumber, int pageSize, string? userId, CancellationToken cancellationToken);
    } 
}
