namespace WebApi.CustomExceptions;

public class TokenExpiredException : SystemException
{
    public TokenExpiredException() 
        : base("Your token is expired")
    {
    }
}