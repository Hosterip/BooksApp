using MediatR;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Users.Commands.UpdateName;

public class UpdateNameCommandHandler : IRequestHandler<UpdateNameCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateNameCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateNameCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleById(request.UserId);
        user!.FirstName = request.FirstName;
        user.MiddleName = request.MiddleName;
        user.LastName = request.LastName;
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}