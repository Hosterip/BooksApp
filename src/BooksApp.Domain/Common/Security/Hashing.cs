using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PostsApp.Domain.Common.Security;

public static class Hashing
{
    public static bool IsPasswordValid(string userHash, string salt, string password)
    {
        var hashToValidate = GenerateHash(password, StringToByteArray(salt));
        return hashToValidate == userHash;
    }

    public static (string Hash, string Salt) GenerateHashSalt(string password)
    {
        var salt = GenerateSalt();
        var hash = GenerateHash(password, salt);
        return (hash, Convert.ToHexString(salt));
    }

    private static string GenerateHash(string password, byte[] salt)
    {
        var hash = Convert.ToHexString(KeyDerivation.Pbkdf2(
            password,
            salt,
            KeyDerivationPrf.HMACSHA512,
            10000,
            32));
        return hash;
    }

    private static byte[] StringToByteArray(string hex)
    {
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }

    private static byte[] GenerateSalt()
    {
        return RandomNumberGenerator.GetBytes(32);
    }
}