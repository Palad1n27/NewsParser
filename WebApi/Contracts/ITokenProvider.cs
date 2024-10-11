using Domain.Models.DbModels;

namespace WebApi.Contracts;

public interface ITokenProvider
{
    Task UpdateRefreshToken(Guid userId);
    Task<bool> CheckAccessToken(string accessToken);
    Task<string> GenerateAccessToken(string userLogin, Role role);
}