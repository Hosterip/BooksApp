using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.Common.Constants;
using BooksApp.Domain.Common.Enums.MaxLengths;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Books.Commands.UpdateBook;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator(IUnitOfWork unitOfWork, IImageFileBuilder imageFileBuilder)
    {
        // Books
        RuleFor(book => book.Title)
            .MaximumLength((int)BookMaxLengths.Title);
        RuleFor(post => post.Description)
            .MaximumLength((int)BookMaxLengths.Description);

        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                request.Title == null || !await unitOfWork.Books.AnyByTitle(request.UserId, request.Title))
            .WithMessage(BookValidationMessages.WithSameNameAlreadyExists)
            .OverridePropertyName(nameof(UpdateBookCommand.Title));
        RuleFor(book => book)
            .MustAsync(async (request, cancellationToken) =>
            {
                var canUpdate = await unitOfWork.Users
                    .AnyAsync(user => user.Id == UserId.CreateUserId(request.UserId) &&
                                      (user.Role.Name == RoleNames.Admin || user.Role.Name == RoleNames.Moderator));
                return await unitOfWork.Books.AnyAsync(book =>
                    book.Id == BookId.CreateBookId(request.Id) &&
                    (book.Author.Id == UserId.CreateUserId(request.UserId) || canUpdate));
            }).WithMessage(BookValidationMessages.BookNotYour);

        // Genres

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
        RuleFor(request => request.Image)
            .Must(file => file == null || imageFileBuilder.IsValid(file.FileName))
            .WithMessage(ImageValidationMessages.WrongFileName);
    }
}