using FluentValidation;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Constants.Exceptions;

namespace PostsApp.Application.Roles.Commands.UpdateRole;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Role)
            .MustAsync(async (roleName, cancellationToken) =>
            {
                return await unitOfWork.Roles.AnyAsync(role => role.Name == roleName);
            }).WithMessage(RoleExceptionConstants.NotFound);    
        
        RuleFor(user => user.UserId)
            .MustAsync(async (userId, cancellationToken) =>
            {
                return await unitOfWork.Users.AnyAsync(user => user.Id == userId);
            }).WithMessage(UserExceptionConstants.NotFound);
        RuleFor(request => request)
            .Must(request => request.ChangerId != request.UserId)
            .WithMessage("You can not change your role")
            .OverridePropertyName("UserId And ChangerId");
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var targetUser = await unitOfWork.Users.GetSingleWhereAsync(user => user.Id == request.UserId);
                var user = await unitOfWork.Users.GetSingleWhereAsync(user => user.Id == request.ChangerId);
                if (user is null || targetUser is null) return false;
                if (user.Role.Name == RoleConstants.Moderator &&
                    (request.Role == RoleConstants.Moderator ||
                     request.Role == RoleConstants.Admin ||
                     targetUser.Role.Name == RoleConstants.Moderator ||
                     targetUser.Role.Name == RoleConstants.Admin))
                    return false;
                
                return user.Role.Name == RoleConstants.Admin || user.Role.Name == RoleConstants.Moderator;
            }).WithMessage(UserExceptionConstants.Permission);
    }
}