using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using FluentValidation;

namespace BooksApp.Application.Roles.Commands.UpdateRole;

public sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator(IUnitOfWork unitOfWork, IUserService userService)
    {
        var changerId = userService.GetId()!.Value;

        RuleFor(request => request.Role)
            .MustAsync(async (roleName, cancellationToken) =>
            {
                return await unitOfWork.Roles.AnyAsync(
                    role => role.Name == roleName,
                    cancellationToken);
            }).WithMessage(ValidationMessages.Role.NotFound);

        RuleFor(request => request)
            .MustAsync(async (_, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleById(changerId, cancellationToken);

                return user?.Role.Name is RoleNames.Admin;
            })
            .WithMessage(ValidationMessages.User.Permission)
            .WithName(nameof(changerId));

        RuleFor(request => request)
            .Must(request => changerId != request.UserId)
            .WithMessage(ValidationMessages.Role.CanNotChangeYourOwn)
            .WithName($"{nameof(UpdateRoleCommand.UserId)} And {nameof(changerId)}");

        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) => 
                await unitOfWork.Users.AnyById(userId, cancellationToken))
            .WithMessage(ValidationMessages.User.NotFound);
    }
}