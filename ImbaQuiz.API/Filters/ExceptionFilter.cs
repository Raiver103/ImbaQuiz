using ImbaQuiz.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ImbaQuiz.API.Filters
{
  public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is NotFoundException notFoundException)
        { 
            context.Result = new NotFoundObjectResult(new { message = notFoundException.Message });
            context.ExceptionHandled = true; 
        }
    }
}

}