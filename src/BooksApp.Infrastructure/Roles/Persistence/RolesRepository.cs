using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Domain.Role;
using BooksApp.Infrastructure.Common.Persistence;

namespace BooksApp.Infrastructure.Roles.Persistence;

public class RolesRepository : GenericRepository<Role>, IRolesRepository
{
    public RolesRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}