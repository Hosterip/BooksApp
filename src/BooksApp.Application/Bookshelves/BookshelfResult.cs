namespace BooksApp.Application.Bookshelves;

public class BookshelfResult
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string ReferentialName { get; init; }
}