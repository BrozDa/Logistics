using MapService.Domain.Models;
using MapService.Infrastructure.Persistence;
using MapService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MapService.Infrastructure.Repositories
{
    public class NodeRepository(MapServiceContext dbContext) : INodeRepository
    {
        private readonly MapServiceContext _dbContext = dbContext;
        public async Task<IEnumerable<Node>> GetAllAsync() => await _dbContext.Nodes.ToListAsync();
    }
}
