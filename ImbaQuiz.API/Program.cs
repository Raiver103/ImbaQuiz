using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Application.Mapping;
using ImbaQuiz.Application.Services;
using ImbaQuiz.Domain.Interfaces;
using ImbaQuiz.infrastructure.Persistence;
using ImbaQuiz.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowAllOrigins", policy =>
    {
       // ��������� ������� � ������ ��������� (����� ���������� ������ ������ �������)
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddControllers();

 
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ImbaQuiz API", Version = "v1" });
});

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();

var app = builder.Build();

app.UseCors("AllowAllOrigins");
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();
app.Run();