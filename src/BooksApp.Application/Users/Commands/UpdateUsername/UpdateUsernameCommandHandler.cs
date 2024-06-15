using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Users.Commands.UpdateUsername;

internal sealed class UpdateUsernameCommandHandler : IRequestHandler<UpdateUsernameCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUsernameCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(UpdateUsernameCommand request, CancellationToken cancellationToken)
    {
        var user = 
            await _unitOfWork.Users.GetSingleWhereAsync(user => user.Id.Value == request.Id);
        user!.Username = request.NewUsername;
        
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}