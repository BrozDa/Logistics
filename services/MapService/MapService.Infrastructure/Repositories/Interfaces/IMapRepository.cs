using MapService.Domain.Models;

namespace MapService.Infrastructure.Repositories.Interfaces
{
    public interface IMapRepository
    {
        public Task<IEnumerable<Map>> GetAllAsync();
    }
}
