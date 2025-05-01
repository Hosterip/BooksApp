using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.RemoveBook;

[Authorize]
public sealed class RemoveBookCommand : IRequest
{
    public required Guid BookId { get; init; }
    public required Guid BookshelfId { get; init; }
}