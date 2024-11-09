using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.AddBookByName;

public sealed class AddBookByNameCommand : IRequest
{
    public required Guid BookId { get; init; }
    public required Guid UserId { get; init; }
    public required string BookshelfName { get; init; }
}