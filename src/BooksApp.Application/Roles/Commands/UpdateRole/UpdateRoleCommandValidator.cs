using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using FluentValidation;

namespace BooksApp.Application.Roles.Commands.UpdateRole;

public sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Role)
            .MustAsync(async (roleName, cancellationToken) =>
            {
                return await unitOfWork.Roles.AnyAsync(
                    role => role.Name == roleName,
                    cancellationToken);
            }).WithMessage(RoleValidationMessages.NotFound);

        RuleFor(request => request.ChangerId)
            .MustAsync(async (userId, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleById(
                    userId, cancellationToken);

                return user?.Role.Name is RoleNames.Admin;
            })
            .WithMessage(UserValidationMessages.Permission);

        RuleFor(request => request)
            .Must(request => request.ChangerId != request.UserId)
            .WithMessage(RoleValidationMessages.CanNotChangeYourOwn)
            .WithName($"{nameof(UpdateRoleCommand.UserId)} And {nameof(UpdateRoleCommand.ChangerId)}");

        RuleFor(request => request.ChangerId)
            .MustAsync(async (userId, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId, cancellationToken))
            .WithMessage(UserValidationMessages.NotFound);

        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) => 
                await unitOfWork.Users.AnyById(userId, cancellationToken))
            .WithMessage(UserValidationMessages.NotFound);
    }
}