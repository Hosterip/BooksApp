using MediatR;
using PostsApp.Application.Books.Results;

namespace PostsApp.Application.Books.Queries.GetSingleBook;

public class GetSingleBookQuery : IRequest<BookResult>
{
    public required Guid Id { get; init; } 
}