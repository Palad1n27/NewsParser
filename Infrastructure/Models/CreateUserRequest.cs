using Domain.Models.DbModels;

namespace Infrastructure.Models;

public class CreateUserRequest
{
    public required Guid Id { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required Role Role { get; set; }
    public required Guid RefreshTokenId { get; set; }
    public required DateTime RefreshTokenCreationDate { get; set; }
    public required DateTime RefreshTokenExpirationDate { get; set; }
}