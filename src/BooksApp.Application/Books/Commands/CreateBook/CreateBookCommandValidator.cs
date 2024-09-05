using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Security;

namespace PostsApp.Application.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(post => post.Title).NotEmpty().Length(1, 255);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) => 
                await unitOfWork.Books.AnyByRefName(request.UserId, request.Title))
            .WithMessage(UserValidationMessages.NotFound)
            .OverridePropertyName("Title");
        RuleFor(post => post.Description).NotEmpty().Length(1, 5000);
        RuleFor(post => post.UserId)
            .MustAsync(async (id, cancellationToken) => await unitOfWork.Users.AnyById(id))
            .WithMessage(UserValidationMessages.NotFound);
        RuleFor(request => request.GenreIds).Must(genreIds =>
        {
            if (genreIds is null)
                return false;
            return !unitOfWork.Genres.GetAllByIds(genreIds).Any(genre => genre is null);
        }).WithMessage("One or more of the genres weren't found.");
        RuleFor(request => request.GenreIds)
            .Must(genreIds => genreIds is not null && genreIds.Any())
            .WithMessage("Book must have at least one genre.");
    }
}