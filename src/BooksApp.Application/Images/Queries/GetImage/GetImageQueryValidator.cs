using BooksApp.Domain.Common.Constants;
using FluentValidation;

namespace BooksApp.Application.Images.Queries.GetImage;

public class GetImageQueryValidator : AbstractValidator<GetImageQuery>
{
    public GetImageQueryValidator()
    {
        RuleFor(request => request.ImageName)
            .Must(imageName =>
            {
                var imagePath =
                    Path.Combine(
                        Environment.GetEnvironmentVariable(EnvironmentNames.ImageFolderPath) ?? "images",
                        imageName
                    );
                var fileInfo = new FileInfo(imagePath);
                return fileInfo.Exists;
            });
    }
}