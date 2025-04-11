using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ImbaQuiz.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();

            return services;
        }
    }
}
