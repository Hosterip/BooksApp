using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Results;
using BooksApp.Contracts.Books;
using BooksApp.Contracts.Genres;
using BooksApp.Contracts.Users;
using Mapster;

namespace BooksApp.API.Common.Mapping;

public class BooksMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<BookResult, BookResponse>()
            .Map(dest => dest.Cover, src => src.CoverName)
            .Map(dest => dest.Slug, src => src.ReferentialName)
            .Map(dest => dest.Author, src => src.Author.Adapt<ExtendedUserResponse>(config))
            .Map(dest => dest.Genres, 
                src => src.Genres.Select(x => x.Adapt<GenreResponse>(config)));
        
        config.NewConfig<PaginatedArray<BookResult>, BooksResponse>()
            .Map(dest => dest.Items,
                src => src.Items.Select(x => x.Adapt<BookResult, BookResponse>(config)));
    }   
}