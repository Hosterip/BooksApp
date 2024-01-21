using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;
using PostsApp.Domain.Exceptions;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Posts.Commands.CreatePost;

internal sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostResult>
{
    private readonly IAppDbContext _dbContext;

    public CreatePostCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PostResult> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .SingleAsync(user => user.Id == request.Id, cancellationToken);
        var post = new Post { User = user, Title = request.Title, Body = request.Body };
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync(cancellationToken);
        var result = new PostResult
        {
            User = new UserResult { Id = user.Id, Username = user.Username }, Title = post.Title, Body = post.Body,
            Id = post.Id
        };
        return result;
    }
}