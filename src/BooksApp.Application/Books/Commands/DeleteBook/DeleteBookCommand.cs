using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Books.Commands.DeleteBook;

[Authorize]
public class DeleteBookCommand : IRequest
{
    public required Guid Id { get; init; }
}