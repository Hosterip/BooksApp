using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Book.ValueObjects;
using PostsApp.Domain.Common.Constants;
using PostsApp.Domain.User.ValueObjects;

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
                var canUpdate = await unitOfWork.Users
                    .AnyAsync(user => user.Id == UserId.CreateUserId(request.UserId) &&
                                      (user.Role.Name == RoleNames.Admin || user.Role.Name == RoleNames.Moderator));
                return await unitOfWork.Books.AnyAsync(book => 
                    book.Id == BookId.CreateBookId(request.Id) &&
                    (book.Author.Id == UserId.CreateUserId(request.UserId) || canUpdate));
            }).WithMessage(ConstantsBookException.PostNotYour);
    }
}