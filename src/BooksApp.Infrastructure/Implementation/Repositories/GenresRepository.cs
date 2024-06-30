using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Domain.Genre;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation.Repositories;

public class GenresRepository : GenericRepository<Genre>, IGenresRepository 
{
    public GenresRepository(AppDbContext dbContext) : base(dbContext) { }
}