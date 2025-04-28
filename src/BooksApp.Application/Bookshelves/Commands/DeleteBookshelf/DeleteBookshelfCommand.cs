using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.DeleteBookshelf;

[Authorize]
public class DeleteBookshelfCommand : IRequest
{
    public required Guid BookshelfId { get; init; }
}