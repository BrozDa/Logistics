using MapService.Domain.Models;
using MapService.Infrastructure.Persistence;
using MapService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MapService.Infrastructure.Repositories
{
    public class EdgeRepository(MapServiceContext dbContext) : IEdgeRepository
    {
        private readonly MapServiceContext _dbContext = dbContext;
        public async Task<IEnumerable<Edge>> GetAllAsync() => await _dbContext.Edges.ToListAsync();
    }
}
