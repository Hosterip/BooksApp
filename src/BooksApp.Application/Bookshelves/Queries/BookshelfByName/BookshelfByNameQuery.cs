using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.BookshelfByName;

public class BookshelfByNameQuery : IRequest<BookshelfResult>
{
    public required string Name { get; init; }
    public required Guid UserId { get; init; }
}