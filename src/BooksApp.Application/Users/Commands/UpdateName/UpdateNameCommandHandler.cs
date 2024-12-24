using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Users.Commands.UpdateName;

internal sealed class UpdateNameCommandHandler : IRequestHandler<UpdateNameCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateNameCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateNameCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleById(request.UserId, cancellationToken);
        user!.ChangeName(
            request.FirstName,
            request.MiddleName,
            request.LastName);

        await _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}