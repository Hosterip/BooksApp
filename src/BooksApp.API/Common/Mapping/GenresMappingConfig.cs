using BooksApp.Application.Genres;
using BooksApp.Contracts.Genres;
using Mapster;

namespace BooksApp.API.Common.Mapping;

public sealed class GenresMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GenreResult, GenreResponse>();

        config.NewConfig<IEnumerable<GenreResult>, IEnumerable<GenreResult>>()
            .Map(dest => dest, 
                src => src.Select(x => x.Adapt<GenreResponse>(config)));
    }
}