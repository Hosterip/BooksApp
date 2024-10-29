using BooksApp.Application.Bookshelves;
using BooksApp.Domain.Bookshelf;
using Mapster;

namespace BooksApp.Application.Common.Mapping;

public class BookshelfMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Bookshelf, BookshelfResult>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.ReferentialName, src => src.ReferentialName);
    }
}