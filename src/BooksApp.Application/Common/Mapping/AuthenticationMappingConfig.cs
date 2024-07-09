using Mapster;
using PostsApp.Application.Auth;
using PostsApp.Domain.User;

namespace PostsApp.Application.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, AuthResult>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.Role, src => src.Role.Name)
            .Map(dest => dest.AvatarName, src => src.Avatar.ImageName);
    }
}