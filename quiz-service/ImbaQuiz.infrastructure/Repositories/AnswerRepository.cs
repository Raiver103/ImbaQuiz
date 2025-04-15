using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Interfaces;
using ImbaQuiz.infrastructure.Persistence;
using Microsoft.EntityFrameworkCore; 

namespace ImbaQuiz.infrastructure.Repositories
{ 
    public class AnswerRepository : IAnswerRepository
    {
        private readonly AppDbContext _context;

        public AnswerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Answer>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Answers.ToListAsync(cancellationToken);
        }

        public async Task<Answer> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Answers.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<Answer> CreateAsync(Answer answer, CancellationToken cancellationToken)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync(cancellationToken);
            return answer;
        }

        public async Task<Answer> UpdateAsync(Answer answer, CancellationToken cancellationToken)
        {
            _context.Answers.Update(answer);
            await _context.SaveChangesAsync(cancellationToken);
            return answer;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var answer = await _context.Answers.FindAsync(new object[] { id }, cancellationToken);
            if (answer != null)
            {
                _context.Answers.Remove(answer);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId, CancellationToken cancellationToken)
        {
            return await _context.Answers
                                .Where(a => a.QuestionId == questionId)
                                .ToListAsync(cancellationToken);
        }
    }

}
