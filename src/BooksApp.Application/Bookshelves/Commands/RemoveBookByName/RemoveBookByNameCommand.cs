using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.RemoveBookByName;

[Authorize]
public sealed class RemoveBookByNameCommand : IRequest
{
    public required Guid BookId { get; init; }
    public required string BookshelfName { get; init; }
}