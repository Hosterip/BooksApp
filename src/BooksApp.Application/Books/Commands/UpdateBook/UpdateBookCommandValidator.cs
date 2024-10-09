using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Constants.ValidationMessages;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Book.ValueObjects;
using PostsApp.Domain.Common.Constants;
using PostsApp.Domain.Common.Enums.MaxLengths;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Application.Books.Commands.UpdateBook;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(book => book.Title)
            .MaximumLength((int)BookMaxLengths.Title);
        RuleFor(post => post.Description)
            .MaximumLength((int)BookMaxLengths.Description);
        
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) => 
                request.Title == null || !await unitOfWork.Books.AnyByRefName(request.UserId, request.Title))
            .WithMessage(BookValidationMessages.WithSameNameAlreadyExists)
            .OverridePropertyName("Title");
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
        RuleFor(request => request.GenreIds).Must(genreIds =>
        {
            if (genreIds is null)
                return false;
            return !unitOfWork.Genres.GetAllByIds(genreIds).Any(genre => genre is null);
        }).WithMessage(BookValidationMessages.GenresNotFound);
        RuleFor(request => request.GenreIds)
            .Must(genreIds => genreIds is not null && genreIds.Any())
            .WithMessage(BookValidationMessages.AtLeastOneGenre);
    }
}