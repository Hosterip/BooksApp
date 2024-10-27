using MediatR;

namespace PostsApp.Application.Roles.Queries.GetRoles;

public sealed class GetRoleQuery : IRequest<RoleResult[]>
{
}