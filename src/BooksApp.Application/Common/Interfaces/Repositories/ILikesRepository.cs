using PostsApp.Domain.Models;

namespace PostsApp.Application.Common.Interfaces.Repositories;

public interface ILikesRepository : IGenericRepository<Like>
{
    public Task<int> CountLikes(int id);
}