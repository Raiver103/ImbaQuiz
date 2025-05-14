namespace news_service.API.Interfaces
{
    public interface INewsRepository
    { 
        Task<string> AddNewsAsync(string news);
        Task<IEnumerable<string>> GetNewsAsync();
    }
}
