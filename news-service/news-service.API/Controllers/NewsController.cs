// Controllers/NewsController.cs
using Microsoft.AspNetCore.Mvc;
using news_service.API.Interfaces;
using news_service.API.Models;

namespace news_service.API.Controllers
{
    [ApiController]
    [Route("api/news")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetNews()
        {
            var news = await _newsService.GetNewsAsync();
            return Ok(news);
        }

        [HttpPost]
        public async Task<IActionResult> AddNews([FromBody] NewsRequest request)
        {  
            var createdNews = await _newsService.AddNewsAsync(request.News);
            return CreatedAtAction(nameof(GetNews), null, createdNews); 
        }
    }
}
