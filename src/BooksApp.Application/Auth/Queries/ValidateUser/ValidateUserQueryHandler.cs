using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Roles;
using MediatR;

namespace BooksApp.Application.Auth.Queries.ValidateUser;

internal sealed class ValidateUserQueryHandler : IRequestHandler<ValidateUserQuery, RoleResult?>
{
    private readonly IUnitOfWork _unitOfWork;

    public ValidateUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RoleResult?> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleById(
            request.UserId,
            cancellationToken);
        if (user != null && user.SecurityStamp == request.SecurityStamp)
            return new RoleResult {Id = user.Role.Id.ToString()!, Name = user.Role.Name};
        return null;
    }
}