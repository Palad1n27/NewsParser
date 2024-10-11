using Domain.Models.DbModels;

namespace Application.Contracts;

public interface IApiNewsService
{
    Task<List<string>> GetPostLinks(string link);
    Task<List<Post>> FetchNewsAsync();

    Task<List<Post>> GetNewsByDate(DateTime initialDate, DateTime finalDate);
    
    Task<List<string>> GetPopularWordsInNews();

    Task<List<Post>> GetPostsBySearch( string searchText);
}