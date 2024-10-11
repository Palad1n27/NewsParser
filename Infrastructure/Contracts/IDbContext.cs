using Domain.Models.DbModels;
using Domain.Models.RequestModels;
using Infrastructure.Models;

namespace Infrastructure.Contracts;

public interface IDbContext
{
    Task<List<Post>> PostNewsAsync(List<Post> newPosts);
    Task<List<Post>> GetAllPosts();
    Task<List<Post>> GetNewsListByDateAsync(DateTime initial, DateTime final);
    Task<List<Post>> GetPostsBySearchAsync(string text);

    Task CreateUserAsync(CreateUserRequest request);
    Task<RefreshTokenRequest?> GetUserRefreshTokenAsync(string userLogin);
    Task UpdateRefreshToken(UpdateRefreshTokenRequest request);
    
    Task<(string password, Role role)> GetUserCredentials(string login);

}