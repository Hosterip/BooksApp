using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Users.Commands.UpdateEmail;

internal sealed class UpdateEmailCommandHandler(
    IUnitOfWork unitOfWork,
    IUserService userService)
    : IRequestHandler<UpdateEmailCommand>
{
    public async Task Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
    {
        var userId = userService.GetId()!.Value;
        var user = await unitOfWork.Users.GetSingleById(userId, cancellationToken);
        user!.ChangeEmail(request.Email);

        await unitOfWork.Users.Update(user);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}