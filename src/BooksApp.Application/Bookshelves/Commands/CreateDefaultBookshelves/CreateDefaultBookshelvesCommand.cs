using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.CreateDefaultBookshelves;

public sealed class CreateDefaultBookshelvesCommand : IRequest
{
    public required Guid UserId { get; init; }
}