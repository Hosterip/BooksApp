using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.UpdateBookshelfName;

public sealed class UpdateBookshelfNameCommand : IRequest
{
    public required Guid BookshelfId { get; init; }
    public required string NewName { get; init; }
}