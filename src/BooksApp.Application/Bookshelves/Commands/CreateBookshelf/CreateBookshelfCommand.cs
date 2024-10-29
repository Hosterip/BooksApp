using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.CreateBookshelf;

public sealed class CreateBookshelfCommand : IRequest<BookshelfResult>
{
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
}