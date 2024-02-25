using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Constants.Exceptions;
using PostsApp.Domain.Exceptions;

namespace PostsApp.Application.Auth.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users
            .GetSingleWhereAsync(user => user.Id == request.Id);
        
        if (!HashSaltGen.IsPasswordValid(user!.Hash, user.Salt, request.OldPassword)) 
            throw new AuthException(AuthExceptionConstants.Password);

        var hashSalt = HashSaltGen.GenerateHashSalt(request.NewPassword);
        user.Hash = hashSalt.Hash;
        user.Salt = hashSalt.Salt;

        await _unitOfWork.SaveAsync(cancellationToken);
    }
}