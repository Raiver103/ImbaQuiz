using ImbaQuiz.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
namespace ImbaQuiz.API.Middleware 
{ 
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
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
