using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentValidation;

namespace BooksApp.Application.Books.Commands.CreateBook;

public sealed class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator(IUnitOfWork unitOfWork, IImageFileBuilder imageFileBuilder)
    {
        // Books validation
        RuleFor(book => book.Title)
            .NotEmpty()
            .MaximumLength(MaxPropertyLength.Book.Title);
        RuleFor(book => book.Description)
            .NotEmpty()
            .MaximumLength(MaxPropertyLength.Book.Description);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                !await unitOfWork.Books.AnyByTitle(request.UserId, request.Title, cancellationToken))
            .WithMessage(BookValidationMessages.WithSameNameAlreadyExists)
            .WithName(nameof(CreateBookCommand.Title));

        // User Validation

        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) => await unitOfWork.Users.AnyById(userId, cancellationToken))
            .WithMessage(BookValidationMessages.MustBeAnAuthor);
        RuleFor(book => book.UserId)
            .MustAsync(async (id, cancellationToken) => await unitOfWork.Users.AnyById(id, cancellationToken))
            .WithMessage(UserValidationMessages.NotFound);

        // Genres Validation

        RuleFor(request => request.GenreIds).MustAsync(
            async (genreIds, cancellationToken) =>
        {
            if (genreIds.Count == 0)
                return false;
            var genres = await unitOfWork.Genres.GetAllByIds(genreIds, cancellationToken);
            return genres.Any();
        }).WithMessage(BookValidationMessages.GenresNotFound);

        // Images

        RuleFor(request => request.Image.Length)
            .LessThan(10000000);
        RuleFor(request => request.Image.FileName)
            .Must(fileName => imageFileBuilder.IsValid(fileName))
            .WithMessage(ImageValidationMessages.WrongFileName);
    }
}