using FluentValidation;
using PostsApp.Application.Common.Constants.ValidationMessages;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Bookshelves.Queries.GetBookshelfBooks;

public class GetBookshelfBooksQueryValidator : AbstractValidator<GetBookshelfBooksQuery>
{
    public GetBookshelfBooksQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.BookshelfId)
            .MustAsync(async (bookshelfId, cancellationToken) =>
                await unitOfWork.Bookshelves.AnyById(bookshelfId))
            .WithMessage(BookshelfValidationMessages.NotFound);
    }
}