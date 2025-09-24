namespace MapService.Application.SeedService.SeedModels
{
    internal class SeedEdge
    {
        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public float Weight { get; set; } = 1;
    }
}
