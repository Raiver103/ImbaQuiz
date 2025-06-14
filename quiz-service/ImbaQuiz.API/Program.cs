using ImbaQuiz.API.Extensions;
using ImbaQuiz.API.Middleware; 
using ImbaQuiz.infrastructure.Extensions;
using ImbaQuiz.Application.Extensions; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration); 
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseCors("AllowAllOrigins");
app.MapControllers();

app.UseSwagger(); 
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ImbaQuiz API v1");
    c.RoutePrefix = string.Empty;  
});

app.Run();
