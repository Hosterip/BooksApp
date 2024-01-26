using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Exceptions;

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
            .Post.GetSingleWhereAsync(post => post.Id == request.Id && post.User.Id == request.Id);
        await _unitOfWork.Post.RemoveAsync(post!);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}