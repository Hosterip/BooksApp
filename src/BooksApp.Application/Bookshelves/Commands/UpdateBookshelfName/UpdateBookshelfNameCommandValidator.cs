using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Bookshelf.ValueObjects;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Commands.UpdateBookshelfName;

public sealed class UpdateBookshelfNameCommandValidator : AbstractValidator<UpdateBookshelfNameCommand>
{
    public UpdateBookshelfNameCommandValidator(IUnitOfWork unitOfWork, IUserService userService)
    {
        var userId = userService.GetId()!.Value;
        RuleFor(x => x.BookshelfId)
            .MustAsync(async (bookshelfId, token) =>
            {
                return await unitOfWork.Bookshelves
                    .AnyAsync(x => x.Id == BookshelfId.Create(bookshelfId) &&
                                   x.UserId == UserId.Create(userId), token);
            })
            .WithMessage(ValidationMessages.Bookshelf.NotYours);
        
        RuleFor(x => x)
            .MustAsync(async (request, token) =>
            {
                var bookshelf = await unitOfWork.Bookshelves.GetSingleById(request.BookshelfId, token);
                return bookshelf?.Name != request.NewName;
            })
            .WithName(nameof(UpdateBookshelfNameCommand.NewName))
            .WithMessage(ValidationMessages.Bookshelf.NameIsTheSameAsItWas);
    }
}