using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Exceptions;

namespace PostsApp.Application.Posts.Commands.DeletePost;

internal sealed class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
{
    private readonly IAppDbContext _dbContext;

    public DeletePostCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = _dbContext.Posts.SingleOrDefault(post => post.Id == request.Id && post.User.Id == request.Id);
        if (post == null)
            throw new PostException("Post not found or post not yours");
        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}