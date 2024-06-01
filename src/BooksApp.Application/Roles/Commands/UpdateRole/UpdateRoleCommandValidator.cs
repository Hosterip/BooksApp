using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Security;

namespace PostsApp.Application.Roles.Commands.UpdateRole;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Role)
            .MustAsync(async (roleName, cancellationToken) =>
            {
                return await unitOfWork.Roles.AnyAsync(role => role.Name == roleName);
            }).WithMessage(ConstantsRoleException.NotFound);    
        
        RuleFor(user => user.UserId)
            .MustAsync(async (userId, cancellationToken) =>
            {
                return await unitOfWork.Users.AnyAsync(user => user.Id == userId);
            }).WithMessage(ConstantsUserException.NotFound);
        RuleFor(request => request)
            .Must(request => request.ChangerId != request.UserId)
            .WithMessage("You can not change your own role")
            .OverridePropertyName("UserId And ChangerId");
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var targetUser = await unitOfWork.Users.GetSingleWhereAsync(user => user.Id == request.UserId);
                var changerUser = await unitOfWork.Users.GetSingleWhereAsync(user => user.Id == request.ChangerId);
                if (changerUser is null || targetUser is null) return false;
                return RolePermissions.UpdateRole(
                    changerUser.Role.Name, 
                    targetUser.Role.Name,
                    request.Role);
            }).WithMessage(ConstantsUserException.Permission);
    }
}