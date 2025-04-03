using Microsoft.AspNetCore.Mvc;

namespace ImbaQuiz.API.Controllers
{
  [Route("error")]
  public class ErrorController : ControllerBase
  {
      [HttpGet]
      public IActionResult HandleError()
      {
          var error = new { message = "An unexpected error occurred." };
          return StatusCode(500, error);
      }
  } 
}