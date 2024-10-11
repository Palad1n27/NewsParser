using Application.Contracts;
using Domain.Models.DbModels;
using Domain.Models.RequestModels;
using Infrastructure.Contracts;
using Infrastructure.Models;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<(string login, Role role)> RegisterAsync(RegisterRequest request)
    {
        var newUser = new CreateUserRequest
        {
            Id = Guid.NewGuid(),
            Login = request.Login,
            Password = _passwordHasher.Generate(request.Password),
            Role = request.Role,
            RefreshTokenId = Guid.NewGuid(),
            RefreshTokenCreationDate = TimeProvider.System.GetUtcNow().DateTime,
            RefreshTokenExpirationDate = TimeProvider.System.GetUtcNow().DateTime.AddMinutes(2)
        };
        await _context.CreateUserAsync(newUser);
        return (newUser.Login, newUser.Role);
    }

    public async Task<(string login,Role role)> LoginAsync(LoginRequest request)
    {
        var userCredentials = await _context.GetUserCredentials(request.Login);
        if (_passwordHasher.Verify(request.Password, userCredentials.password))
            return (request.Login, userCredentials.role);
        
        throw new Exception("Password is incorrect");
    }
}