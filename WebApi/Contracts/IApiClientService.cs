namespace WebApi.Contracts;

public interface IApiClientService
{
    Object GetTopicInfo(string url);
    
    List<Object> GetPopularNewsByDate(string url, DateTime initialDate, DateTime finalDate);

    List<Object> GetNewsByText(string url, string searchText);
}