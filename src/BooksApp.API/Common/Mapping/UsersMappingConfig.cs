using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using BooksApp.Contracts.Users;
using Mapster;

namespace BooksApp.API.Common.Mapping;

public sealed class UsersMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserResult, ExtendedUserResponse>()
            .Map(dest => dest.ViewerRelationship,
                src => src.ViewerRelationship.Adapt<ViewerRelationshipResponse>(config));
        config.NewConfig<UsersResponse, PaginatedArray<UserResult>>()
            .Map(dest => dest.Items,
                src => src.Items.Select(x => x.Adapt<ExtendedUserResponse>(config)));
    }
}