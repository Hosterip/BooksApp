using Mapster;
using PostsApp.Application.Reviews.Results;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.Review;

namespace PostsApp.Application.Common.Mapping;

public class ReviewMappingConfig: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Review, ReviewResult>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.User, src => src.User.Adapt<UserResult>(config))
            .Map(dest => dest.BookId, src => src.Book.Id.Value.ToString());
    }
}