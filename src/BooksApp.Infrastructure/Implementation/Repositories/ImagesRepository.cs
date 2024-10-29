using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Domain.Image;
using BooksApp.Infrastructure.Data;

namespace BooksApp.Infrastructure.Implementation.Repositories;

public class ImagesRepository : GenericRepository<Image>, IImagesRepository
{
    public ImagesRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}