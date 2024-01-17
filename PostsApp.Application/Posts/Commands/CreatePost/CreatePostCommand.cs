using MediatR;
using PostsApp.Application.Posts.Results;

namespace PostsApp.Application.Posts.Commands.CreatePost;

public sealed class CreatePostCommand : IRequest<PostResult>
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string Username { get; set; }
}