using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Users.Commands.UpdateUser;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Users.Commands.UpdateUsername;

internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = 
            await _unitOfWork.Users.GetSingleWhereAsync(user => user.Id == request.Id);
        user!.Username = request.NewUsername;
        
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}