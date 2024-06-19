using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Auth.Queries.Login;

internal sealed class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, AuthResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public LoginUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<AuthResult> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await 
            _unitOfWork.Users.GetSingleWhereAsync(user => user.Username == request.Username);
        
        return new AuthResult
        {
            Id = user!.Id.Value.ToString(),
            Role = user.Role.Name,
            SecurityStamp = user.SecurityStamp,
            Username = user.Username,
            AvatarName = user.Avatar?.ImageName 
        };
    }
}