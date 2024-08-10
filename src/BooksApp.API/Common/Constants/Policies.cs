namespace PostsApp.Common.Constants;

public static class Policies
{
    public const string Authorized = nameof(Authorized);
    public const string NotAuthorized = nameof(NotAuthorized);
    public const string Admin = nameof(Admin);
    public const string Moderator = nameof(Moderator);
    public const string Author = nameof(Author);
    public const string AdminOrModerator = nameof(AdminOrModerator);
}