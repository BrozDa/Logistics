using MapService.Application.SeedService.SeedModels;
using MapService.Domain.Models;
using MapService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace MapService.Application.SeedService
{
    /// <summary>
    /// Performs seeding of the database with initial data
    /// </summary>
    /// <param name="logger">Instanse of logger to log any encountered errors</param>
    /// <param name="dbContext">Instance of EF DB context used for persistence</param>
    public class SeedService(
        ILogger<SeedService> logger,
        MapServiceContext dbContext
        )
    {
        private ILogger<SeedService> _logger = logger;
        private MapServiceContext _dbContext = dbContext;

        /// <summary>
        /// Deletes existing database, performs migration and seeds database with initial data contained in the passed file
        /// </summary>
        /// <param name="seedDataFile">A path to file containing seeding data in JSON format</param>
        /// <returns>True, if seeding was successful; false otherwise - any encountered error is logged</returns>
        public async Task<bool> SeedInitialDataAsync(string seedDataFile)
        {

            var seedData = await LoadSeedDataAsync(seedDataFile);

            if (seedData is null) { return false; }

            return await InsertSeedDataAsync(seedData);

        }
        /// <summary>
        /// Deletes existing database and performs migration
        /// </summary>
        private async Task MigrateDatabaseAsync()
        {
            await _dbContext.Database.EnsureDeletedAsync();
            await _dbContext.Database.MigrateAsync();
        }
        /// <summary>
        /// Loads initial data from passed file to SeedData model
        /// </summary>
        /// <param name="seedDataFile"></param>
        /// <returns>An instance of <see cref="SeedData"/> contaning deserialized data; null in case of any errors</returns>
        private async Task<SeedData?> LoadSeedDataAsync(string seedDataFile)
        {
            try
            {
                if(string.IsNullOrEmpty(seedDataFile)) {

                    _logger.LogWarning($"[SeedService][LoadSeedData] Empty path passed");
                    return null; 
                }
                if (!File.Exists(seedDataFile))
                {
                    _logger.LogWarning("[SeedService][LoadSeedData] File {file} does not exist", seedDataFile);
                    return null;
                }

                using StreamReader reader = new StreamReader(seedDataFile);
                var rawData = await reader.ReadToEndAsync();

                Console.WriteLine("Raw data: " + rawData);
                if (string.IsNullOrEmpty(rawData))
                {
                    _logger.LogWarning("[SeedService][LoadSeedData] Unable to read file {file}", seedDataFile);
                    return null;
                }

                var opts = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var seedData = JsonSerializer.Deserialize<SeedData>(rawData, opts);

                if (seedData is null)
                {
                    _logger.LogWarning($"[SeedService][LoadSeedData] Deserialization of seed data failed");
                    return null;
                }

                Console.WriteLine(seedData.Nodes.Count);
                Console.WriteLine(seedData.Edges.Count);
                Console.WriteLine();
                Console.WriteLine();
                return seedData;
            }
            catch (ArgumentNullException ex) 
            {
                _logger.LogError($"[SeedService][LoadSeedData] Null reference passed to JSON deserializer: {ex.Message}");
                return null;
            }
            catch(JsonException ex)
            {
                _logger.LogError($"[SeedService][LoadSeedData] Invalid JSON passed: {ex.Message}");
                return null;
            }
            catch(Exception ex)
            {
                _logger.LogError($"[SeedService][LoadSeedData] Unhandled exception occured: {ex.Message}");
                return null;
            }
            
        }

        /// <summary>
        /// Inserts loaded data to the database
        /// </summary>
        /// <param name="seedData">Deserialized seeding data</param>
        /// <returns>True, if insert was successful; false otherwise - any encountered error is logged</returns>
        /// <remarks>All of nothing approach - if there is any error then any partial commit is rolled back</remarks>
        private async Task<bool> InsertSeedDataAsync(SeedData seedData) 
        {
            await using var efTransaction = await _dbContext.Database.BeginTransactionAsync();

            var map = new Map() { Name = "Default Map" };
            await _dbContext.Maps.AddAsync(map);

            var nodeMap = await SeedNodes(seedData.Nodes, map);
            if(nodeMap is null) { return false; }
            
            var seedEdgesResult = await SeedEdges(seedData.Edges, nodeMap);

            if(seedEdgesResult != true) { return false; }
            
            await efTransaction.CommitAsync();

            return true;

        }
        /// <summary>
        /// Inserts initial nodes to the database
        /// </summary>
        /// <param name="seedNodes">A List of <see cref="SeedNode"/> to be inserted</param>
        /// <param name="map">An instance of <see cref="Map"/> to which nodes are associated to</param>
        /// <returns>A dictionary <string, Node> (node name, node instance) of inserted data; null in case of any errors</returns>
        private async Task<Dictionary<string, Node>?> SeedNodes(List<SeedNode> seedNodes, Map map)
        {
            if(seedNodes is null)
            {
                _logger.LogWarning($"[SeedService][SeedNodes] Nodes empty");
                return null;
            }

            Dictionary<string, Node> nodeMap = new();

            foreach (var seedNode in seedNodes) 
            {
                if (nodeMap.ContainsKey(seedNode.Name))
                {
                    _logger.LogWarning($"[SeedService][SeedNodes] Duplicate Node name found {seedNode.Name}, skipping node");
                    continue;
                }

                var node = new Node() { Name = seedNode.Name, MapId = map.Id };
                nodeMap.Add(seedNode.Name, node);

            }

            if (nodeMap.Any()) 
            { 
                await _dbContext.Nodes.AddRangeAsync(nodeMap.Values);
                await _dbContext.SaveChangesAsync();
            }

            return nodeMap;
        }
        /// <summary>
        /// Inserts initial edges to the database
        /// </summary>
        /// <param name="seedEdges">List of <see cref="SeedEdge"/> to be inserted</param>
        /// <param name="nodeMap">A dictionary <string, Node> (node name, node instance) used to define relationships between edges and nodes</param>
        /// <returns>True if the edges were inserted successfully, false otherwise</returns>
        private async Task<bool> SeedEdges(List<SeedEdge> seedEdges, Dictionary<string, Node> nodeMap)
        {
            if (seedEdges is null || !seedEdges.Any())
            {
                _logger.LogWarning("[SeedService][SeedEdges] Edges empty");
                return false;
            }

            string source;
            string destination;
            List<Edge> edges = new();

            foreach (var seedEdge in seedEdges)
            {
                source = seedEdge.Source;
                destination = seedEdge.Destination;

                if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(destination))
                {
                    _logger.LogWarning($"[SeedService][InsertSeedData] Empty edge source or destination, skipped");
                    continue;
                }

                if (!nodeMap.TryGetValue(seedEdge.Source, out var srcNode) || !nodeMap.TryGetValue(seedEdge.Destination, out var destNode))
                {
                    _logger.LogWarning($"[SeedService][InsertSeedData] Edge refers to invalid Node, skipping");
                    continue;
                }

                edges.Add(new Edge()
                {
                    Weight = seedEdge.Weight > 0 ? seedEdge.Weight : 1,
                    SourceNodeId = srcNode.Id,
                    DestinationNodeId = destNode.Id,
                });
            }

            if (edges.Any())
            {
                await _dbContext.AddRangeAsync(edges);
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }
    }
}
