using MediatR;

namespace PostsApp.Application.Books.Commands.AddRemoveLike;

public class AddRemoveLikeCommand : IRequest
{
    public required int UserId { get; init; }
    public required int PostId { get; init; }
}