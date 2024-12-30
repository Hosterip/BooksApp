using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using BooksApp.Domain.Common.Interfaces;
using BooksApp.Domain.Role;
using BooksApp.Domain.User;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Auth.Commands.Register;

internal sealed class RegisterUserCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterUserCommand, AuthResult>
{
    public async Task<AuthResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var hashSalt = passwordHasher.GenerateHashSalt(request.Password);
        var memberRole = await unitOfWork.Roles.GetSingleWhereAsync(
            role => role.Name == RoleNames.Member,
            cancellationToken);

        var user = User.Create(
            request.Email,
            request.FirstName,
            request.MiddleName,
            request.LastName,
            memberRole!,
            hashSalt.Hash,
            hashSalt.Salt,
            null
        );

        await unitOfWork.Roles.Update(memberRole!);
        await unitOfWork.Users.AddAsync(user, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);
        return mapper.Map<AuthResult>(user!);
    }
}