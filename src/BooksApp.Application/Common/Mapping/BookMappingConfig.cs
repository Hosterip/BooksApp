using Mapster;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Genres;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.Book;

namespace PostsApp.Application.Common.Mapping;

public class BookMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Book, BookResult>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.CoverName, src => src.Cover.ImageName)
            .Map(dest => dest.ReferentialName, src => src.ReferentialName)
            .Map(dest => dest.Author, src => src.Author.Adapt<UserResult>(config))
            .Map(dest => dest.Genres,
                src => src.Genres.Select(genre => new GenreResult { Id = genre.Id.Value, Name = genre.Name }).ToList());
    }
}