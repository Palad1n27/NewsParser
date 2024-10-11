using System.Text.RegularExpressions;
using Application.Contracts;
using Domain.Models.DbModels;
using Infrastructure.Contracts;

namespace Application.Services;

public class CnnNewsService : IApiNewsService
{
    private readonly HttpClient _client = new();
    private readonly IDbContext _context;
    public CnnNewsService(IDbContext context)
    {
        _context = context;
    }

    public async Task<List<Post>> FetchNewsAsync()
    {
        
        var links = await GetPostLinks("https://edition.cnn.com/" + "world");
        var newsList = new List<Post>();

        for (var i = 0; i < links.Count; i++)
        {
            var news = await ParseWebPage(links[i]);
            if(string.IsNullOrEmpty(news.Content))
                continue;
            newsList.Add(news);
        }

        await _context.PostNewsAsync(newsList);
        return newsList;
    }

    public async Task<List<Post>> GetNewsByDate(DateTime initialDate, DateTime finalDate)
    {
        return await _context.GetNewsListByDate(initialDate,finalDate);
    }

    public Task<List<Post>> GetPopularWordsInNews( )
    {
        throw new NotImplementedException();
    }

    public async Task<List<Post>> GetPostsBySearch(string searchText)
    {
        return await _context.GetPostsBySearch(searchText);
    }

    public async Task<Post> ParseWebPage(string link) 
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
        
        
        return new Post
        {
            Id = link,
            Name = clearedTitle,
            Content = clearedBody,
            CreationDate = creationDate
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