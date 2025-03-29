using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Users.Commands.AddRemoveFollower;

public sealed class AddRemoveFollowerCommandValidator : AbstractValidator<AddRemoveFollowerCommand>
{
    public AddRemoveFollowerCommandValidator(IUnitOfWork unitOfWork, IUserService userService)
    {
        var followerId = userService.GetId()!.Value;

        RuleFor(x => x.UserId)
            .MustAsync(async (guid, token) => 
                await unitOfWork.Users.AnyById(guid, token))
            .WithMessage(ValidationMessages.User.NotFound);
        
        RuleFor(x => x)
            .MustAsync(async (_, token) =>
                await unitOfWork.Users.AnyById(followerId, token))
            .WithMessage(ValidationMessages.User.NotFound)
            .WithName(nameof(followerId));
        
        RuleFor(x => x)
            .Must(request =>
                followerId != request.UserId)
            .WithMessage(ValidationMessages.User.CantFollowYourself)
            .WithName(nameof(followerId));
    }
}