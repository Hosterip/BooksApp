using MediatR;
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
        var user = _dbContext.Users.SingleOrDefault(user => user.Username == request.Username);
        if (user is null)
            ThrowAuthException();
        string hash = AuthUtils.CreateHash(request.Password, AuthUtils.StringToByteArray(user.Salt));
        if (hash != user.Hash)
            ThrowAuthException();
        return new AuthResult{username = user.Username};
    }

    private void ThrowAuthException()
    {
        throw new AuthException("Username or password is incorrect");
    }
}