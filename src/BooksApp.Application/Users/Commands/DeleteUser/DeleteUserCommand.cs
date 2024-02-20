using MediatR;

namespace PostsApp.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public required int Id { get; init; }
}