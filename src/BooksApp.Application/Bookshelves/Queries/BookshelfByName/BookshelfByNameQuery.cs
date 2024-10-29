using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.BookshelfByRefName;

public class BookshelfByNameQuery : IRequest<BookshelfResult>
{
    public required string Name { get; init; }
    public required Guid UserId { get; init; }
}