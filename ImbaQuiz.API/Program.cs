using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Application.Mapping;
using ImbaQuiz.Application.Extensions;
using ImbaQuiz.Application.Services;
using ImbaQuiz.Domain.Interfaces;
using ImbaQuiz.infrastructure.Persistence;
using ImbaQuiz.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore; 
using Microsoft.OpenApi.Models;
using System;
using ImbaQuiz.infrastructure.Extensions;
using ImbaQuiz.API.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowAllOrigins", policy =>
    {
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

builder.Services.AddInfrastructure();
builder.Services.AddApplicationServices();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ExceptionFilter()); // Добавьте фильтр для обработки ошибок
});

var app = builder.Build();

app.UseMiddleware<ImbaQuiz.API.Controllers.ExceptionMiddleware>();
app.UseExceptionHandler("/error");

app.UseCors("AllowAllOrigins");
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();
app.Run();