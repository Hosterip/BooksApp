namespace PostsApp.Application.Common.Constants.Exceptions;

public static class ConstantsBookException
{
    public const string NotFound = "Post not found";
    public const string MustBeAnAuthor = "User must be with role of Author";
    public const string PostNotYour = "Post not yours or not enough privileges";
}