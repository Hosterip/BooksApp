using FluentValidation;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Constants;

namespace PostsApp.Application.Bookshelves.Commands.DeleteBookshelf;

public class DeleteBookshelfCommandValidator : AbstractValidator<DeleteBookshelfCommand>
{
    public DeleteBookshelfCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.BookshelfId)
            .MustAsync(async (bookshelfId, cancellationToken) =>
            {
                var bookshelf = await unitOfWork.Bookshelves.GetSingleById(bookshelfId);

                if (bookshelf is null) return false;
                if (DefaultBookshelvesNames.AllValues.Contains(bookshelf.ReferentialName)) return false;
                return true;
            });
    }
}