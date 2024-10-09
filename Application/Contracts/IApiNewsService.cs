using WebApi.DDL.DbModels;

namespace Application.Contracts;

public interface IApiNewsService
{
    Task<List<string>> GetPostLinks(string link);
    Task<List<News>> FetchNewsAsync();

    Task<IEnumerable<News>> GetNewsByDate(string url, DateTime initialDate, DateTime finalDate);
    
    List<Task<News>> GetPopularWordsInNews(string url, DateTime initialDate, DateTime finalDate);

    List<Task<News>> GetNewsByText(string url, string searchText);
}