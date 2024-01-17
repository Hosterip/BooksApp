using MediatR;
using PostsApp.Application.Posts.Results;

namespace PostsApp.Application.Posts.Queries.GetSinglePost;

public class GetSinglePostQuery : IRequest<PostResult>
{
    public int Id { get; set; } 
}