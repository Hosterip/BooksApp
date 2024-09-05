namespace PostsApp.Application.Common.Constants.ValidationMessages;

public static class BookValidationMessages
{
    public const string NotFound = "Book not found";
    public const string MustBeAnAuthor = "User must be with role of Author";
    public const string WithSameNameAlreadyExists = "You already have book with the same name";
    public const string PostNotYour = "Book not yours or not enough privileges";
}