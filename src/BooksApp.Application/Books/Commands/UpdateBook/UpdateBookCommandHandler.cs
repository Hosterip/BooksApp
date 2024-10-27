using MapsterMapper;
using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Books.Commands.UpdateBook;

internal sealed class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookResult>
{
    private readonly IImageFileBuilder _imageFileBuilder;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IImageFileBuilder imageFileBuilder)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _imageFileBuilder = imageFileBuilder;
    }

    public async Task<BookResult> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetSingleById(request.Id);
        if (request.Title != null)
            book!.Title = request.Title;
        if (request.Description != null)
            book!.Description = request.Description;
        if (request.Image != null)
        {
            await _imageFileBuilder.DeleteImage(book.Cover.ImageName, cancellationToken);
            var fileName = await _imageFileBuilder.CreateImage(request.Image, cancellationToken);

            book!.Cover.ImageName = fileName;
        }

        book!.Genres = _unitOfWork.Genres.GetAllByIds(request.GenreIds).ToList()!;

        await _unitOfWork.SaveAsync(cancellationToken);
        var result = _mapper.Map<BookResult>(book);
        var bookStats = _unitOfWork.Books.RatingStatistics(book.Id.Value);
        result.AverageRating = bookStats.AverageRating;
        result.Ratings = bookStats.Ratings;
        return result;
    }
}