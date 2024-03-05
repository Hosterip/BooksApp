using MediatR;
using PostsApp.Application.Common.Interfaces;

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
        
        return new AuthResult{Id = user!.Id, Username = user.Username, Role = user.Role.Name};
    }
}