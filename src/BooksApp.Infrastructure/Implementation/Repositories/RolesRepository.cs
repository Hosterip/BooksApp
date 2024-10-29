using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Domain.Role;
using BooksApp.Infrastructure.Data;

namespace BooksApp.Infrastructure.Implementation.Repositories;

public class RolesRepository : GenericRepository<Role>, IRolesRepository
{
    public RolesRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}