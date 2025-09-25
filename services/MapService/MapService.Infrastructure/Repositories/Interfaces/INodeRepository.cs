using MapService.Domain.Models;

namespace MapService.Infrastructure.Repositories.Interfaces
{
    public interface INodeRepository
    {
        public Task<IEnumerable<Node>> GetAllAsync();
    }
}
