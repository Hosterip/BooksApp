using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Users.Commands.AddRemoveFollower;

public sealed class AddRemoveFollowerCommandValidator : AbstractValidator<AddRemoveFollowerCommand>
{
    public AddRemoveFollowerCommandValidator(IUnitOfWork unitOfWork, IUserService userService)
    {
        var followerId = userService.GetId()!.Value;

        RuleFor(x => x.UserId)
            .MustAsync(async (userId, token) =>
                await unitOfWork.Users.AnyById(userId, token))
            .WithMessage(ValidationMessages.User.NotFound);

        RuleFor(x => x)
            .Must(request =>
                followerId != request.UserId)
            .WithMessage(ValidationMessages.User.CantFollowYourself)
            .WithName(nameof(followerId));
    }
}