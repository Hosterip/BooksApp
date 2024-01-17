using MediatR;

namespace PostsApp.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public string username { get; set; }
}