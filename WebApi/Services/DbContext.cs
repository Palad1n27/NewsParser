using Dapper;
using Npgsql;
using WebApi.Contracts;
using WebApi.DDL.DbModels;

namespace WebApi.Services;

public class DbContext : IDbContext
{
    private readonly  NpgsqlConnection _connection;
    public DbContext(NpgsqlConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<News> PostNewsAsync(News news)
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