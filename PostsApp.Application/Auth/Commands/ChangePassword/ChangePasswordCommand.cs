using MediatR;

namespace PostsApp.Application.Auth.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest
{
    public int Id { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}