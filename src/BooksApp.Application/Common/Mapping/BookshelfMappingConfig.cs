using Mapster;
using PostsApp.Application.Bookshelves;
using PostsApp.Domain.Bookshelf;

namespace PostsApp.Application.Common.Mapping;

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