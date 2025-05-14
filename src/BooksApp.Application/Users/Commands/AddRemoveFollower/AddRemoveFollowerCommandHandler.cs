using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Users.Commands.AddRemoveFollower;

internal sealed class AddRemoveFollowerCommandHandler(
    IUnitOfWork unitOfWork,
    IUserService userService)
    : IRequestHandler<AddRemoveFollowerCommand>
{
    public async Task Handle(AddRemoveFollowerCommand request, CancellationToken cancellationToken)
    {
        var followerId = userService.GetId()!.Value;

        var user = await unitOfWork.Users.GetSingleById(
            request.UserId,
            cancellationToken,
            true,
            true);

        var follower = await unitOfWork.Users.GetSingleById(followerId, cancellationToken);
        if (user!.HasFollower(follower!.Id))
            user.RemoveFollower(follower);
        else
            user.AddFollower(follower);

        await unitOfWork.SaveAsync(cancellationToken);
    }
}