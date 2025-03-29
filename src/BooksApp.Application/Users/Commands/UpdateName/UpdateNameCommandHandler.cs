using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Users.Commands.UpdateName;

internal sealed class UpdateNameCommandHandler(
    IUnitOfWork unitOfWork,
    IUserService userService) 
    : IRequestHandler<UpdateNameCommand>
{
    public async Task Handle(UpdateNameCommand request, CancellationToken cancellationToken)
    {
        var userId = userService.GetId()!.Value;
        var user = await unitOfWork.Users.GetSingleById(userId, cancellationToken);
        user!.ChangeName(
            request.FirstName,
            request.MiddleName,
            request.LastName);

        await unitOfWork.Users.Update(user);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}