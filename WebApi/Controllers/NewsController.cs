using Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using WebApi.DDL.DbModels;

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
    
}