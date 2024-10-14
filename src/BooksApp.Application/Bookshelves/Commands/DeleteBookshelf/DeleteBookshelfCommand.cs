using MediatR;

namespace PostsApp.Application.Bookshelves.Commands.DeleteBookshelf;

public class DeleteBookshelfCommand : IRequest
{
    public required Guid BookshelfId { get; set; }
}