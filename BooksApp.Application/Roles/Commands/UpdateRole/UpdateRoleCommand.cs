using MediatR;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Roles.Commands.UpdateRole;

public sealed class UpdateRoleCommand : IRequest<UserResult>
{
    public int ChangerId { get; set; }
    public int UserId { get; set; }
    public string Role { get; set; }
}