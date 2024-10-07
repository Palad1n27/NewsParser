using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;
using WebApi.DDL.DbModels;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TopicController : Controller
{
    private readonly IApiClientService _apiClientService;

    public TopicController(IApiClientService apiClientService)
    {
        _apiClientService = apiClientService;
    }


    [HttpGet]
    [ActionName("GetSortedByDate")]
    public async Task<ActionResult<List<News>>> GetTopics()
    {
        return await _apiClientService.FetchNewsAsync();
    }
}