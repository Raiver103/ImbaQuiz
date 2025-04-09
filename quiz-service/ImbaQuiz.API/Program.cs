using ImbaQuiz.API.Extensions;
using ImbaQuiz.API.Middleware; 
using ImbaQuiz.infrastructure.Extensions;
using ImbaQuiz.Application.Extensions;
using ImbaQuiz.API.Services;
using ImbaQuiz.infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
 

builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowAllOrigins");
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
