using ImbaQuiz.Domain.Interfaces;
using ImbaQuiz.infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection; 

namespace ImbaQuiz.infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();

            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();

            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            return services;
        }
    }
}
