namespace Domain.Models.DbModels;

public class Post
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Content { get; set; }
    public required DateTime? CreationDate { get; set; }
}