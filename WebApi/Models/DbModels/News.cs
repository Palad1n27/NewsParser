namespace WebApi.DDL.DbModels;

public class News
{
    public Guid Id { get; set; }
    public Guid SourceId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime? CreationDateTime { get; set; }
}