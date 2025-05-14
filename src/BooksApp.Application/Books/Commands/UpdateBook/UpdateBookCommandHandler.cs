using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Interfaces;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Books.Commands.UpdateBook;

internal sealed class UpdateBookCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageFileBuilder imageFileBuilder)
    : IRequestHandler<UpdateBookCommand, BookResult>
{
    public async Task<BookResult> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await unitOfWork.Books.GetSingleById(request.Id, cancellationToken);
        if (request.Title != null)
            book!.ChangeTitle(request.Title);
        if (request.Description != null)
            book!.ChangeDescription(request.Description);
        if (request.Image != null)
        {
            imageFileBuilder.DeleteImage(book!.Cover.ImageName);
            var fileName = await imageFileBuilder.CreateImage(request.Image, cancellationToken);

            book.Cover.ChangeImageName(fileName!);
        }

        var genres = await unitOfWork.Genres.GetAllByIds(request.GenreIds, cancellationToken);

        book!.ChangeGenres(genres.ToList());

        await unitOfWork.Books.Update(book);
        await unitOfWork.SaveAsync(cancellationToken);

        var result = mapper.Map<BookResult>(book);
        var bookStats = unitOfWork.Books.RatingStatistics(book.Id.Value);
        result.AverageRating = bookStats.AverageRating;
        result.Ratings = bookStats.Ratings;
        return result;
    }
}