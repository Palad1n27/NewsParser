namespace WebApi.DDL.DbModels;

public class Topic
{
    public Guid Id { get; set; }
    public Guid SourceId { get; set; }
    public string Name { get; set; }
    public DateTime CreationDateTime { get; set; }
}