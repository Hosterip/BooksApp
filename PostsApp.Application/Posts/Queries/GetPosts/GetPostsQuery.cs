using MediatR;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;

namespace PostsApp.Application.Posts.Queries.GetPosts;

public class GetPostsQuery : IRequest<PaginatedArray<PostResult>>
{
    public string? Query { get; set; }
    public int Page { get; set; }
    public int? Limit { get; set; }
}