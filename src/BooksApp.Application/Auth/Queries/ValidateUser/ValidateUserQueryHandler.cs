using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Roles;
using MediatR;

namespace BooksApp.Application.Auth.Queries.ValidateUser;

internal sealed class ValidateUserQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ValidateUserQuery, RoleResult?>
{
    public async Task<RoleResult?> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetSingleById(
            request.UserId,
            cancellationToken);
        if (user != null && user.SecurityStamp == request.SecurityStamp)
            return new RoleResult {Id = user.Role.Id.ToString()!, Name = user.Role.Name};
        return null;
    }
}