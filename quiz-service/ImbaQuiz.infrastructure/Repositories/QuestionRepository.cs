using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Interfaces;
using ImbaQuiz.infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ImbaQuiz.infrastructure.Repositories
{ 
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Question>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Questions.ToListAsync(cancellationToken);
        }

        public async Task<Question> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Questions.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<Question> CreateAsync(Question question, CancellationToken cancellationToken)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync(cancellationToken);
            return question;
        }

        public async Task<Question> UpdateAsync(Question question, CancellationToken cancellationToken)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync(cancellationToken);
            return question;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var question = await _context.Questions.FindAsync(new object[] { id }, cancellationToken);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<List<Question>> GetQuestionsByQuizIdAsync(int quizId, CancellationToken cancellationToken)
        {
            return await _context.Questions
                .Where(q => q.QuizId == quizId)
                .ToListAsync(cancellationToken);
        }
    }
}
