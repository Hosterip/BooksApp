using MediatR;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Auth.Queries.ValidateUser;

internal sealed class ValidateUserQueryHandler : IRequestHandler<ValidateUserQuery, string?>
{
    private readonly IUnitOfWork _unitOfWork;

    public ValidateUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string?> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleById(request.UserId);
        if (user != null && user.SecurityStamp == request.SecurityStamp)
            return user.Role.Name;
        return null;
    }
}