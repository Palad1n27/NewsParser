using Application.Contracts;
using Domain.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TopicController : Controller
{
    private readonly IApiNewsService _apiNewsService;
    private readonly ITokenProvider _tokenProvider;

    public TopicController(IApiNewsService apiNewsService,ITokenProvider tokenProvider)
    {
        _apiNewsService = apiNewsService;
        _tokenProvider = tokenProvider;
    }

    [HttpGet]
    [ActionName("posts")]
    public async Task<ActionResult<List<Post>>> GetPostsByDate([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var accessToken = HttpContext.Session.GetString("AccessToken");
        await _tokenProvider.CheckAccessToken(accessToken!);
        return Ok(await _apiNewsService.GetNewsByDate(from, to));
    }
    [HttpGet]
    [ActionName("fetch")]
    public async Task<ActionResult<List<Post>>> Fetch()
    {
        var accessToken = HttpContext.Session.GetString("AccessToken");
        await _tokenProvider.CheckAccessToken(accessToken!);
        if (accessToken.Substring(0, 5) is not "Admin")
            throw new Exception("No access");
        
        return Ok(await _apiNewsService.FetchNewsAsync());
    }
    [HttpGet]
    [ActionName("search")]
    public async Task<ActionResult<List<Post>>> GetPostsBySearch([FromQuery] string text)
    {
        var accessToken = HttpContext.Session.GetString("AccessToken");
        await _tokenProvider.CheckAccessToken(accessToken!);
        return Ok(await _apiNewsService.GetPostsBySearch(text));
    }
    [HttpGet]
    [ActionName("topten")]
    public async Task<ActionResult<List<Post>>> GetTopTenWords()
    {
        var accessToken = HttpContext.Session.GetString("AccessToken");
        await _tokenProvider.CheckAccessToken(accessToken!);
        return Ok(await _apiNewsService.GetPopularWordsInNews());
    }
}