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
        return await _context.GetNewsListByDateAsync(initialDate,finalDate);
    }

    public async Task<List<string>> GetPopularWordsInNews()
    {

        var posts = await _context.GetAllPosts();
        var allWordsScoring = new List<ScoredWord>();
        var top10Words = new List<string>();
        var symbols = new List<string>
        {
            ",", ".", "/", "!", "#", "{", "}", "(", ")", "_", "-", "—", ";", ":","”"
        };
        var allNotAppropriateWords = NotAppropriateWords;
        
        foreach (var post in posts)
        {
            var content = post.Content;

            foreach (var symbol in symbols)
            {
               content = content.Replace(symbol, " ");
            }

            var splitedWordsInPost = content.Split(" ");
            for (int i = 0; i < splitedWordsInPost.Length; i++)
            {
                var scoredWord = new ScoredWord
                { 
                    Score = 1,
                    Word = splitedWordsInPost[i].ToLower().Trim()
                };
                if (!allNotAppropriateWords.Contains(scoredWord.Word))
                { 
                    allWordsScoring.Add(scoredWord);
                }
            }
            
        }
        var scoredWords = allWordsScoring
            .GroupBy(x => x.Word)
            .Select(g => new 
            {
                Word = g.Key,      
                Score = g.Count()  
            }).OrderByDescending(w => w.Score).Take(10)
            .ToList();  
        
        for (int i = 0; i < scoredWords.Count; i++)
        {
            top10Words.Add(scoredWords[i].Word); 
        }
        
        return top10Words;
    }

    public async Task<List<Post>> GetPostsBySearch(string searchText)
    {
        return await _context.GetPostsBySearchAsync(searchText);
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
        clearedTitle = Regex.Replace(clearedTitle, @"&nbsp;", " ");
        var bodyMatches = Regex.Matches(html, bodyPattern, RegexOptions.Singleline);
        var results = new List<string>();

        foreach (Match match in bodyMatches)
        {
            var paragraphText = Regex.Replace(match.Groups[1].Value, clearPattern, string.Empty).Trim();
            results.Add(paragraphText);
        }

        var clearedBody = string.Join(" ", results);
        clearedBody = Regex.Replace(clearedBody, @"^CNN\s*&nbsp;&mdash;&nbsp;", string.Empty);
        clearedBody = Regex.Replace(clearedBody, @"&nbsp;", " ");
        clearedBody = Regex.Replace(clearedBody, @"&mdash;", "—");
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

    private List<string> NotAppropriateWords => new List<string>{ "about", "above", "across", "after", "against", "along", "among", "around", 
            "at", "before", "behind", "below", "beneath", "beside", "between", "beyond", 
            "by", "despite", "during", "except", "for", "from", "in", "inside", "into", 
            "like", "near", "of", "off", "on", "onto", "out", "outside", "over", 
            "through", "to", "toward", "under", "until", "up", "with", "without",
            "a", "an", "the", "and", "but", "or", "nor", "for", "so", "yet",
            "to", "in", "of", "on", "at", "by", "with", "as", "from", "about",
            "into", "during", "before", "after", "above", "below", "between",
            "among", "without", "like", "up", "down", "near", "along", "through",
            "except", "despite", "whether", "if", "that", "who", "whom", "whose",
            "which", "what", "when", "where", "how", "why", "just", "only", 
            "also", "even", "still", "yet", "both", "each", "every", "some",
            "any", "no", "few", "more", "most", "several", "many", "much", 
            "such", "very", "too", "rather", "quite", "almost", "nearly",
            "although", "though", "while", "whereas", "if", "unless", "until", 
            "when", "as", "because", "since", "before", "after", "while", 
            "whether", "both", "either", "neither", 
            "I", "me", "you", "your", "yours", "he", "him", "his", "she", "her", 
            "hers", "it", "its", "we", "us", "our", "ours", "they", "them", 
            "their", "theirs", "who", "whom", "whose", "this", "that", "these", 
            "those", "one", "another", "anyone", "someone", "everyone", "no one", 
            "anybody", "somebody", "everybody", "nobody", "anything", "something", 
            "everything", "nothing", "was", "is", "has", "were", "have", "are", "be", 
            "been", "being", "am", "not", "do", "does", "did", "doing", 
            "there", "here", "my", "mine", "can", "could", "shall", "should", 
            "will", "would", "may", "might", "must", "said", "told", "tell", 
            "says", "saying","","according","had","cnn","other"};

}