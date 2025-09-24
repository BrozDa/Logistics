using MapService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MapService.Infrastructure.Persistence
{
    public class MapServiceContext : DbContext
    {
        public DbSet<Map> Maps { get; set; } = null!;
        public DbSet<Node> Nodes { get; set; } = null!;
        public DbSet<Edge> Edges { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Map>()
                .HasMany<Node>()
                .WithOne()
                .HasForeignKey(n => n.MapId);

            modelBuilder.Entity<Node>()
                .HasMany<Edge>(e => e.EdgesTo)
                .WithOne(n => n.DestinationNode)
                .HasForeignKey(e => e.DestinationNodeId);

            modelBuilder.Entity<Node>()
                .HasMany<Edge>(e => e.EdgesFrom)
                .WithOne(n => n.SourceNode)
                .HasForeignKey(e => e.SourceNodeId);
        }
    }
}
