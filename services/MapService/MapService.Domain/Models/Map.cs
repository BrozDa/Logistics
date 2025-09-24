namespace MapService.Domain.Models
{
    public class Map
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Node> Nodes { get; set; } = null!;

    }
}
