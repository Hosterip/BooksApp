using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Auth;
using PostsApp.Domain.Models;

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
        if (await _dbContext.Users.AnyAsync(user => user.Username == request.Username, cancellationToken))
            throw new AuthException("Username is occupied");
        var hashSalt = AuthUtils.CreateHashSalt(request.Password);
        User user = new User { Username = request.Username, Hash = hashSalt.hash, Salt = hashSalt.salt };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new AuthResult {Id = user.Id, username = request.Username };
    }
}