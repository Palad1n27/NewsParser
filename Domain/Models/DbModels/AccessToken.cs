namespace Domain.Models.DbModels;

public class AccessToken
{
    public required Guid Id { get; set; }
    public required string UserLogin { get; set; }
    public required Role Role { get; set; }
    public required DateTime CreationDate { get; set; } = TimeProvider.System.GetUtcNow().DateTime;
}