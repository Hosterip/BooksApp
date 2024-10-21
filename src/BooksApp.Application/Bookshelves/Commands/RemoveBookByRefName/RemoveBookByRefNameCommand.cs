using MediatR;

namespace PostsApp.Application.Bookshelves.Commands.RemoveBookFromDefaultBookshelf;

public sealed class RemoveBookByRefNameCommand : IRequest
{
    public required Guid BookId { get; set; }
    public required Guid UserId { get; set; }
    public required string BookshelfRefName { get; set; }
}