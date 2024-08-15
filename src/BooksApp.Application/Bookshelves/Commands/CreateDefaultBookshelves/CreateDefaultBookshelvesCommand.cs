using MediatR;

namespace PostsApp.Application.Bookshelves.Commands.CreateDefaultBookshelves;

public sealed class CreateDefaultBookshelvesCommand : IRequest
{
    public required Guid UserId { get; set; }
}