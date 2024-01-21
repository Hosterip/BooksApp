using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Auth;

namespace PostsApp.Application.Auth.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandle : IRequestHandler<ChangePasswordCommand>
{
    private readonly IAppDbContext _dbContext;

    public ChangePasswordCommandHandle(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .SingleAsync(user => user.Id == request.Id, cancellationToken);
        
        if (!AuthUtils.IsPasswordValid(user.Hash, user.Salt, request.OldPassword)) 
            throw new AuthException("Old password is wrong");

        var hashSalt = AuthUtils.CreateHashSalt(request.NewPassword);
        user.Hash = hashSalt.hash;
        user.Salt = hashSalt.salt;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}