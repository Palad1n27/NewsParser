namespace Application.Contracts;

public interface IPasswordHasher
{
    bool Verify(string password, string hash);
    string Generate(string password);
}