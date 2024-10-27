using MediatR;

namespace PostsApp.Application.Bookshelves.Commands.AddBookToDefaultBookshelf;

public sealed class AddBookByNameCommand : IRequest
{
    public required Guid BookId { get; set; }
    public required Guid UserId { get; set; }
    public required string BookshelfRefName { get; set; }
}