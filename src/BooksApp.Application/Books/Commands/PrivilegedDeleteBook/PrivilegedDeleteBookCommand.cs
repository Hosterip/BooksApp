using MediatR;

namespace BooksApp.Application.Books.Commands.PrivilegedDeleteBook;

public sealed class PrivilegedDeleteBookCommand : IRequest
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
}