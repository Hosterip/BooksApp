using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PostsApp.Domain.Common.Security;

public record HashingResult(string Hash, string Salt);

public static class Hashing
{
    public static bool IsPasswordValid(string userHash, string salt, string password)
    {
        string hashToValidate = GenerateHash(password, StringToByteArray(salt));
        return hashToValidate == userHash;
    }

    public static HashingResult GenerateHashSalt(string password)
    {
        byte[] salt = GenerateSalt();
        string hash = GenerateHash(password, salt);
        return new HashingResult(hash, Convert.ToHexString(salt));
    }
    
    private static string GenerateHash(string password, byte[] salt)
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
}