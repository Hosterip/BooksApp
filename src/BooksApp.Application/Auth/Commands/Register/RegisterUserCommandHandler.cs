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
        var memberRole = await unitOfWork.Roles.GetSingleWhereAsync(
            role => role.Name == RoleNames.Member,
            cancellationToken);

        var user = User.Create(
            passwordHasher,
            request.Email,
            memberRole!,
            request.Password,
            null,
            request.FirstName,
            request.MiddleName,
            request.LastName
        );

        await unitOfWork.Roles.Attach(memberRole!);
        await unitOfWork.Users.AddAsync(user, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);
        return mapper.Map<AuthResult>(user!);
    }
}