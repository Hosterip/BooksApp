using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.RemoveBookByName;

public sealed class RemoveBookByNameCommand : IRequest
{
    public required Guid BookId { get; set; }
    public required Guid UserId { get; set; }
    public required string BookshelfName { get; set; }
}