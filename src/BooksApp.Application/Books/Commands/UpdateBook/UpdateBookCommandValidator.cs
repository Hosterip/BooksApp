using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Books.Commands.UpdateBook;

public sealed class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator(
        IUnitOfWork unitOfWork,
        IImageFileBuilder imageFileBuilder,
        IUserService userService)
    {
        var userId = userService.GetId()!.Value;

        // Books
        RuleFor(book => book.Title)
            .MaximumLength(MaxPropertyLength.Book.Title);
        RuleFor(post => post.Description)
            .MaximumLength(MaxPropertyLength.Book.Description);

        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                request.Title == null || !await unitOfWork.Books.AnyByTitle(userId, request.Title, cancellationToken))
            .WithMessage(ValidationMessages.Book.WithSameNameAlreadyExists)
            .WithName(nameof(UpdateBookCommand.Title));
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                await unitOfWork.Books
                    .AnyAsync(book => book.Id == BookId.Create(request.Id) &&
                                      book.Author.Id == UserId.Create(userId), cancellationToken)
            )
            .WithMessage(ValidationMessages.Book.BookNotYour)
            .WithName(nameof(UserId));

        // Genres

        RuleFor(request => request.GenreIds).MustAsync(
            async (genreIds, cancellationToken) =>
        {
            if (genreIds.Count == 0)
                return false;
            var genres = await unitOfWork.Genres.GetAllByIds(genreIds, cancellationToken);
            return genres.Any();
        }).WithMessage(ValidationMessages.Book.GenresNotFound);

        // Images

        RuleFor(request => request.Image!.Length)
            .LessThan(10000000);
        RuleFor(request => request.Image)
            .Must(file => file == null || imageFileBuilder.IsValid(file.FileName))
            .WithMessage(ValidationMessages.Image.WrongFileName);
    }
}