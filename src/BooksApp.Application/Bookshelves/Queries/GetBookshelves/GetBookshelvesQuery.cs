using MediatR;
using PostsApp.Domain.Bookshelf;

namespace PostsApp.Application.Bookshelves.Queries.GetBookshelves;

public sealed class GetBookshelvesQuery : IRequest<List<BookshelfResult>>
{
    public required Guid UserId { get; set; }
}