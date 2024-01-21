using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.Exceptions;

namespace PostsApp.Application.Users.Queries.GetSingleUser;

internal sealed class GetSingleUserQueryHandler : IRequestHandler<GetSingleUserQuery, SingleUserResult>
{
    private readonly IAppDbContext _dbContext;

    public GetSingleUserQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SingleUserResult> Handle(GetSingleUserQuery request, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users.SingleOrDefault(user => user.Id == request.Id);
        if (user is null)
            throw new UserException("User not found");

        var posts = await 
            (
                from post in _dbContext.Posts
                where post.User == user
                select new PostResult
                    { Id = post.Id, Title = post.Title, Body = post.Body })
            .ToArrayAsync(cancellationToken);
        return new SingleUserResult { Id = user.Id, Username = user.Username, Posts = posts };
    }
}