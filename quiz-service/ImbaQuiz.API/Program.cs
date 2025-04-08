using ImbaQuiz.API.Extensions;
using ImbaQuiz.API.Middleware; 
using ImbaQuiz.infrastructure.Extensions;
using ImbaQuiz.Application.Extensions;
using ImbaQuiz.API.Services;

var builder = WebApplication.CreateBuilder(args);
 

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build(); 

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowAllOrigins");
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
