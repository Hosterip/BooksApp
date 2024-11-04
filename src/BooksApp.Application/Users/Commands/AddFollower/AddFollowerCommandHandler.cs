using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Users.Commands.AddFollower;

internal sealed class AddFollowerCommandHandler : IRequestHandler<AddFollowerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddFollowerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddFollowerCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Users.AddFollower(request.UserId, request.FollowerId);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}