using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.RemoveBookByName;

public sealed class RemoveBookByNameCommand : IRequest
{
    public required Guid BookId { get; init; }
    public required Guid UserId { get; init; }
    public required string BookshelfName { get; init; }
}