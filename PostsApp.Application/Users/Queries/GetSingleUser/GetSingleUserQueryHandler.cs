using MediatR;
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
        var user = _dbContext.Users.SingleOrDefault(user => user.Username == request.Username);
        if (user is null)
            throw new UserException("User not found");

        var posts =
            (
                from post in _dbContext.Posts
                where post.User == user
                select new PostResult
                    { id = post.Id, title = post.Title, body = post.Body })
            .ToArray();
        return new SingleUserResult { username = user.Username, posts = posts };
    }
}