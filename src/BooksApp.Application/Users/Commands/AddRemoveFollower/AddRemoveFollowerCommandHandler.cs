using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Users.Commands.AddRemoveFollower;

internal sealed class AddRemoveFollowerCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AddRemoveFollowerCommand>
{
    public async Task Handle(AddRemoveFollowerCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetSingleById(request.UserId, cancellationToken, includeRelationships: true, asTracking: true);

        var follower = await unitOfWork.Users.GetSingleById(request.FollowerId, token: cancellationToken);
        if(user!.HasFollower(follower!.Id))
            user.RemoveFollower(follower);
        else
            user.AddFollower(follower);

        await unitOfWork.SaveAsync(cancellationToken);
    }
}