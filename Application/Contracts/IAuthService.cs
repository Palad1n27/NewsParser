using Domain.Models.DbModels;
using Domain.Models.RequestModels;

namespace Application.Contracts;

public interface IAuthService
{
    Task<(string login, Role role)> RegisterAsync(RegisterRequest request);
    Task<(string login, Role role)> LoginAsync(LoginRequest request);
}