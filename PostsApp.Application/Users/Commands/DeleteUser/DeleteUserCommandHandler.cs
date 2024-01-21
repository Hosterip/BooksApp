using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Posts.Commands.DeletePost;
using PostsApp.Domain.Exceptions;

namespace PostsApp.Application.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler : IRequestHandler<DeletePostCommand>
{
    private readonly IAppDbContext _dbContext;

    public DeleteUserCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users.SingleOrDefault(user => user.Id == request.Id);
        if (user is null)
            throw new UserException("User not found");
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}