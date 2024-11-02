using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Images.Queries.GetImage;

public sealed class GetImageQueryValidator : AbstractValidator<GetImageQuery>
{
    public GetImageQueryValidator(IImageFileBuilder fileBuilder)
    {
        RuleFor(request => request.ImageName)
            .Must(imageName => fileBuilder.AnyImage(imageName))
            .WithMessage(ImageValidationMessages.NotFound);
    }
}