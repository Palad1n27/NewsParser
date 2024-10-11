using Domain.Models.DbModels;
using Infrastructure.Contracts;
using Infrastructure.Models;
using WebApi.CustomExceptions;

namespace WebApi;

public class TokenProvider
{
    private readonly IDbContext _dbContext;

    public TokenProvider(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GenerateAccessToken(string userLogin, Role role)
    {
        var refreshToken = await _dbContext.GetUserRefreshTokenAsync(userLogin);
        if (refreshToken!.CreationDate < TimeProvider.System.GetUtcNow().DateTime)
            throw new TokenExpiredException();
        
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
        string dateString = accessToken.Substring(2, 5); 
        bool isParsed = DateTime.TryParse(dateString, out DateTime creationDate);

        if (!isParsed || creationDate < TimeProvider.System.GetUtcNow().DateTime)
            return false;
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