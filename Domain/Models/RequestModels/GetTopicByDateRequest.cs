namespace Domain.Models.RequestModels;

public class GetTopicByDateRequest
{
    public DateTime InitDate { get; set; }
    public DateTime FinishDate { get; set; }
}