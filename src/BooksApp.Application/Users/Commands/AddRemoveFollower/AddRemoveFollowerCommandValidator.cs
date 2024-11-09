using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Users.Commands.AddRemoveFollower;

public sealed class AddRemoveFollowerCommandValidator : AbstractValidator<AddRemoveFollowerCommand>
{
    public AddRemoveFollowerCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.UserId)
            .MustAsync((guid, token) => unitOfWork.Users.AnyById(
                guid, token))
            .WithMessage(UserValidationMessages.NotFound);
        
        RuleFor(x => x.FollowerId)
            .MustAsync((guid, token) =>
                unitOfWork.Users.AnyById(guid, token))
            .WithMessage(UserValidationMessages.NotFound);
        
        RuleFor(x => x)
            .Must(request =>
                request.FollowerId != request.UserId)
            .WithMessage(UserValidationMessages.CantFollowYourself)
            .OverridePropertyName(nameof(AddRemoveFollowerCommand.FollowerId));
    }
}