using MediatR;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Roles.Commands.UpdateRole;

public sealed class UpdateRoleCommand : IRequest<UserResult>
{
    public required Guid ChangerId { get; init; }
    public required Guid UserId { get; init; }
    public required string Role { get; init; }
}