using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.DeleteBookshelf;

public class DeleteBookshelfCommand : IRequest
{
    public required Guid BookshelfId { get; init; }
}