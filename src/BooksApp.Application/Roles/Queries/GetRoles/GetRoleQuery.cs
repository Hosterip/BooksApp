using MediatR;

namespace BooksApp.Application.Roles.Queries.GetRoles;

public sealed class GetRoleQuery : IRequest<RoleResult[]>
{
}