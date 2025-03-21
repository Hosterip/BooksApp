using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.CreateBookshelf;

public sealed class CreateBookshelfCommand : IRequest<BookshelfResult>
{
    public required string Name { get; init; }
}