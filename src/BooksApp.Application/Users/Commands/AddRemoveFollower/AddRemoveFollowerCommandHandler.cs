using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Users.Commands.AddRemoveFollower;

internal sealed class AddRemoveFollowerCommandHandler : IRequestHandler<AddRemoveFollowerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddRemoveFollowerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddRemoveFollowerCommand request, CancellationToken cancellationToken)
    {
        if(await _unitOfWork.Users.AnyFollower(request.UserId, request.FollowerId, cancellationToken))
            await _unitOfWork.Users.RemoveFollower(request.UserId, request.FollowerId, cancellationToken);
        else
            await _unitOfWork.Users.AddFollower(request.UserId, request.FollowerId, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}