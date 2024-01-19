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
        var post = _dbContext.Posts.Include(post => post.User).SingleOrDefault(post => post.Id == request.Id);
        if (post == null) 
            throw new PostException("Post not found");

        var user = new UserResult { username = post.User.Username };

        return new PostResult { id = post.Id, title = post.Title, body = post.Body, user = user };
    }
}