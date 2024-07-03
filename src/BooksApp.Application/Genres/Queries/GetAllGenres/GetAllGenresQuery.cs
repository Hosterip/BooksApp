using MediatR;

namespace PostsApp.Application.Genres.Queries.GetAllGenres;

public class GetAllGenresQuery : IRequest<List<GenreResult>> { }