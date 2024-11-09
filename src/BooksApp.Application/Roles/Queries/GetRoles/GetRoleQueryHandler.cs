using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Roles.Queries.GetRoles;

internal sealed class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, IEnumerable<RoleResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRoleQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<RoleResult>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
        var roles =
        (
            from role in await _unitOfWork.Roles.GetAllAsync(cancellationToken)
            select new RoleResult { Id = role.Id.Value.ToString(), Name = role.Name }
        ).ToArray();
        return roles;
    }
}