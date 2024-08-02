using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Bookshelves.Queries.GetBookshelfBooks;

public sealed class GetBookshelfBooksQuery : IRequest<PaginatedArray<BookResult>>
{
    public required Guid BookshelfId { get; set; }
    public int? Limit { get; set; }
    public int? Page { get; set; }
}