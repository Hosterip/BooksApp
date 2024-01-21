using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Users.Commands.UpdateUsername;

internal sealed class UpdateUsernameCommandHandler : IRequestHandler<UpdateUsernameCommand>
{
    private readonly IAppDbContext _dbContext;

    public UpdateUsernameCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Handle(UpdateUsernameCommand request, CancellationToken cancellationToken)
    {
        var user = 
            await _dbContext.Users.SingleAsync(user => user.Id == request.Id, cancellationToken);

        user.Username = request.NewUsername;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}