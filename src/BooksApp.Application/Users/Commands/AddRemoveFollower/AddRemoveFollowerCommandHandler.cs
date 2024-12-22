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
        var user = await _unitOfWork.Users.GetUserWithRelationshipsById(request.UserId, cancellationToken);

        var follower = await _unitOfWork.Users.GetSingleById(request.FollowerId, token: cancellationToken);
        if(user!.HasFollower(follower!.Id))
            user.RemoveFollower(follower);
        else
            user.AddFollower(follower);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}