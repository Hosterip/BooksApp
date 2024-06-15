using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Security;

namespace PostsApp.Application.Auth.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, AuthResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<AuthResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users
            .GetSingleWhereAsync(user => user.Id.Value == request.Id);

        var hashSalt = Hashing.GenerateHashSalt(request.NewPassword);
        user!.Hash = hashSalt.Hash;
        user.Salt = hashSalt.Salt;
        user.SecurityStamp = Guid.NewGuid().ToString();

        await _unitOfWork.SaveAsync(cancellationToken);
        return new AuthResult
        {
            Id = user.Id.Value.ToString(),
            Role = user.Role.Name,
            SecurityStamp = user.SecurityStamp,
            Username = user.Username,
            AvatarName = user.Avatar?.ImageName
        };
    }
}