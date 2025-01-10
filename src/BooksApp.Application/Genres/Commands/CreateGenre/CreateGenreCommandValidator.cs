using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentValidation;

namespace BooksApp.Application.Genres.Commands.CreateGenre;

public sealed class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Name)
            .MustAsync(async (name, cancellationToken) =>
            {
                return !await unitOfWork.Genres.AnyAsync(
                    genre => genre.Name.ToLower() == name.ToLower(),
                    cancellationToken);
            })
            .WithMessage(GenreValidationMessages.AlreadyExists);

        RuleFor(x => x.Name)
            .MinimumLength(1)
            .MaximumLength(GenreMaxLengths.Name);
    }
}