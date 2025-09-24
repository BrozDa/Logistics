namespace MapService.Domain.Models
{
    public class Edge
    {
        public Guid Id { get; set; }
        public double Weight { get; set; }

        public Guid SourceNodeId { get; set; }
        public Node SourceNode { get; set; } = null!;

        public Guid DestinationNodeId { get; set; }
        public Node DestinationNode { get; set; } = null!;
    }
}
