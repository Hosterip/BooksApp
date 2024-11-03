using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.GetBookshelves;

public sealed class GetBookshelvesQuery : IRequest<IEnumerable<BookshelfResult>>
{
    public required Guid UserId { get; set; }
}