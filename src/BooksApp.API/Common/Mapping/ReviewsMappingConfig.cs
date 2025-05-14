using BooksApp.Application.Reviews.Results;
using BooksApp.Contracts.Reviews;
using BooksApp.Contracts.Users;
using Mapster;

namespace BooksApp.API.Common.Mapping;

public sealed class ReviewsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ReviewResult, ReviewResponse>()
            .Map(dest => dest.User,
                src => src.User.Adapt<ExtendedUserResponse>(config))
            .Map(dest => dest.Text,
                src => src.Body);
    }
}