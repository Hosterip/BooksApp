namespace PostsApp.Domain.Constants.Exceptions;

public static class BookExceptionConstants
{
    public static readonly string NotFound = "Post not found";
    public static readonly string UserMustBeCorrect = "User must be correct";
    public static readonly string MustBeAnAuthor = "User must be with role of Author";
    public static readonly string PostNotYour = "Post not yours or not enough privileges";
}