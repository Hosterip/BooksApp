using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.BookshelfById;

public class BookshelfByIdQuery : IRequest<BookshelfResult>
{
    public required Guid BookshelfId { get; init; }
}