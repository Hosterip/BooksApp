using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Security;

namespace PostsApp.Application.Roles.Commands.UpdateRole;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
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
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var targetUser = await unitOfWork.Users.GetSingleById(request.UserId);
                var changerUser = await unitOfWork.Users.GetSingleById(request.ChangerId);
                if (changerUser is null || targetUser is null) return false;
                return RolePermissions.UpdateRole(
                    changerUser.Role.Name,
                    targetUser.Role.Name,
                    request.Role);
            }).WithMessage(UserValidationMessages.Permission);
    }
}