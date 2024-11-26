using BooksApp.Application.Roles;
using BooksApp.Contracts.Roles;
using Mapster;

namespace BooksApp.API.Common.Mapping;

public sealed class RolesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RoleResult, RoleResponse>();
        config.NewConfig<IEnumerable<RoleResult>, IEnumerable<RoleResponse>>()
            .Map(dest => dest,
                src => src.Select(x => x.Adapt<RoleResponse>(config)));
    }
}