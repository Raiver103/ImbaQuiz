using ImbaQuiz.Domain.Entities;
using ImbaQuiz.infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace ImbaQuiz.infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        private readonly AuditInterceptor _auditInterceptor;
        public AppDbContext(DbContextOptions<AppDbContext> options, AuditInterceptor auditInterceptor) : base(options)
        {

            _auditInterceptor = auditInterceptor;
            try
            {
                Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migrate Error: {ex.Message}");
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Quizzes)
                .WithOne(q => q.User)
                .HasForeignKey(q => q.UserId);

            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.Questions)
                .WithOne(qt => qt.Quiz)
                .HasForeignKey(qt => qt.QuizId);

            modelBuilder.Entity<Question>()
                .HasMany(qt => qt.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder); 
        }
    }
}
