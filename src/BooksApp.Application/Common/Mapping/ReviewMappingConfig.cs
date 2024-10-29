using BooksApp.Application.Reviews.Results;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.Review;
using Mapster;

namespace BooksApp.Application.Common.Mapping;

public class ReviewMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Review, ReviewResult>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.User, src => src.User.Adapt<UserResult>(config))
            .Map(dest => dest.BookId, src => src.Book.Id.Value.ToString());
    }
}