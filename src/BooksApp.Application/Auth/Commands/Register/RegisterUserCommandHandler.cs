using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Models;
using PostsApp.Domain.Security;

namespace PostsApp.Application.Auth.Commands.Register;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<AuthResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var hashSalt = Hashing.GenerateHashSalt(request.Password);
        var memberRole = await _unitOfWork.Roles.GetSingleWhereAsync(role => role.Name == RoleNames.Member);
        User user = new User
        {
            Username = request.Username, 
            Hash = hashSalt.Hash, 
            Salt = hashSalt.Salt, 
            Role = memberRole!
        };
        
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync(cancellationToken);
        return new AuthResult
        {
            Id = user.Id,
            Role = user.Role.Name,
            SecurityStamp = user.SecurityStamp,
            Username = user.Username,
            AvatarName = null
        };
    }
}