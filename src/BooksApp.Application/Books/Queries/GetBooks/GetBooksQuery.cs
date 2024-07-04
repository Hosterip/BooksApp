using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Books.Queries.GetBooks;

public class GetBooksQuery : IRequest<PaginatedArray<BookResult>>
{
    public string? Query { get; init; }
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public Guid? UserId { get; init; }
    public Guid? GenreId { get; init; }
}