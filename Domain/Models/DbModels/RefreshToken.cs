namespace Domain.Models.DbModels;

public class RefreshToken
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
}