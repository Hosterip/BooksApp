using MediatR;

namespace PostsApp.Application.Users.Commands.UpdateUsername;

public class UpdateUsernameCommand : IRequest
{
    public required Guid? Id { get; init; }
    public required string NewUsername { get; init; }
}