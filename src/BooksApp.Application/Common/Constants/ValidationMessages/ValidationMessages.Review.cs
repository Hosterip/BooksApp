namespace BooksApp.Application.Common.Constants.ValidationMessages;

public partial class ValidationMessages
{
    public static class Review
    {
        public const string NotFound = "Review not found";
        public const string AlreadyHave = "You already have a review to this book";
        public const string NotYours = "Review is not yours";
    }
}