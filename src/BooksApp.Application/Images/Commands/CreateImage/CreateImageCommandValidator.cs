using FluentValidation;
using PostsApp.Application.Common.Constants;
using PostsApp.Application.Common.Constants.ValidationMessages;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Constants;

namespace PostsApp.Application.Images.Commands.CreateImage;

public class CreateImageCommandValidator : AbstractValidator<CreateImageCommand>
{
    public CreateImageCommandValidator(IImageFileBuilder imageFileBuilder)
    {
        RuleFor(request => request.Image.Length)
            .LessThan(10000000);
        RuleFor(request => request.Image.FileName)
            .Must(fileName => imageFileBuilder.IsValid(fileName))
            .WithMessage(ImageValidationMessages.WrongFileName);
    }
}