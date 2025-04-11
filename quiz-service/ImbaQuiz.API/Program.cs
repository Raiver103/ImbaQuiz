using ImbaQuiz.API.Extensions;
using ImbaQuiz.API.Middleware; 
using ImbaQuiz.infrastructure.Extensions;
using ImbaQuiz.Application.Extensions; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration); 
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    MigrationManager.ApplyMigrations(app.Services);
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowAllOrigins");
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
