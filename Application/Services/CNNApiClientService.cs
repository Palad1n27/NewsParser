using System.Text.RegularExpressions;
using Application.Contracts;
using Infrastructure.Contracts;
using WebApi.DDL.DbModels;

namespace Application.Services;

public class CnnNewsService : IApiNewsService
{
    private readonly HttpClient _client = new();
    private readonly IDbContext _context;
    public CnnNewsService(IDbContext context)
    {
        _context = context;
    }

    public async Task<List<News>> FetchNewsAsync()
    {
        string[] themes = { "china", "americans", "asia", "world", "united-kingdom" };

        var theme = themes[new Random().Next(0, 5)];
        var links = await GetPostLinks("https://edition.cnn.com/" + "world");
        var newsList = new List<News>();

        for (var i = 0; i < links.Count; i++)
        {
            var news = await ParseWebPage(links[i]);
            newsList.Add(news);
        }

        return newsList;
    }

    public async Task<IEnumerable<News>> GetNewsByDate(string url, DateTime initialDate, DateTime finalDate)
    {
        return await _context.GetNewsListByDate(initialDate,finalDate);
    }

    public List<Task<News>> GetPopularWordsInNews(string url, DateTime initialDate, DateTime finalDate)
    {
        throw new NotImplementedException();
    }

    public List<Task<News>> GetNewsByText(string url, string searchText)
    {
        throw new NotImplementedException();
    }

    public async Task<News> ParseWebPage(string link) 
    {
        var responseMessage = await _client.GetAsync("https://edition.cnn.com" + link);
        
        var stringDate = link.Substring(1, 10);
        var creationDate = DateTime.Parse(stringDate);
        
        var html = await responseMessage.Content.ReadAsStringAsync();
        var titlePattern = @"<title>(.*?)<\/title>";
        var bodyPattern = @"<div class=""article__content-container"">.*?<div class=""article__content""[^>]*>(.*?)<\/div>\s*<\/div>";
        var clearPattern = @"<[^>]+>|<!--.*?-->";

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

        var clearedBody = string.Join(" ", results);
        clearedBody = Regex.Replace(clearedBody, @"^CNN\s*&nbsp;&mdash;&nbsp;", string.Empty);
        clearedBody = Regex.Replace(clearedBody, @"\s+", " ").Trim();
        
        
        return new News
        {
            Title = clearedTitle,
            Content = clearedBody,
            CreationDateTime = creationDate
        };
    }

    public async Task<List<string>> GetPostLinks(string link)
    {
        var response = await _client.GetAsync(link);
        var stringResponse = await response.Content.ReadAsStringAsync();
        
        var linkPattern = @"<a\s+href=""([^""]+)""\s+class=""[^""]*container__link[^""]*""";

        var linkCollections =
            Regex.Matches(stringResponse, linkPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        var extractedLinks = new List<string>();

        foreach (Match match in linkCollections)
        {
            var extractedLink = match.Groups[1].Value.Trim();

            if (!extractedLinks.Contains(extractedLink) && extractedLink[1] == '2' && extractedLink[2] == '0')
                extractedLinks.Add(extractedLink);
            if (extractedLinks.Count >= 30)
                break;
        }

        return extractedLinks;
    }
}