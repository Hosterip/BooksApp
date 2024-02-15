using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Books.Queries.GetBooks;

public class GetBooksQuery : IRequest<PaginatedArray<BookResult>>
{
    public string? Query { get; set; }
    public int? Page { get; set; }
    public int? Limit { get; set; }
}