using Application.Contracts;
using Domain.Models.DbModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TopicController : Controller
{
    private readonly IApiNewsService _apiNewsService;

    public TopicController(IApiNewsService apiNewsService)
    {
        _apiNewsService = apiNewsService;
    }

    [HttpGet]
    [ActionName("posts")]
    public async Task<ActionResult<List<Post>>> GetPostsByDate([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
       return Ok(await _apiNewsService.GetNewsByDate(from, to));
    }
    [HttpGet]
    [ActionName("search")]
    public async Task<ActionResult<List<Post>>> GetPostsBySearch([FromQuery] string text)
    {
        return Ok(await _apiNewsService.GetPostsBySearch(text));
    }
    [HttpGet]
    [ActionName("topten")]
    public async Task<ActionResult<List<Post>>> GetTopTenWords()
    {
        return Ok(await _apiNewsService.GetPopularWordsInNews());
    }
}