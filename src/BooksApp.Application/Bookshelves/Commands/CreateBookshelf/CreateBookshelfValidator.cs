using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Constants.ValidationMessages;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Bookshelves.Commands.CreateBookshelf;

public class CreateBookshelfValidator : AbstractValidator<CreateBookshelfCommand>
{
    public CreateBookshelfValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) => 
                await unitOfWork.Users.AnyById(userId))
            .WithMessage(UserValidationMessages.NotFound);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) => 
                await unitOfWork.Bookshelves.AnyByRefName(request.UserId, request.Name))
            .WithMessage(BookshelfValidationMessages.AlreadyHaveWithSameName);
    }
}