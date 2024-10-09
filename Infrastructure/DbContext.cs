using Dapper;
using Infrastructure.Contracts;
using Npgsql;
using WebApi.DDL.DbModels;

namespace Infrastructure;

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

    public async Task<IEnumerable<News>> GetNewsListByDate(DateTime initial, DateTime final)
    {
        string query = $@"select id, name, creation_date, content from posts
                                        where creation_date >= @Initial and creation_date <= @Final";

        return await _connection.QueryAsync<News>(query, new{Initial = initial, Final = final});
    }

    public Task<List<string>> GetPopularWordsInNews(string newsName)
    {
        throw new NotImplementedException();
    }

    public Task<List<News>> GetTopicsByTextSearch(string text)
    {
        throw new NotImplementedException();
    }
}