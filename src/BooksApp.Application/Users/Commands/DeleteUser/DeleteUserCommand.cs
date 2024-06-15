using MediatR;

namespace PostsApp.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public required Guid Id { get; init; }
}