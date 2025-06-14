using Microsoft.OpenApi.Models; 
using Microsoft.EntityFrameworkCore;
using ImbaQuiz.infrastructure.Persistence;
using ImbaQuiz.Application.Mapping; 
using ImbaQuiz.API.Services;
using ImbaQuiz.Domain.Interfaces;
using ImbaQuiz.infrastructure.Configuration;
using System.Reflection;
using ImbaQuiz.infrastructure.Interceptors;

namespace ImbaQuiz.API.Extensions
{
    public static class ApiServiceExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ImbaQuiz API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddScoped<AuditInterceptor>();

            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                var interceptor = serviceProvider.GetRequiredService<AuditInterceptor>();
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(interceptor);
            });

            services.Configure<RabbitMqSettings>(configuration.GetSection(RabbitMqSettings.SectionName));
            services.AddSingleton<ILogSender, LogSender>();

            return services;
        }
    }
}
