using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Users.Commands.UpdateEmail;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Application.Users.Commands.UpdateUsername;

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
            await _unitOfWork.Users.GetSingleById(request.Id);
        user!.Email = request.Email;
        
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}