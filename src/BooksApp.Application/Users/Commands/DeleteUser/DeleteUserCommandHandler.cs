using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetSingleById(request.Id, cancellationToken);
        await unitOfWork.Users.Remove(user!);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}