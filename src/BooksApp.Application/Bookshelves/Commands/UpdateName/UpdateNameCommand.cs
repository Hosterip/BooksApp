using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.UpdateName;

public sealed class UpdateNameCommand : IRequest
{
    public required Guid BookshelfId { get; init; }
    public required string NewName { get; init; }
}