using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Genre;

namespace PostsApp.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, GenreResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateGenreCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GenreResult> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = Genre.Create(request.Name);
        await _unitOfWork.Genres.AddAsync(genre);
        await _unitOfWork.SaveAsync(cancellationToken);
        return new GenreResult
        {
            Id = genre.Id.Value,
            Name = genre.Name
        };
    }
}