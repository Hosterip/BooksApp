using PostsApp.Domain.Security;

namespace Application.UnitTest.Auth.TestUtils.Constants;

public static class HashSaltConstants
{
    private static readonly HashingResult Hashing = PostsApp.Domain.Security.Hashing.GenerateHashSalt(PasswordConstants.CorrectPassword);
    public static readonly string Hash = Hashing.Hash;
    public static readonly string Salt = Hashing.Salt;
}