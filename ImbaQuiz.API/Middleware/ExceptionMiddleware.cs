using ImbaQuiz.Domain.Exceptions;

namespace ImbaQuiz.API.Controllers
{
  public class ExceptionMiddleware
  {
      private readonly RequestDelegate _next;

      public ExceptionMiddleware(RequestDelegate next)
      {
          _next = next;
      }

      public async Task InvokeAsync(HttpContext httpContext)
      {
          try
          {
              await _next(httpContext);  
          }
          catch (Exception ex)
          {
              await HandleExceptionAsync(httpContext, ex);  
          }
      }

      private Task HandleExceptionAsync(HttpContext context, Exception exception)
      {
          context.Response.ContentType = "application/json";
           
          if (exception is NotFoundException)
          {
              context.Response.StatusCode = StatusCodes.Status404NotFound;
              return context.Response.WriteAsync(new { message = exception.Message }.ToString());
          }
           
          context.Response.StatusCode = StatusCodes.Status500InternalServerError;
          return context.Response.WriteAsync(new { message = "An unexpected error occurred." }.ToString());
      }
  }
}