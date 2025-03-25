using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Books.Commands.CreateBook;

public sealed class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator(IUnitOfWork unitOfWork, IImageFileBuilder imageFileBuilder,
        IUserService userService)
    {
        var userId = userService.GetId()!.Value;

        // Books validation
        RuleFor(book => book.Title)
            .NotEmpty()
            .MaximumLength(MaxPropertyLength.Book.Title);
        RuleFor(book => book.Description)
            .NotEmpty()
            .MaximumLength(MaxPropertyLength.Book.Description);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                !await unitOfWork.Books.AnyByTitle(userId, request.Title, cancellationToken))
            .WithMessage(ValidationMessages.Book.WithSameNameAlreadyExists)
            .WithName(nameof(CreateBookCommand.Title));

        // User Validation

        RuleFor(request => request)
            .MustAsync(async (_, cancellationToken) =>
                await unitOfWork.Users.AnyAsync(
                    x => x.Id == UserId.Create(userId) && x.Role.Name == RoleNames.Author,
                    cancellationToken)
            )
            .WithMessage(ValidationMessages.Book.MustBeAnAuthor)
            .WithName(nameof(UserId));
        RuleFor(book => book)
            .MustAsync(async (_, cancellationToken) => await unitOfWork.Users.AnyById(userId, cancellationToken))
            .WithMessage(ValidationMessages.User.NotFound)
            .WithName(nameof(UserId));

        // Genres Validation

        RuleFor(request => request.GenreIds).MustAsync(
            async (genreIds, cancellationToken) =>
            {
                if (genreIds.Count == 0)
                    return false;
                var genres = await unitOfWork.Genres.GetAllByIds(genreIds, cancellationToken);
                return genres.Any();
            }).WithMessage(ValidationMessages.Book.GenresNotFound);

        // Images

        RuleFor(request => request.Image.Length)
            .LessThan(10000000);
        RuleFor(request => request.Image.FileName)
            .Must(imageFileBuilder.IsValid)
            .WithMessage(ValidationMessages.Image.InvalidFileName);
    }
}