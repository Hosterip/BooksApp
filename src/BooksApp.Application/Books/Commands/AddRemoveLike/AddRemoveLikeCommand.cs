using MediatR;

namespace PostsApp.Application.Books.Commands.AddRemoveLike;

public class AddRemoveLikeCommand : IRequest
{
    public int UserId { get; set; }
    public int PostId { get; set; }
}