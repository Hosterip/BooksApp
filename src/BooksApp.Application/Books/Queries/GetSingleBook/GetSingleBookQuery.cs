using BooksApp.Application.Books.Results;
using MediatR;

namespace BooksApp.Application.Books.Queries.GetSingleBook;

public class GetSingleBookQuery : IRequest<BookResult>
{
    public required Guid Id { get; init; }
}