using WebApi.DDL.DbModels;

namespace WebApi.Contracts;

public interface IApiClientService
{
    Task<List<News>> FetchNewsAsync();
    
    List<Task<News>> GetNewsByDate(string url, DateTime initialDate, DateTime finalDate);
    
    List<Task<News>> GetPopularNews(string url, DateTime initialDate, DateTime finalDate);

    List<Task<News>> GetNewsByText(string url, string searchText);
}