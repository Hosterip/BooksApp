using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Interfaces;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Books.Commands.UpdateBook;

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
        var book = await _unitOfWork.Books.GetSingleById(request.Id, cancellationToken);
        if (request.Title != null)
            book!.Title = request.Title;
        if (request.Description != null)
            book!.Description = request.Description;
        if (request.Image != null)
        {
            _imageFileBuilder.DeleteImage(book!.Cover.ImageName);
            var fileName = await _imageFileBuilder.CreateImage(request.Image, cancellationToken);

            book!.Cover.ImageName = fileName!;
        }

        var genres = await _unitOfWork.Genres.GetAllByIds(request.GenreIds, cancellationToken);
        
        book!.ChangeGenres(genres.ToList());

        await _unitOfWork.SaveAsync(cancellationToken);
        var result = _mapper.Map<BookResult>(book);
        var bookStats = _unitOfWork.Books.RatingStatistics(book.Id.Value);
        result.AverageRating = bookStats.AverageRating;
        result.Ratings = bookStats.Ratings;
        return result;
    }
}