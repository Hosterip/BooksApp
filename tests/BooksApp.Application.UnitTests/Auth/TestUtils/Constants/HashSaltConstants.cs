namespace Application.UnitTest.Auth.TestUtils.Constants;

public static class HashSaltConstants
{
    private static readonly (string Hash, string Salt) Hashing =
        PostsApp.Domain.Common.Security.Hashing.GenerateHashSalt(PasswordConstants.CorrectPassword);

    public static readonly string Hash = Hashing.Hash;
    public static readonly string Salt = Hashing.Salt;
}