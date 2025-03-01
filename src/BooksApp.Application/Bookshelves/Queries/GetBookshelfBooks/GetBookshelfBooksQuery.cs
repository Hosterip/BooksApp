using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Results;
using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.GetBookshelfBooks;

public sealed class GetBookshelfBooksQuery : IRequest<PaginatedArray<BookResult>>
{
    public required Guid BookshelfId { get; init; }
    public int? Limit { get; init; }
    public int? Page { get; init; }
}