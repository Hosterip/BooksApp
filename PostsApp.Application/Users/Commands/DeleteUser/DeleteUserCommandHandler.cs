using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Posts.Commands.DeletePost;
using PostsApp.Domain.Exceptions;

namespace PostsApp.Application.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler : IRequestHandler<DeletePostCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.User.GetSingleWhereAsync(user => user.Id == request.Id);
        if (user is null)
            throw new UserException("User not found");
        await _unitOfWork.User.RemoveAsync(user);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}