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
    public NewsApplicationTests()
    {
        
    }
    [Fact]
    public async Task GetPostLinks_Return_30_Links()
    { 
        //Act
       var links =await _newsService.GetPostLinks("https://edition.cnn.com/" + "world");
       
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
}