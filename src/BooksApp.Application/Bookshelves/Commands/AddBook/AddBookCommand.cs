using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.AddBook;

[Authorize]
public sealed class AddBookCommand : IRequest
{
    public required Guid BookId { get; init; }
    public required Guid BookshelfId { get; init; }
}