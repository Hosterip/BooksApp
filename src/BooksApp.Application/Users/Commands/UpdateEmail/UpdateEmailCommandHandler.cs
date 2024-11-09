using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Users.Commands.UpdateEmail;

internal sealed class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmailCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
    {
        var user =
            await _unitOfWork.Users.GetSingleById(request.Id, cancellationToken);
        user!.Email = request.Email;

        await _unitOfWork.SaveAsync(cancellationToken);
    }
}