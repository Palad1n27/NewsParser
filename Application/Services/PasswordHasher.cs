using System.Security.Cryptography;

namespace Application.Services;

public static class PasswordHasher
{
    private const int SaltSize = 128 / 8;
    private const int KeySize = 256 / 8;
    private const int Iterations = 10000;
    
    private static readonly HashAlgorithmName HashAlgorithmName = HashAlgorithmName.SHA512;
    private const char Delimiter = ';';

    public static string Generate(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, KeySize);

        return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public static bool Verify(string password, string hash)
    {
        var elements = hash.Split(Delimiter);
        var salt = Convert.FromBase64String(elements[0]);
        var hashToVerify = Convert.FromBase64String(elements[1]);

        var hashToVerifyWithSalt = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, KeySize);

        return CryptographicOperations.FixedTimeEquals(hashToVerify, hashToVerifyWithSalt);
    }
}