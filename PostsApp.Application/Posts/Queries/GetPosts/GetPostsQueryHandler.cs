using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Posts.Results;
using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Posts.Queries.GetPosts;

internal class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, PostsResult>
{
    private readonly IAppDbContext _dbContext;

    public GetPostsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PostsResult> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        string? query = request.Query;
        int limit = request.Limit ?? 10;
        int page = request.Page;
        
        var rawPosts =
            from post in _dbContext.Posts
            where query == null || post.Title.Contains(request.Query)
            let user = new DefaultUserResult { username = post.User.Username }
            select new PostResult { id = post.Id, title = post.Title, body = post.Body, user = user };
        
        var posts = rawPosts
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToArray();

        int totalCount = rawPosts.Count();
        int totalPages = (int)Math.Ceiling((decimal)totalCount / limit);

        return new PostsResult { totalCount = totalCount, totalPages = totalPages, posts = posts };
    }
}