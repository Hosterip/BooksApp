using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.CreateDefaultBookshelves;

[Authorize]
public sealed class CreateDefaultBookshelvesCommand : IRequest
{
    public required Guid UserId { get; init; }
}