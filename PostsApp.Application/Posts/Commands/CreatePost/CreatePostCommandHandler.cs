using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Posts.Results;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.Exceptions;
using PostsApp.Models;

namespace PostsApp.Application.Posts.Commands.CreatePost;

internal class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostResult>
{
    private readonly IAppDbContext _dbContext;

    public CreatePostCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PostResult> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users.SingleOrDefault(user => user.Username == request.Username);
        if (user == null) 
            throw new PostException("User not found");
        var post = new Post { User = user, Title = request.Title, Body = request.Body };
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync(cancellationToken);
        var result = new PostResult
        {
            user = new DefaultUserResult { username = user.Username }, title = post.Title, body = post.Body,
            id = post.Id
        };
        return result;
    }
}