using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.AddBook;

public sealed class AddBookCommand : IRequest
{
    public required Guid BookId { get; set; }
    public required Guid UserId { get; set; }
    public required Guid BookshelfId { get; set; }
}