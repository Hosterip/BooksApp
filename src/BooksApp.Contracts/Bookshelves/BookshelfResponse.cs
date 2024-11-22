namespace BooksApp.Contracts.Bookshelves;

public sealed class BookshelfResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}