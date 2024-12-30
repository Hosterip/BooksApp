using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Users.Commands.UpdateName;

internal sealed class UpdateNameCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateNameCommand>
{
    public async Task Handle(UpdateNameCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetSingleById(request.UserId, cancellationToken);
        user!.ChangeName(
            request.FirstName,
            request.MiddleName,
            request.LastName);

        await unitOfWork.Users.Update(user);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}