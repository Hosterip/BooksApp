using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Constants;

namespace PostsApp.Application.Books.Commands.UpdateBook;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(post => post.Title).Length(1, 255);
        RuleFor(post => post.Body).Length(1, 255);
        RuleFor(post => post)
            .MustAsync(async (request, cancellationToken) =>
            {
                var doCan = await unitOfWork.Users
                    .AnyAsync(user => user.Id.Value == request.UserId &&
                                      (user.Role.Name == RoleNames.Admin || user.Role.Name == RoleNames.Moderator));
                return await unitOfWork.Books.AnyAsync(book => 
                    book.Id.Value == request.Id &&
                    (book.Author.Id.Value == request.UserId || doCan));
            }).WithMessage(ConstantsBookException.PostNotYour);
    }
}