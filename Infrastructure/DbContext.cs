using Dapper;
using Domain.Models.DbModels;
using Infrastructure.Contracts;
using Infrastructure.Models;
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

    public async Task<List<Post>> GetNewsListByDateAsync(DateTime initial, DateTime final)
    {
        string selectQuery = $@"select id, name, creation_date, content from posts
                                        where creation_date >= @Initial and creation_date <= @Final";

        return (await _connection.QueryAsync<Post>(selectQuery, new{Initial = initial, Final = final})).ToList();
    }

    public async Task<List<string>> GetPopularWordsInNewsAsync()
    {
        string selectQuery = $@"select id, name, creation_date, content from posts";
        var posts =  (await _connection.QueryAsync<Post>(selectQuery)).ToList();
        var topWords = new List<string>();
        foreach (var post in posts)
        {
            
            
        }

        return null;
    }

    public async Task<List<Post>> GetPostsBySearchAsync(string text)
    {
        string selectQuery = $@"select id, name, creation_date, content from posts";

        return (await _connection.QueryAsync<Post>(selectQuery)).Where(post => post.Content.Contains(text) || post.Name.Contains(text))
            .ToList();
    }

    public async Task CreateUserAsync(CreateUserRequest request)
    {
        string insertQuery = $@"insert into users(id, login, password, role, refresh_token,
                                refresh_token_creation_date, refresh_token_expiration_date) 
                                values(@Id,
                                       @Login,
                                       @Password,
                                       @Role,
                                       @RefreshToken,
                                       @RefreshTokentCreationDate,
                                       @RefreshTokenExpirationDate)";

        await _connection.ExecuteAsync(insertQuery, 
            new
            {
                request.Id,request.Login, HashedPassword = request.Password,request.Role,
                request.RefreshTokenId,request.RefreshTokenCreationDate,request.RefreshTokenExpirationDate
            });
    }

    public async Task<RefreshTokenRequest?> GetUserRefreshTokenAsync(string userLogin)
    {
        var selectQuery = $@"select id, login, role, refresh_token, refresh_token_creation_date,
                             refresh_token_expiration_date from users where login = @UserLogin";
        return await _connection.QueryFirstOrDefaultAsync<RefreshTokenRequest>(selectQuery, new { UserLogin = userLogin });
    }

    public async Task UpdateRefreshToken(UpdateRefreshTokenRequest request)
    {
        var updateQuery = $@"update users set refresh_token = @RefreshToken,
                             refresh_token_creation_date = @RefreshTokenCreationDate,
                             refresh_token_expiration_date = @RefreshTokenExpirationDate
                             from users where id = @UserId";
         await _connection.ExecuteAsync(updateQuery, new {request.RefreshTokenId, request.CreationDate,
             request.ExpirationDate, request.UserId});
    }

    public async Task<(string,Role)> GetUserCredentials(string login)
    {
        var selectQuery = $@"select password, role from users where login = @Login";
        return await _connection.QueryFirstOrDefaultAsync<(string,Role)>(selectQuery, new { Login = login });
    }
}