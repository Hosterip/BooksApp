using MediatR;

namespace BooksApp.Application.Users.Commands.AddRemoveFollower;

public sealed class AddRemoveFollowerCommand : IRequest
{
    public required Guid UserId { get; init; }
}