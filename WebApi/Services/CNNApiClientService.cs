using System.Text.RegularExpressions;
using WebApi.Contracts;
using WebApi.DDL.DbModels;

namespace WebApi.Services;

public class ApiClientService : IApiClientService
{
    private readonly HttpClient _client = new();
    private readonly DbContext _dbContext;

    public ApiClientService(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<News>> FetchNewsAsync()
    {
        var links = new List<string>();
        string[] themes = {"china", "americans", "asia", "world", "united-kingdom"};

        var newsList = new List<News>();
        for (int i = 0; i < themes.Length; i++)
        {
            var linksFromTheme = await GetPostLinks(themes[i]);
            for (int j = 0; j < linksFromTheme.Count; j++)
            {
                links.Add(linksFromTheme[j]);
            }
        }
        for (int i = 0; i < links.Count; i++)
        {
            var news = await ParseWebPage(links[i]);
            newsList.Add(news);
            await _dbContext.PostNewsAsync(news);
        }
        return newsList;
    }


    public List<Task<News>> GetNewsByDate(string url, DateTime initialDate, DateTime finalDate)
    {
        throw new NotImplementedException();
    }

    public List<Task<News>> GetPopularNews(string url, DateTime initialDate, DateTime finalDate)
    {
        throw new NotImplementedException();
    }

    public List<Task<News>> GetNewsByText(string url, string searchText)
    {
        throw new NotImplementedException();
    }

    public async Task<News> ParseWebPage(string html)
    {
        var titlePattern = @"<title>(.*?)<\/title>";
        var bodyPattern =
            @"<div class=""article__content-container"">.*?<div class=""article__content""[^>]*>(.*?)<\/div>\s*<\/div>";
        var clearPattern = @"<[^>]+>|<!--.*?-->";
        var datePattern = @"Published\s+(\d{1,2}:\d{2}\s[APM]{2}\s\w{3},\s\d{1,2}\s\w+\s\d{4})";

        var dateMatch = Regex.Match(html, datePattern, RegexOptions.IgnoreCase);
        var createdDateTime = DateTime.Today;

        var titleMatch = Regex.Match(html, titlePattern, RegexOptions.IgnoreCase);
        var clearedTitle = titleMatch.Success
            ? Regex.Replace(titleMatch.Groups[1].Value, clearPattern, string.Empty).Trim()
            : string.Empty;

        clearedTitle = clearedTitle.Replace("|CNN", "").Trim();

        var bodyMatches = Regex.Matches(html, bodyPattern, RegexOptions.Singleline);
        var results = new List<string>();

        foreach (Match match in bodyMatches)
        {
            var paragraphText = Regex.Replace(match.Groups[1].Value, clearPattern, string.Empty).Trim();
            results.Add(paragraphText);
        }

        if (dateMatch.Success)
        {
            var dateString = dateMatch.Groups[1].Value.Trim();
            dateString = dateString.Replace("Published", "").Trim();

            if (DateTime.TryParse(dateString, out var parsedDate)) createdDateTime = parsedDate;
        }

        var clearedBody = string.Join(" ", results);
        clearedBody = Regex.Replace(clearedBody, @"^CNN\s*&nbsp;&mdash;&nbsp;", string.Empty);
        clearedBody = Regex.Replace(clearedBody, @"\s+", " ").Trim();

        return new News
        {
            Title = clearedTitle,
            Content = clearedBody,
            CreationDateTime = createdDateTime
        };
    }

    public async Task<List<string>> GetPostLinks(string link)
    {
        var response = await _client.GetAsync(link);
        var stringResponse = await response.Content.ReadAsStringAsync();
        
        var linkRegex = new Regex(@"<a[^>]*href=""(.*?)""[^>]*>", RegexOptions.IgnoreCase);
        MatchCollection linkCollections = linkRegex.Matches(stringResponse);

        List<string> extractedLinks = new List<string>();
        
        foreach (Match match in linkCollections)
        {
            string extractedLink = match.Groups[1].Value;
            extractedLinks.Add(extractedLink);
        }

        return extractedLinks;
    }
}