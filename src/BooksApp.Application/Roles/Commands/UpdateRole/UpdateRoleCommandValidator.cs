using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Security;
using FluentValidation;

namespace BooksApp.Application.Roles.Commands.UpdateRole;

internal sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Role)
            .MustAsync(async (roleName, cancellationToken) =>
            {
                return await unitOfWork.Roles.AnyAsync(role => role.Name == roleName);
            }).WithMessage(RoleValidationMessages.NotFound);

        RuleFor(user => user.UserId)
            .MustAsync(async (userId, cancellationToken) => await unitOfWork.Users.AnyById(userId))
            .WithMessage(UserValidationMessages.NotFound);
        RuleFor(request => request)
            .Must(request => request.ChangerId != request.UserId)
            .WithMessage(RoleValidationMessages.CanNotChangeYourOwn)
            .OverridePropertyName($"{nameof(UpdateRoleCommand.UserId)} And {nameof(UpdateRoleCommand.ChangerId)}");
        RuleFor(request => request.ChangerId)
            .MustAsync(async (userId, cancellationToken) => await unitOfWork.Users.AnyById(userId))
            .WithMessage(UserValidationMessages.NotFound);
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) => await unitOfWork.Users.AnyById(userId))
            .WithMessage(UserValidationMessages.NotFound);
    }
}