using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Roles.Queries.GetRoles;

internal sealed class GetRoleQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetRoleQuery, IEnumerable<RoleResult>>
{
    public async Task<IEnumerable<RoleResult>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
        var roles =
        (
            from role in await unitOfWork.Roles.GetAllAsync(cancellationToken)
            select new RoleResult { Id = role.Id.Value.ToString(), Name = role.Name }
        ).ToArray();
        return roles;
    }
}