using Microsoft.OpenApi.Models; 
using Microsoft.EntityFrameworkCore; 
using System.Reflection;
using news_service.API.Interfaces;
using news_service.API.Repositories;
using StackExchange.Redis;
using news_service.API.Services;

namespace ImbaQuiz.API.Extensions
{
    public static class ApiServiceExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddMemoryCache();

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var config = configuration.GetValue<string>("Redis:ConnectionString");
                return ConnectionMultiplexer.Connect(config);
            });

            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<INewsService, NewsService>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")  
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
