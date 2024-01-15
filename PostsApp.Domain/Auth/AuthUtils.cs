using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PostsApp.Domain.Auth;

public static class AuthUtils
{
    public static string CreateHash(string password, byte[] salt)
    {
        string hash = Convert.ToHexString(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 10000,
            numBytesRequested: 32));
        return hash;
    }
    
    public static byte[] StringToByteArray(string hex) {
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }
}