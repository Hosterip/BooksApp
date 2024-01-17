using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Auth;
using PostsApp.Models;

namespace PostsApp.Application.Auth.Commands.Register;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResult>
{
    private readonly IAppDbContext _dbContext;

    public RegisterUserCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<AuthResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (_dbContext.Users.SingleOrDefault(user => user.Username == request.Username) != null)
            throw new AuthException("Username is occupied");
        byte[] salt = AuthUtils.GenerateSalt();
        string hash = AuthUtils.CreateHash(request.Password, salt);
        User user = new User { Username = request.Username, Hash = hash, Salt = Convert.ToHexString(salt) };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new AuthResult { username = request.Username };
    }
}