﻿using MediatR;

namespace BooksApp.Application.Genres.Queries.GetAllGenres;

public class GetAllGenresQuery : IRequest<IEnumerable<GenreResult>>
{
}