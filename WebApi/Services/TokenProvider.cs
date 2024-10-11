using Domain.Models.DbModels;
using Infrastructure.Contracts;
using Infrastructure.Models;
using WebApi.Contracts;
using WebApi.CustomExceptions;

namespace WebApi.Services;

public class TokenProvider : ITokenProvider
{
    private readonly IDbContext _dbContext;

    public TokenProvider(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GenerateAccessToken(string userLogin, Role role)
    {
        return new AccessToken
        {
            Id = Guid.NewGuid(),
            UserLogin = userLogin,
            Role = role,
            CreationDate = TimeProvider.System.GetUtcNow().DateTime
        }.ToString()!;
    }

    public async Task<bool> CheckAccessToken(string accessToken)
    {
        RefreshTokenRequest token;
        if (accessToken is null)
            throw new Exception("Unauthorized");
        
        return true;
    }

    public async Task UpdateRefreshToken(Guid userId)
    {
        var updatedRefreshToken = new UpdateRefreshTokenRequest
        {
            UserId = userId,
            RefreshTokenId = Guid.NewGuid(),
            CreationDate = TimeProvider.System.GetUtcNow().DateTime,
            ExpirationDate = TimeProvider.System.GetUtcNow().DateTime.AddMinutes(2)
        };

        await _dbContext.UpdateRefreshToken(updatedRefreshToken);
    }
}