using ImbaQuiz.Application.Common;
using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Interfaces;
using ImbaQuiz.infrastructure.Persistence;
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
            return await _context.Quizzes.ToListAsync(cancellationToken);
        }

        public async Task<Quiz> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Quizzes.FindAsync(new object[] { id }, cancellationToken);
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
            var query = _context.Quizzes.AsQueryable();
 
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(q => q.UserId == userId);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<Quiz>
            {
                Items = items,
                TotalCount = totalCount
            };
        } 
    }
}
