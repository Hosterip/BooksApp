using PostsApp.Domain.Common;

namespace Application.UnitTest.Auth.TestUtils.Constants;

public static class HashSaltConstants
{
    private static readonly HashSaltResult HashSalt = HashSaltGen.GenerateHashSalt(PasswordConstants.CorrectPassword);
    public static readonly string Hash = HashSalt.Hash;
    public static readonly string Salt = HashSalt.Salt;
}