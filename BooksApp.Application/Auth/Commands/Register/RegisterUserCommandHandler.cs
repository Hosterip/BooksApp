using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Auth;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Constants.Exceptions;
using PostsApp.Domain.Exceptions;
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
        if (await _unitOfWork.Users.AnyAsync(user => user.Username == request.Username))
            throw new AuthException(AuthExceptionConstants.Occupied);
        var hashSalt = AuthUtils.CreateHashSalt(request.Password);
        var memberRole = await _unitOfWork.Roles.GetSingleWhereAsync(role => role.Name == RoleConstants.Member);
        User user = new User { Username = request.Username, Hash = hashSalt.hash, Salt = hashSalt.salt, Role = memberRole!};
        
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync(cancellationToken);
        return new AuthResult {Id = user.Id, Username = request.Username, Role = RoleConstants.Member };
    }
}