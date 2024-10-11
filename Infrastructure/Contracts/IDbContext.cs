using Domain.Models.DbModels;

namespace Infrastructure.Contracts;

public interface IDbContext
{
    Task<List<Post>> PostNewsAsync(List<Post> newPosts);
    Task<List<Post>> GetNewsListByDate(DateTime initial, DateTime final);
    Task<List<string>> GetPopularWordsInNews();
    Task<List<Post>> GetPostsBySearch(string text);
}