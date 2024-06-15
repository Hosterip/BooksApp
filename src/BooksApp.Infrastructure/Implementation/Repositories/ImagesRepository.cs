using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Domain.Image;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation.Repositories;

public class ImagesRepository :  GenericRepository<Image>, IImagesRepository
{
    public ImagesRepository(AppDbContext dbContext) : base(dbContext) { }
}