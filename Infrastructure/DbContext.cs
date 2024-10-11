using Dapper;
using Domain.Models.DbModels;
using Infrastructure.Contracts;
using Npgsql;
namespace Infrastructure;

public class DbContext : IDbContext
{
    private readonly  NpgsqlConnection _connection;
    public DbContext(NpgsqlConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<List<Post>> PostNewsAsync(List<Post> newPosts)
    {
        
        string insertQuery = $@"insert into posts(id, name, creation_date, content) 
                                values(@Id,
                                       @Name,
                                       @Creation_Date,
                                       @Content)";

            foreach (var post in newPosts)
            {
                await _connection.ExecuteAsync(insertQuery, new {post.Id, post.Name, Creation_Date = post.CreationDate, post.Content });
            }
        

        return newPosts;
    }

    public async Task<List<Post>> GetNewsListByDate(DateTime initial, DateTime final)
    {
        string selectQuery = $@"select id, name, creation_date, content from posts
                                        where creation_date >= @Initial and creation_date <= @Final";

        return (await _connection.QueryAsync<Post>(selectQuery, new{Initial = initial, Final = final})).ToList();
    }

    public async Task<List<string>> GetPopularWordsInNews()
    {
        string selectQuery = $@"select id, name, creation_date, content from posts";
        var posts =  (await _connection.QueryAsync<Post>(selectQuery)).ToList();
        var topWords = new List<string>();
        foreach (var post in posts)
        {
            
            
        }

        return null;
    }

    public async Task<List<Post>> GetPostsBySearch(string text)
    {
        string selectQuery = $@"select id, name, creation_date, content from posts";

        return (await _connection.QueryAsync<Post>(selectQuery)).Where(post => post.Content.Contains(text) || post.Name.Contains(text))
            .ToList();
    }
}