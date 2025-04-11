using ImbaQuiz.Domain.Exceptions;
using ImbaQuiz.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
namespace ImbaQuiz.API.Middleware 
{ 
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogSender _logSender;

        public ExceptionMiddleware(RequestDelegate next
            , ILogSender logSender
            )
        {
            _next = next;
            _logSender = logSender;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            { 
                _logSender.SendLog($"An error occurred: {ex.Message}\n{ex.StackTrace}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = exception switch
            {
                NotFoundException => new { message = exception.Message, statusCode = (int)HttpStatusCode.NotFound },
                ArgumentException => new { message = exception.Message, statusCode = (int)HttpStatusCode.BadRequest },
                _ => new { message = "An unexpected error occurred.", statusCode = (int)HttpStatusCode.InternalServerError }
            };

            response.StatusCode = errorResponse.statusCode;
            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
