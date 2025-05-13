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
