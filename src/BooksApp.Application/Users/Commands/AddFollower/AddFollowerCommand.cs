using MediatR;

namespace BooksApp.Application.Users.Commands.AddFollower;

public sealed class AddFollowerCommand : IRequest
{
    public required Guid UserId { get; init; }
    public required Guid FollowerId { get; init; }
}