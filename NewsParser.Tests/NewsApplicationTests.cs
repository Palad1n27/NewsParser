using Application.Contracts;
using Application.Services;
using Infrastructure;
using Infrastructure.Contracts;
using Npgsql;

namespace NewsParser.Tests;

public class NewsApplicationTests 
{
    private readonly IApiNewsService _newsService;
    private readonly IDbContext _dbContext;
    private readonly string url = "https://edition.cnn.com/" + "world";
    public NewsApplicationTests()
    {
        _dbContext = new DbContext(new NpgsqlConnection("Server=localhost;Port=5432;Database=best_news;User Id=postgres;Password=2772;"));
        _newsService = new CnnNewsService(_dbContext);
        
    }
    [Fact]
    public async Task GetPostLinks_Return_30_Links()
    { 
        //Act
       var links =await _newsService.GetPostLinks(url);
       
        //Assert
        Assert.Equal(links.Count,30);
    }
    [Fact]
    public async Task FetchNews_Return_30_World_News()
    {
        //Act
        var news = await _newsService.FetchNewsAsync();
        
        //Assert
        Assert.Equal(30,news.Count);
    }
    [Fact]
    public async Task PostNews_Return_30_World_News()
    {
        //Act
        var news = await _newsService.FetchNewsAsync();
        
        //Assert
        Assert.Equal(30,news.Count);
    }

    [Fact]
    public async Task GetPostsByDate_Return_Success()
    {
        var news = await _newsService.GetNewsByDate(DateTime.Now.AddDays(-15),DateTime.Now);
        //Assert

    }
    
    [Fact]
    public async Task GetPostsBySearch_Return_Success()
    {
        var news = await _newsService.GetPostsBySearch("try");
        //Assert

    }
}