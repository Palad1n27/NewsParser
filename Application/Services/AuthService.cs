using Application.Contracts;
using Domain.Models.DbModels;
using Domain.Models.RequestModels;
using Infrastructure.Contracts;
using Infrastructure.Models;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IDbContext _context;

    public AuthService(IDbContext context)
    {
        _context = context;
    }

    public async Task<(string login, Role role)> RegisterAsync(RegisterRequest request)
    {
        var newUser = new CreateUserRequest
        {
            Id = Guid.NewGuid(),
            Login = request.Login,
            Password = PasswordHasher.Generate(request.Password),
            Role = request.Role,
            RefreshTokenId = Guid.NewGuid(),
            RefreshTokenCreationDate = TimeProvider.System.GetUtcNow().DateTime,
            RefreshTokenExpirationDate = TimeProvider.System.GetUtcNow().DateTime.AddMinutes(2)
        };
        return (newUser.Login, newUser.Role);

    }

    public async Task<(string login,Role role)> LoginAsync(LoginRequest request)
    {
        var userCredentials = await _context.GetUserCredentials(request.Login);
        if (PasswordHasher.Verify(userCredentials.password, request.Password))
            return (request.Login, userCredentials.role);
        
        throw new Exception("Password is incorrect");
    }
}