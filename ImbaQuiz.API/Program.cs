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

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();


var app = builder.Build();

app.Run();