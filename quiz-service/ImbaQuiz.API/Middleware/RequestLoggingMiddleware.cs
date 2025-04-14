using ImbaQuiz.Domain.Interfaces;

namespace ImbaQuiz.API.Middleware
{ 
    public class RequestLoggingMiddleware
  {
      private readonly RequestDelegate _next;
      private readonly ILogSender _logSender;

      public RequestLoggingMiddleware(RequestDelegate next, ILogSender logSender)
      {
          _next = next;
          _logSender = logSender;
      }

      public async Task InvokeAsync(HttpContext context)
      {
          var request = context.Request;
          var method = request.Method;
          var path = request.Path;

          _logSender.SendLog($"Incoming request: {method} {path}");

          try
          {
              await _next(context);

              _logSender.SendLog($"Request {method} {path} responded with {context.Response.StatusCode}");
          }
          catch (Exception ex)
          {
              _logSender.SendLog($"Error handling request {method} {path}: {ex.Message}");
              throw; 
          }
      }
  }
}