using WebApi.Contracts;

namespace WebApi.Services;

public class ApiClientService : IApiClientService
{
    public object GetTopicInfo(string url)
    {
        throw new NotImplementedException();
    }

    public List<object> GetPopularNewsByDate(string url, DateTime initialDate, DateTime finalDate)
    {
        throw new NotImplementedException();
    }

    public List<object> GetNewsByText(string url, string searchText)
    {
        throw new NotImplementedException();
    }
}