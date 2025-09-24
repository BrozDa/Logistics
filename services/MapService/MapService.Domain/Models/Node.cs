namespace MapService.Domain.Models
{
    public class Node
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Edge> EdgesFrom { get; set; } = null!;
        public IEnumerable<Edge> EdgesTo { get; set; } = null!;
        public Guid MapId { get; set; }
        public Map Map { get; set; } = null!;

    }
}
