using news_service.API.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace news_service.API.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _repository;

        public NewsService(INewsRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> AddNewsAsync(string news)
        { 
            await _repository.AddNewsAsync(news);
            return news;
        }

        public async Task<IEnumerable<string>> GetNewsAsync()
        {
            return await _repository.GetNewsAsync();
        }
    }
}
