using MediatR;

namespace PostsApp.Application.Posts.Commands.AddRemoveLike;

public class AddRemoveLikeCommand : IRequest
{
    public int UserId { get; set; }
    public int PostId { get; set; }
}