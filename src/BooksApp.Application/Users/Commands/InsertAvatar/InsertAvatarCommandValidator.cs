using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Users.Commands.InsertAvatar;

public sealed class InsertAvatarCommandValidator : AbstractValidator<InsertAvatarCommand>
{
    public InsertAvatarCommandValidator(
        IUnitOfWork unitOfWork,
        IUserService userService,
        IImageFileBuilder imageFileBuilder)
    {
        var userId = userService.GetId()!.Value;

        RuleFor(request => request)
            .MustAsync(async (_, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId, cancellationToken))
            .WithMessage(ValidationMessages.User.NotFound)
            .WithName(nameof(UserId));
        
        // Images
        RuleFor(request => request.Image.Length)
            .LessThan(10000000);
        RuleFor(request => request.Image)
            .Must(file => file != null && imageFileBuilder.IsValid(file.FileName))
            .WithMessage(ValidationMessages.Image.InvalidFileName);
    }
}