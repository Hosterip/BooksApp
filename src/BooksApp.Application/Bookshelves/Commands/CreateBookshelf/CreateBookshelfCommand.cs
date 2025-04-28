using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.CreateBookshelf;

[Authorize]
public sealed class CreateBookshelfCommand : IRequest<BookshelfResult>
{
    public required string Name { get; init; }
}