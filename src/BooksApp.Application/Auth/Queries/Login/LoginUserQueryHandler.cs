using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Exceptions;

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
        
        if (user is null || !HashSaltGen.IsPasswordValid(user.Hash, user.Salt, request.Password))
            throw new AuthException(ConstantsAuthException.UsernameOrPassword);
        
        return new AuthResult{Id = user.Id, Username = user.Username, Role = user.Role.Name};
    }
}