namespace Infrastructure.Models;

public class RefreshTokenRequest
{
    public Guid Id{ get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
}