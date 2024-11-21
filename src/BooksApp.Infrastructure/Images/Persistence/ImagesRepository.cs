using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Domain.Image;
using BooksApp.Infrastructure.Common.Persistence;

namespace BooksApp.Infrastructure.Images.Persistence;

public class ImagesRepository : GenericRepository<Image>, IImagesRepository
{
    public ImagesRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}