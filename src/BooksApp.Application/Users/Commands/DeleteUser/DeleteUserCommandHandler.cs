using MediatR;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleById(request.Id);
        _unitOfWork.Users.Remove(user!);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}