using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Auth.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IPasswordHasher passwordHasher)
    : IRequestHandler<ChangePasswordCommand, AuthResult>
{
    public async Task<AuthResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users
            .GetSingleById(request.Id, cancellationToken);

        if (!user!.IsPasswordValid(passwordHasher, request.OldPassword))
            throw new ValidationException([
                new ValidationFailure(
                    nameof(ChangePasswordCommand.OldPassword),
                    AuthValidationMessages.Password)
            ]);

        user.ChangePassword(passwordHasher, request.NewPassword);
        
        user.ChangeSecurityStamp();

        await unitOfWork.Users.Update(user);
        await unitOfWork.SaveAsync(cancellationToken);
        return mapper.Map<AuthResult>(user);
    }
}