using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Domain.Models;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation.Repositories;

public class ImagesRepository :  GenericRepository<Image>, IImageRepository
{
    public ImagesRepository(AppDbContext dbContext) : base(dbContext) { }
}