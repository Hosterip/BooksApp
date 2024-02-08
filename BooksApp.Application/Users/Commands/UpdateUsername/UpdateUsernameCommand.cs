using MediatR;

namespace PostsApp.Application.Users.Commands.UpdateUsername;

public class UpdateUsernameCommand : IRequest
{
    public int Id { get; set; }
    public string NewUsername { get; set; }
}