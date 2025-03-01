using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Results;
using MediatR;

namespace BooksApp.Application.Books.Queries.GetBooks;

public class GetBooksQuery : IRequest<PaginatedArray<BookResult>>
{
    public string? Title { get; init; }
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public Guid? UserId { get; init; }
    public Guid? GenreId { get; init; }
}