using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.RemoveBook;

public sealed class RemoveBookCommand : IRequest
{
    public required Guid BookId { get; init; }
    public required Guid BookshelfId { get; init; }
}