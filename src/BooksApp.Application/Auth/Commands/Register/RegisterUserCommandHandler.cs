using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Models;

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
        var hashSalt = HashSaltGen.GenerateHashSalt(request.Password);
        var memberRole = await _unitOfWork.Roles.GetSingleWhereAsync(role => role.Name == RoleConstants.Member);
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
            Username = request.Username,
            Role = user.Role.Name
        };
    }
}