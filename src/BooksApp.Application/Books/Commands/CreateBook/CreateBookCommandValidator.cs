using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Enums.MaxLengths;
using BooksApp.Domain.Common.Security;
using FluentValidation;

namespace BooksApp.Application.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator(IUnitOfWork unitOfWork, IImageFileBuilder imageFileBuilder)
    {
        // Books validation
        RuleFor(book => book.Title)
            .NotEmpty()
            .MaximumLength((int)BookMaxLengths.Title);
        RuleFor(book => book.Description)
            .NotEmpty()
            .MaximumLength((int)BookMaxLengths.Description);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                !await unitOfWork.Books.AnyByTitle(request.UserId, request.Title))
            .WithMessage(BookValidationMessages.WithSameNameAlreadyExists)
            .OverridePropertyName(nameof(CreateBookCommand.Title));

        // User Validation

        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleById(userId);
                return user is not null && RolePermissions.CreateBook(user.Role.Name);
            })
            .WithMessage(BookValidationMessages.MustBeAnAuthor);
        RuleFor(book => book.UserId)
            .MustAsync(async (id, cancellationToken) => await unitOfWork.Users.AnyById(id))
            .WithMessage(UserValidationMessages.NotFound);

        // Genres Validation

        RuleFor(request => request.GenreIds).Must(genreIds =>
        {
            if (genreIds is null)
                return false;
            return !unitOfWork.Genres.GetAllByIds(genreIds).Any(genre => genre is null);
        }).WithMessage(BookValidationMessages.GenresNotFound);
        RuleFor(request => request.GenreIds)
            .Must(genreIds => genreIds is not null && genreIds.Any())
            .WithMessage(BookValidationMessages.AtLeastOneGenre);

        // Images

        RuleFor(request => request.Image.Length)
            .LessThan(10000000);
        RuleFor(request => request.Image.FileName)
            .Must(fileName => imageFileBuilder.IsValid(fileName))
            .WithMessage(ImageValidationMessages.WrongFileName);
    }
}