using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Results;
using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.GetBookshelfBooks;

public sealed class GetBookshelfBooksQuery : IRequest<PaginatedArray<BookResult>>
{
    public required Guid BookshelfId { get; set; }
    public int? Limit { get; set; }
    public int? Page { get; set; }
}