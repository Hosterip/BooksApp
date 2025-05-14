using BooksApp.Application.Bookshelves;
using BooksApp.Contracts.Bookshelves;
using Mapster;

namespace BooksApp.API.Common.Mapping;

public sealed class BookshelvesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<BookshelfResult, BookshelfResponse>();

        config.NewConfig<IEnumerable<BookshelfResult>, IEnumerable<BookshelfResponse>>()
            .Map(dest => dest,
                src => src.Select(x => x.Adapt<BookshelfResponse>(config)));
    }
}