using System.Data;
using System.Net;
using WebApi.Middlewares.DetailedErrors;

namespace WebApi.Middlewares;

public class HttpExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HttpExceptionMiddleware> _logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    public HttpExceptionMiddleware(RequestDelegate next, ILogger<HttpExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {            
            await _next.Invoke(context);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogTrace("Object not found. {Exception}. {StackTrace}", ex.Message, ex.StackTrace);
            await SendResponseHandler(context, (int)HttpStatusCode.BadRequest, ex.Message, ex);
        }
        catch (DuplicateNameException ex)
        {
            _logger.LogTrace("User try to register with duplicate data {Exception}", ex.Message);
            await SendResponseHandler(context, (int)HttpStatusCode.BadRequest, ex.Message, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: {Message}", ex.Message);
            await SendResponseHandler(context, (int)HttpStatusCode.InternalServerError, ex.Message, ex);
        }
    }

    private async Task SendResponseHandler(HttpContext context, int statusCode, string message, Exception exception)
    {
        if (context.Response.HasStarted)
            throw exception;

        var errorDetails = new ErrorDetailResponse(statusCode, message);
        var jsonResponse = errorDetails.ToString();

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        context.Response.ContentLength = jsonResponse.Length;

        await context.Response.WriteAsync(jsonResponse);
    }
}