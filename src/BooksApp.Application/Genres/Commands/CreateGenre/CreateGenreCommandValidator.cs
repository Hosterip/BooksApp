using FluentValidation;
using PostsApp.Application.Common.Constants.ValidationMessages;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Name)
            .MustAsync(async (name, cancellationToken) =>
            {
                return !await unitOfWork.Genres.AnyAsync(genre => genre.Name.ToLower() == name.ToLower());
            })
            .WithMessage(GenreValidationMessages.AlreadyExists);
    }
}