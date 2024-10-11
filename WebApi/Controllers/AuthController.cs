using Application.Contracts;
using Domain.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly TokenProvider _tokenProvider;

    public AuthController(IAuthService authService, TokenProvider tokenProvider)
    {
        _authService = authService;
        _tokenProvider = tokenProvider;
    }

    [HttpPost]
    [ActionName("Register")]
    public async Task RegisterAsync(RegisterRequest request)
    {
        var userCredentials = await _authService.RegisterAsync(request);
        var accessToken = await _tokenProvider.GenerateAccessToken(userCredentials.login, userCredentials.role);
        HttpContext.Session.SetString($"AccessToken{request.Login}",accessToken);
    }
    [HttpPost]
    [ActionName("Login")]
    public async Task<ActionResult> LoginAsync(LoginRequest request)
    {
       var loginResponse = await _authService.LoginAsync(request);
       var accessToken = await _tokenProvider.GenerateAccessToken(request.Login,loginResponse.role);
       HttpContext.Session.SetString($"AccessToken{request.Login}",accessToken);
       return Ok();
    }
}