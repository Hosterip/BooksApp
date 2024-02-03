using MediatR;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Posts.Commands.DeletePost;

internal sealed class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePostCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _unitOfWork
            .Posts.GetSingleWhereAsync(post => post.Id == request.Id && post.User.Id == request.UserId);
        await _unitOfWork.Posts.RemoveAsync(post!);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}