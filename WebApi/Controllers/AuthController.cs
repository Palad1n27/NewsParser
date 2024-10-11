using Application.Contracts;
using Domain.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ITokenProvider _tokenProvider;

    public AuthController(IAuthService authService, ITokenProvider tokenProvider)
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
        HttpContext.Session.SetString($"AccessToken",accessToken);
    }
    [HttpPost]
    [ActionName("Login")]
    public async Task<ActionResult> LoginAsync(LoginRequest request)
    {
       var loginResponse = await _authService.LoginAsync(request);
       var accessToken = await _tokenProvider.GenerateAccessToken(request.Login,loginResponse.role);
       HttpContext.Session.SetString($"AccessToken",accessToken);
       return Ok();
    }
    [HttpPost]
    [ActionName("Logout")]
    public async Task<ActionResult> Logout()
    {
        HttpContext.Session.Remove($"AccessToken");
        return Ok();
    }
}