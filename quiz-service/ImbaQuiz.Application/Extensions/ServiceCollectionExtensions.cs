using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using ImbaQuiz.Application.Validators;

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

            services.AddValidatorsFromAssemblyContaining<AnswerDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<QuestionDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<QuizDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();

            return services;
        }
    }
}
