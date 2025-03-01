using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.AddBook;

public sealed class AddBookCommand : IRequest
{
    public required Guid BookId { get; init; }
    public required Guid BookshelfId { get; init; }
}