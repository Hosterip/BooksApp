using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Auth;
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
        if (await _unitOfWork.Users.AnyAsync(user => user.Username == request.Username))
            throw new AuthException(AuthExceptionConstants.Occupied);
        var hashSalt = AuthUtils.CreateHashSalt(request.Password);
        User user = new User { Username = request.Username, Hash = hashSalt.hash, Salt = hashSalt.salt };
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync(cancellationToken);
        return new AuthResult {Id = user.Id, username = request.Username };
    }
}