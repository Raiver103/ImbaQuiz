using news_service.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using ImbaQuiz.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddApiServices(builder.Configuration); 
var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
