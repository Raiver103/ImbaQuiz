
using ImbaQuiz.API.Services;
using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Interfaces;
using ImbaQuiz.infrastructure.Interfaces;
using ImbaQuiz.infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection; 

namespace ImbaQuiz.infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRepository<User, string>, UserRepository>();

            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IRepository<Quiz, int>, QuizRepository>();

            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IRepository<Question, int>, QuestionRepository>();

            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IRepository<Answer, int>, AnswerRepository>();


            services.AddSingleton<ILogSender, LogSender>();

            

            return services;
        }
    }
}
