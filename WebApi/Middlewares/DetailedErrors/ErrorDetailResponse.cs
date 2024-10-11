using System.Text.Json;

namespace WebApi.Middlewares.DetailedErrors;

public class ErrorDetailResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public ErrorDetailResponse(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}