using MediatR;
using PostsApp.Application.Common.Extensions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;

namespace PostsApp.Application.Posts.Queries.GetPosts;

internal sealed class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, PaginatedArray<PostResult>>
{
    private readonly IAppDbContext _dbContext;

    public GetPostsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PaginatedArray<PostResult>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        string? query = request.Query;
        int limit = request.Limit ?? 10;
        int page = request.Page;
        
        var result = await 
            (
                from post in _dbContext.Posts
                where query == null || post.Title.Contains(request.Query)
                let user = new UserResult { Id = post.User.Id, Username = post.User.Username }
                select new PostResult { Id = post.Id, Title = post.Title, Body = post.Body, User = user })
            .PaginationAsync(page, limit);
        
        return result;
    }
}