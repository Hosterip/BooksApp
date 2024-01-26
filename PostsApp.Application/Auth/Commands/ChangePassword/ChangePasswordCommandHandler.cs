using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Auth;

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
        var user = await _unitOfWork.User
            .GetSingleWhereAsync(user => user.Id == request.Id);
        
        if (!AuthUtils.IsPasswordValid(user!.Hash, user.Salt, request.OldPassword)) 
            throw new AuthException("Old password is wrong");

        var hashSalt = AuthUtils.CreateHashSalt(request.NewPassword);
        user.Hash = hashSalt.hash;
        user.Salt = hashSalt.salt;

        await _unitOfWork.SaveAsync(cancellationToken);
    }
}