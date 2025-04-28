using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.AddBookByName;

[Authorize]
public sealed class AddBookByNameCommand : IRequest
{
    public required Guid BookId { get; init; }
    public required string BookshelfName { get; init; }
}