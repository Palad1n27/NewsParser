using WebApi.DDL.DbModels;

namespace WebApi.Contracts;

public interface IDbContext
{
    Task<News> PostNewsAsync(News news);
    Task<List<News>> GetNewsListByDate(DateTime initial, DateTime final);
    Task<List<string>> GetTopicsByDate(string newsName);
    Task<List<News>> GetTopicsByTextSearch(string text);
}