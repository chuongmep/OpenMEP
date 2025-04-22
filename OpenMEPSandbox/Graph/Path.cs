using Autodesk.DesignScript.Runtime;
using OpenMEPSandbox.Algo;

namespace OpenMEPSandbox.Graph;

public class Path
{
    private Path()
    {
    }
    
    /// <summary>
    /// Finds the shortest path between two nodes in a graph using Dijkstra's algorithm.
    /// The graph is represented as a list of edges, where each edge is a triple (source, destination, weight).
    /// The startNode and endNode parameters specify the source and destination nodes, respectively.
    /// The sources, destinations, and weights parameters contain the lists of source nodes, destination nodes,
    /// and edge weights, respectively, that make up the graph. Returns an array of node IDs that form the shortest
    /// path from the start node to the end node. If no path exists, returns an empty array.
    /// </summary>
    /// <param name="startNode">The ID of the start node.</param>
    /// <param name="endNode">The ID of the end node.</param>
    /// <param name="sources">The list of source nodes for each edge in the graph.</param>
    /// <param name="destinations">The list of destination nodes for each edge in the graph.</param>
    /// <param name="weights">The list of edge weights for each edge in the graph.</param>
    /// <returns name="shortestPath">the shortest path</returns>
    /// <returns name="distance">the value distance shortest of path</returns>
    /// <example>
    /// ![](../OpenMEPPage/graph/pic/Path.FindShortestPathDijkstra.png)
    /// [Path.FindShortestPathDijkstra.dyn](../OpenMEPPage/graph/Path.FindShortestPathDijkstra.dyn)
    /// </example>
    [MultiReturn("shortestPath", "distance")]
    public static Dictionary<string, object> FindShortestPathDijkstra(int startNode, int endNode,
        List<int> sources, List<int> destinations, List<int> weights)
    {
        // check case weight < 1
        for (int i = 0; i < weights.Count; i++)
        {
            if (weights[i] < 0)
            {
                throw new Exception(
                    "Weight is negative,Dijkstra does not support negative weight, please use Bellman-Ford algorithm");
            }
        }
        int[][] graphAdjMatrix = DijkstraAlgorithm.BuildGraphAdjMatrix(sources, destinations, weights);
        (List<int> path, int weight) shortestPath =
            DijkstraAlgorithm.FindShortestPath(graphAdjMatrix, (int) startNode, (int) endNode);
        return new Dictionary<string, object>()
        {
            {"shortestPath", shortestPath.path},
            {
                "distance", shortestPath.weight
            }
        };
    }

    /// <summary>
    /// Get shortest path and distance from start node to end node By Bellman-Ford algorithm
    /// </summary>
    /// <param name="sources">A list of source vertices for each edge.</param>
    /// <param name="destinations">A list of destinations vertices for each edge.</param>
    /// <param name="weights">A list of weights for each edge.</param>
    /// <param name="startNode">first location of node</param>
    /// <param name="endNode">end location of node</param>
    /// <returns name="shortestPath">the shortest path</returns>
    /// <returns name="distance">the value distance shortest of path</returns>
    /// <exception cref="Exception"></exception>
    /// <example>
    /// ![](../OpenMEPPage/graph/pic/Path.FindShortestPathBellman.png)
    /// [Path.FindShortestPathBellman.dyn](../OpenMEPPage/graph/Path.FindShortestPathBellman.dyn)
    /// </example>
    [MultiReturn("shortestPath", "distance")]
    public static Dictionary<string, object> FindShortestPathBellman(int startNode, int endNode,
        List<int> sources, List<int> destinations, List<int> weights)
    {
        //Get unique number in sources
        var uniqueSources = sources.Distinct();
        //Get unique number in targets
        var uniqueTargets = destinations.Distinct();
        //Get unique number in sources and targets
        var vertex = uniqueSources.Union(uniqueTargets).Count();
        var edge = sources.Count;
        BellmanFord bf = new BellmanFord(vertex, edge);
        // add edge
        for (int i = 0; i < edge; i++)
        {
            bf.AddEdge(sources[i], destinations[i], weights[i]);
        }
        (List<int>, int) shortestPath = bf.GetShortestPathAndDistance(startNode, endNode);
        return new Dictionary<string, object>
        {
            {"shortestPath", shortestPath.Item1},
            {"distance", shortestPath.Item2}
        };
    }

    /// <summary>
    /// Finds the shortest path from each source vertex to every other vertex in the graph using the Bellman-Ford algorithm,
    /// and returns an array of tuples containing the distance and path to each vertex.
    /// The `v` and `e` parameters specify the number of vertices and edges in the graph, respectively.
    /// The `sources`, `targets`, and `weights` lists define the source, destination, and weight of each edge, respectively.
    /// </summary>
    /// <param name="from">value of node want start check</param>
    /// <param name="sources">A list of source vertices for each edge.</param>
    /// <param name="destinations">A list of target vertices for each edge.</param>
    /// <param name="weights">A list of weights for each edge.</param>
    /// <returns>
    /// An array of tuples, where each tuple contains the distance and path from a source vertex to every other vertex in the graph.
    /// If there is no path from a source to a destination vertex, or if there is a negative weight cycle in the graph,
    /// the distance for that tuple will be `double.MaxValue` and the path will be an empty array.
    /// </returns>
    /// <example>
    /// ![](../OpenMEPPage/graph/pic/Path.GetDistancesPathFromNode.png)
    /// [Path.GetDistancesPathFromNode.dyn](../OpenMEPPage/graph/pic/Path.GetDistancesPathFromNode.dyn)
    /// </example>
    [MultiReturn("path", "distance")]
    public static Dictionary<string, object> GetDistancesPathFromNode(int from, List<int> sources,
        List<int> destinations, List<int> weights)
    {
        //Get unique number in sources
        var uniqueSources = sources.Distinct();
        //Get unique number in destinations
        var uniqueTargets = destinations.Distinct();
        //Get unique number in sources and destinations to get vertex
        var v = uniqueSources.Union(uniqueTargets).Count();
        //Get number of edge
        var e = sources.Count;
        BellmanFordDistance bf = new BellmanFordDistance(v, e);
        // add edge
        for (int i = 0; i < e; i++)
        {
            bf.AddEdge((int) sources[i], (int) destinations[i], (int) weights[i]);
        }
        Dictionary<int, Tuple<List<int>, int>> shortestPaths = bf.GetShortestPathsAndDistances(from);
        List<int> vertexes = new List<int>();
        List<List<int>> paths = new List<List<int>>();
        List<int> distances = new List<int>();
        foreach (KeyValuePair<int, Tuple<List<int>, int>> entry in shortestPaths)
        {
            int vertex = entry.Key;
            List<int> path = entry.Value.Item1;
            int distance = entry.Value.Item2;
            vertexes.Add(vertex);
            paths.Add(path);
            distances.Add(distance);
        }

        return new Dictionary<string, object>()
        {
            {"vertex", vertexes},
            {"path", paths},
            {"distance", distances}
        };
    }
}