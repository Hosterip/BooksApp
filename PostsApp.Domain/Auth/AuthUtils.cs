using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PostsApp.Domain.Auth;

public record HashSaltResult(string hash, string salt);

public static class AuthUtils
{
    private static string CreateHash(string password, byte[] salt)
    {
        string hash = Convert.ToHexString(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 10000,
            numBytesRequested: 32));
        return hash;
    }
    
    private static byte[] StringToByteArray(string hex) {
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }

    private static byte[] GenerateSalt()
    {
        return RandomNumberGenerator.GetBytes(32);
    }

    public static bool IsPasswordValid(string userHash, string salt, string password)
    {
        string hashToValidate = CreateHash(password, StringToByteArray(salt));
        return hashToValidate == userHash;
    }

    public static HashSaltResult CreateHashSalt(string password)
    {
        byte[] salt = GenerateSalt();
        string hash = CreateHash(password, salt);
        return new HashSaltResult(hash, Convert.ToHexString(salt));
    }
}