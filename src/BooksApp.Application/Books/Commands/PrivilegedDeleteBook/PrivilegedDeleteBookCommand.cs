using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Books.Commands.PrivilegedDeleteBook;

[Authorize]
public sealed class PrivilegedDeleteBookCommand : IRequest
{
    public required Guid Id { get; init; }
}