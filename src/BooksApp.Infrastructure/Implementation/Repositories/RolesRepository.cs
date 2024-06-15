using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Domain.Role;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation.Repositories;

public class RolesRepository : GenericRepository<Role>, IRolesRepository
{
    public RolesRepository(AppDbContext dbContext) : base(dbContext) { }
}