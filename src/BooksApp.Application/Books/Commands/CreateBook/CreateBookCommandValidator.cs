using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Constants.ValidationMessages;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Enums.MaxLengths;
using PostsApp.Domain.Common.Security;

namespace PostsApp.Application.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator(IUnitOfWork unitOfWork, IImageFileBuilder imageFileBuilder)
    {
        RuleFor(book => book.Title)
            .NotEmpty()
            .MaximumLength((int)BookMaxLengths.Title);
        RuleFor(post => post.Description)
            .NotEmpty()
            .MaximumLength((int)BookMaxLengths.Description);
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleById(userId);
                return user is not null && RolePermissions.CreateBook(user.Role.Name);
            })
            .WithMessage(BookValidationMessages.MustBeAnAuthor);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) => 
                !await unitOfWork.Books.AnyByRefName(request.UserId, request.Title))
            .WithMessage(BookValidationMessages.WithSameNameAlreadyExists)
            .OverridePropertyName("Title");
        RuleFor(book => book.UserId)
            .MustAsync(async (id, cancellationToken) => await unitOfWork.Users.AnyById(id))
            .WithMessage(UserValidationMessages.NotFound);
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