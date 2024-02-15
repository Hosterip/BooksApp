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
                var doCan = await unitOfWork.Users
                    .AnyAsync(user => user.Id == request.UserId &&
                                      (user.Role.Name == RoleConstants.Admin || user.Role.Name == RoleConstants.Moderator));
                return await unitOfWork.Posts.AnyAsync(book => 
                    book.Id == request.Id &&
                    (book.Author.Id == request.UserId || doCan));
            }).WithMessage(BookExceptionConstants.PostNotYour);
    }
}