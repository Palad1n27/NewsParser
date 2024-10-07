using Npgsql;
using WebApi.Contracts;
using WebApi.DDL.DbModels;

namespace WebApi.Services;

public class DbContext : IDbContext
{
    private readonly  NpgsqlConnection _connection;
    private readonly IApiClientService _apiClientService;
    public DbContext(NpgsqlConnection connection, IApiClientService apiClientService)
    {
        _connection = connection;
        _apiClientService = apiClientService;
    }
    
    public Task<News> PostNewsAsync(News news)
    {
        throw new NotImplementedException();
    }

    public Task<List<News>> GetNewsListByDate(DateTime initial, DateTime final)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetTopicsByDate(string newsName)
    {
        throw new NotImplementedException();
    }

    public Task<List<News>> GetTopicsByTextSearch(string text)
    {
        throw new NotImplementedException();
    }
}