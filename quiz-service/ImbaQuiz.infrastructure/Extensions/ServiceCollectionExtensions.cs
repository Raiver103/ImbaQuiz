using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Interfaces;
using ImbaQuiz.infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return services;
        }
    }
}
