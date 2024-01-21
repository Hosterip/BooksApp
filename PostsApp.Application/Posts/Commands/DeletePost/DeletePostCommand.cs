using MediatR;

namespace PostsApp.Application.Posts.Commands.DeletePost;

public class DeletePostCommand : IRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
}