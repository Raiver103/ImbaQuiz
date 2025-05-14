using ImbaQuiz.Application.Common;
using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Interfaces;
using ImbaQuiz.infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ImbaQuiz.infrastructure.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly AppDbContext _context;

        public QuizRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Quiz>> GetAllAsync(CancellationToken cancellationToken)
        { 
            return await _context.Quizzes.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Quiz> GetByIdAsync(int id, CancellationToken cancellationToken)
        { 
            return await _context.Quizzes.AsNoTracking().FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
        }

        public async Task<Quiz> CreateAsync(Quiz quiz, CancellationToken cancellationToken)
        {
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync(cancellationToken);
            return quiz;
        }

        public async Task<Quiz> UpdateAsync(Quiz quiz, CancellationToken cancellationToken)
        {
            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync(cancellationToken);
            return quiz;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var quiz = await _context.Quizzes.FindAsync(new object[] { id }, cancellationToken);
            if (quiz != null)
            {
                _context.Quizzes.Remove(quiz);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<PaginatedResult<Quiz>> GetPaginatedAsync(int pageNumber, int pageSize, string? userId, CancellationToken cancellationToken)
        { 
            var sqlQuery = @"
                SELECT * FROM Quizzes
                WHERE (@userId IS NULL OR UserId = @userId)
                ORDER BY Id
                OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";
 
            var offset = (pageNumber - 1) * pageSize;
 
            var quizzes = await _context.Quizzes
                .FromSqlRaw(sqlQuery, 
                    new SqlParameter("@userId", (object)userId ?? DBNull.Value),
                    new SqlParameter("@offset", offset),
                    new SqlParameter("@pageSize", pageSize))
                .AsNoTracking()   
                .ToListAsync(cancellationToken);
 
            var totalCount = await _context.Quizzes
                .Where(q => string.IsNullOrEmpty(userId) || q.UserId == userId)
                .CountAsync(cancellationToken);

            return new PaginatedResult<Quiz>
            {
                Items = quizzes,
                TotalCount = totalCount
            };
        }
    }
}
