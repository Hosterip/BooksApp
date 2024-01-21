using MediatR;

namespace PostsApp.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public int Id { get; set; }
}