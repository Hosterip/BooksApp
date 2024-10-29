using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.RemoveBook;

public sealed class RemoveBookCommand : IRequest
{
    public required Guid BookId { get; set; }
    public required Guid BookshelfId { get; set; }
    public required Guid UserId { get; set; }
}