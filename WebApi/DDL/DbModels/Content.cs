namespace WebApi.DDL.DbModels;

public class Content
{
    public Guid Id { get; set; }
    public Guid TopicId { get; set; }
    public Content Name { get; set; }
}