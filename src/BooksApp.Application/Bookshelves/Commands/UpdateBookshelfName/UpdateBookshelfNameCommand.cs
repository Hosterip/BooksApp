using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.UpdateBookshelfName;

[Authorize]
public sealed class UpdateBookshelfNameCommand : IRequest
{
    public required Guid BookshelfId { get; init; }
    public required string NewName { get; init; }
}