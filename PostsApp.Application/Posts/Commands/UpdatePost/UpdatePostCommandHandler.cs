using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;
using PostsApp.Domain.Exceptions;

namespace PostsApp.Application.Posts.Commands.UpdatePost;

internal sealed class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostResult>
{
    private readonly IAppDbContext _dbContext;

    public UpdatePostCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PostResult> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _dbContext.Posts
            .Include(post => post.User)
            .SingleOrDefaultAsync(
                post => post.Id == request.Id,
                cancellationToken);

        if (post is null)
            throw new PostException("Post not found");

        post.Title = request.Title;
        post.Body = request.Body;

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return new PostResult
        {
            Id = post.Id, Title = post.Title, Body = post.Body, User = new UserResult{Username = post.User.Username, Id = post.User.Id}
        };
    }
}