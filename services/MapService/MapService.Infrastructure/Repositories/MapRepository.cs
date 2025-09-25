using MapService.Domain.Models;
using MapService.Infrastructure.Persistence;
using MapService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MapService.Infrastructure.Repositories
{
    public class MapRepository(MapServiceContext dbContext) : IMapRepository
    {
        private MapServiceContext _dbContext = dbContext;
        public async Task<IEnumerable<Map>> GetAllAsync() => await _dbContext.Maps.ToListAsync();

    }
}
