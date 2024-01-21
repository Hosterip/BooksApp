using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Auth;

namespace PostsApp.Application.Auth.Queries.Login;

internal sealed class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, AuthResult>
{
    private readonly IAppDbContext _dbContext;

    public LoginUserQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<AuthResult> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await 
            _dbContext.Users.SingleAsync(user => user.Username == request.Username, cancellationToken);
        if (!AuthUtils.IsPasswordValid(user.Hash, user.Salt, request.Password))
            throw new AuthException("Password is incorrect");
        return new AuthResult{Id = user.Id, username = user.Username};
    }
}