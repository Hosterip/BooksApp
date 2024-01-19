using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;
using PostsApp.Domain.Exceptions;

namespace PostsApp.Application.Posts.Queries.GetSinglePost;

internal sealed class GetSinglePostQueryHandler : IRequestHandler<GetSinglePostQuery, PostResult>
{
    private readonly IAppDbContext _dbContext;

    public GetSinglePostQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PostResult> Handle(GetSinglePostQuery request, CancellationToken cancellationToken)
    {
        var post = await _dbContext.Posts
            .Include(post => post.User)
            .SingleOrDefaultAsync(post => post.Id == request.Id, cancellationToken);
        if (post == null) 
            throw new PostException("Post not found");

        var user = new UserResult { Username = post.User.Username };

        return new PostResult { Id = post.Id, Title = post.Title, Body = post.Body, User = user };
    }
}