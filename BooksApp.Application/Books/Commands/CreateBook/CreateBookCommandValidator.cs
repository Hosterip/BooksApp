using FluentValidation;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Constants.Exceptions;

namespace PostsApp.Application.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(post => post.Title).NotEmpty().Length(1, 255);
        RuleFor(post => post.Description).NotEmpty().Length(1, 1000);
        RuleFor(post => post.Id).MustAsync(async (id, cancellationToken) =>
        {
            return await unitOfWork.Users
                .AnyAsync(user => 
                    user.Id == id);
        }).WithMessage(UserExceptionConstants.NotFound);
        RuleFor(post => post.Id).MustAsync(async (id, cancellationToken) =>
        {
            return await unitOfWork.Users
                .AnyAsync(user => user.Role == Roles.Author);
        }).WithMessage(BookExceptionConstants.MustBeAnAuthor);
        
    }
}