using FluentValidation;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Constants.Exceptions;

namespace PostsApp.Application.Books.Commands.UpdateBook;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(post => post.Title).NotEmpty().Length(1, 255);
        RuleFor(post => post.Body).NotEmpty().Length(1, 255);
        RuleFor(post => post)
            .MustAsync(async (request, cancellationToken) =>
            {
                var isAdmin = await unitOfWork.Users
                    .AnyAsync(user => user.Id == request.UserId && user.Role == Roles.Admin);
                return await unitOfWork.Posts.AnyAsync(book => 
                    book.Id == request.Id &&
                    (book.Author.Id == request.UserId || isAdmin));
            }).WithMessage(BookExceptionConstants.PostNotYour);
    }
}