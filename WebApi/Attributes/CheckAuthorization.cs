using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Services;

namespace WebApi.Attributes;

public class CheckAuthorization : Attribute, IAsyncAuthorizationFilter
{
    private readonly TokenProvider _tokenProvider;

    public CheckAuthorization(TokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        var accessToken = httpContext.Session.GetString("AccessToken");
        var isAuthorized = await _tokenProvider.CheckAccessToken(accessToken!);
        if (isAuthorized is not true)
            throw new UnauthorizedAccessException();
    }
}