namespace Infrastructure.Models;

public class UpdateRefreshTokenRequest
{
    public required Guid UserId { get; set; }
    public required Guid RefreshTokenId { get; set; }
    public required DateTime CreationDate { get; set; } 
    public required DateTime ExpirationDate { get; set; }
}