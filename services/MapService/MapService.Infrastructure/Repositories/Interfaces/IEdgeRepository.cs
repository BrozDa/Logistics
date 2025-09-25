using MapService.Domain.Models;

namespace MapService.Infrastructure.Repositories.Interfaces
{
    public interface IEdgeRepository
    {
        public Task<IEnumerable<Edge>> GetAllAsync();
    }
}
