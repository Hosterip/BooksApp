using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Users.Commands.InsertAvatar;

public sealed class InsertAvatarCommandValidator : AbstractValidator<InsertAvatarCommand>
{
    public InsertAvatarCommandValidator(IUnitOfWork unitOfWork, IImageFileBuilder imageFileBuilder)
    {
        RuleFor(user => user.Id)
            .MustAsync(async (id, cancellationToken) => await unitOfWork.Users.AnyById(id, cancellationToken))
            .WithMessage(ValidationMessages.User.NotFound);
        
        // Images
        RuleFor(request => request.Image.Length)
            .LessThan(10000000);
        RuleFor(request => request.Image)
            .Must(file => file == null || imageFileBuilder.IsValid(file.FileName))
            .WithMessage(ValidationMessages.Image.WrongFileName);
    }
}