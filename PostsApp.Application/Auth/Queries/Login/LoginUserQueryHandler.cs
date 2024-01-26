using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Auth;

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
            _unitOfWork.User.GetSingleWhereAsync(user => user.Username == request.Username);
        if (!AuthUtils.IsPasswordValid(user!.Hash, user.Salt, request.Password))
            throw new AuthException("Password is incorrect");
        return new AuthResult{Id = user.Id, username = user.Username};
    }
}