using MediatR;

namespace PostsApp.Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public required int Id { get; init; }
    public required string NewUsername { get; init; }
}