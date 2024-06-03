using FluentValidation;
using PostsApp.Application.Common.Constants;
using PostsApp.Domain.Constants;

namespace PostsApp.Application.Images.Commands.CreateImage;

public class CreateImageCommandValidator : AbstractValidator<CreateImageCommand>
{
    public CreateImageCommandValidator()
    {
        RuleFor(request => request.Image.Length)
            .LessThan(10000000);
        RuleFor(request => request.Image.FileName)
            .Must(fileName =>
            {
                var path = Path.Combine(
                    Environment.GetEnvironmentVariable(EnvironmentNames.ImageFolderPath) ?? "images",
                    fileName
                    );
                var fileInfo = new FileInfo(path);
                var extension = fileInfo.Extension.Replace(".", "");
                var allowedExtensions = AppConstants.AllowedExtensions.Split(",");
                foreach (var allowedExtension in allowedExtensions)
                {
                    if (extension == allowedExtension) 
                        return true;
                }
                return false;
            })
            .WithMessage($"Wrong file extension. Allowed extensions: {AppConstants.AllowedExtensions}");
    }
}