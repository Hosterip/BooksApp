using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Users.Commands.InsertAvatar;

public sealed class InsertAvatarCommandValidator : AbstractValidator<InsertAvatarCommand>
{
    public InsertAvatarCommandValidator(
        IImageFileBuilder imageFileBuilder)
    {
        // Images
        RuleFor(request => request.Image.Length)
            .LessThan(10000000);
        RuleFor(request => request.Image)
            .Must(file => file != null && imageFileBuilder.IsValid(file.FileName))
            .WithMessage(ValidationMessages.Image.InvalidFileName);
    }
}