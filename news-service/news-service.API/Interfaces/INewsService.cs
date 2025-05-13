namespace news_service.API.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<string>> GetNewsAsync();
        Task<string> AddNewsAsync(string news);
    }
}