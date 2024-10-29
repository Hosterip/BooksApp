namespace BooksApp.Application.Common.Constants.ValidationMessages;

public class BookshelfValidationMessages
{
    public const string NotFound = "Bookshelf was not found";
    public const string NotYours = "Bookshelf is not yours";
    public const string AlreadyExists = "Book already exists in given bookshelf";
    public const string AlreadyHaveWithSameName = "You already have bookshelf with the same name";
    public const string NoBookToRemove = "There is no book with this bookId to delete";
}