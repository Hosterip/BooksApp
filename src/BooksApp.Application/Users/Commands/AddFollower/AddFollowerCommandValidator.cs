using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Users.Commands.AddFollower;

public sealed class AddFollowerCommandValidator : AbstractValidator<AddFollowerCommand>
{
    public AddFollowerCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.UserId)
            .MustAsync(((guid, token) => unitOfWork.Users.AnyById(guid)))
            .WithMessage(UserValidationMessages.NotFound);
        
        RuleFor(x => x.FollowerId)
            .MustAsync(((guid, token) => unitOfWork.Users.AnyById(guid)))
            .WithMessage(UserValidationMessages.NotFound);
        
        RuleFor(x => x)
            .MustAsync(((request, token) => unitOfWork.Users
                .AnyFollower(request.UserId, request.FollowerId)))
            .WithMessage(UserValidationMessages.AlreadyFollowing)
            .OverridePropertyName(nameof(AddFollowerCommand.FollowerId));
        
        RuleFor(x => x)
            .Must(request => request.FollowerId != request.UserId)
            .WithMessage(UserValidationMessages.CantFollowYourself)
            .OverridePropertyName(nameof(AddFollowerCommand.FollowerId));
    }
}