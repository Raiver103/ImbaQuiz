using log_service.API.Configurations;
using log_service.API.Services;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
 
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)   
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection(RabbitMqSettings.SectionName));

builder.Services.AddHostedService<LogConsumerService>();
 
var app = builder.Build();

app.Run();
