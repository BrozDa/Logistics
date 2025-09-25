using MapService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MapService.Infrastructure.Persistence
{
    public class MapServiceContext(DbContextOptions<MapServiceContext> options) : DbContext(options)
    {
        public DbSet<Map> Maps { get; set; } = null!;
        public DbSet<Node> Nodes { get; set; } = null!;
        public DbSet<Edge> Edges { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //One-to-Many between Map and Nodes
            modelBuilder.Entity<Map>()
                .HasMany(m => m.Nodes)
                .WithOne(n => n.Map)
                .HasForeignKey(n => n.MapId);

            //One-to-Many between Node and source Edges
            modelBuilder.Entity<Node>()
                .HasMany(e => e.EdgesTo)
                .WithOne(n => n.DestinationNode)
                .OnDelete(DeleteBehavior.Restrict);

            //One-to-Many between Node and destination Edges
            modelBuilder.Entity<Node>()
                .HasMany(e => e.EdgesFrom)
                .WithOne(n => n.SourceNode)
                .HasForeignKey(e => e.SourceNodeId)
                .OnDelete(DeleteBehavior.Restrict);

                


            
        }
    }
}
