using Autodesk.DesignScript.Runtime;
using OpenMEPSandbox.Algo;

namespace OpenMEPSandbox.Geometry;

public class Graph
{
    private Graph()
    {
        
    }

    /// <summary>
    /// Creates a new `BellmanFord` instance with edges defined by the given source, target, and weight lists, and returns the instance.
    /// The `v` and `e` parameters specify the number of vertices and edges in the graph, respectively.
    /// The `source`, `target`, and `weight` lists define the source, destination, and weight of each edge, respectively.
    /// </summary>
    /// <param name="sources">A list of source vertices for each edge.</param>
    /// <param name="destinations">A list of destinations vertices for each edge.</param>
    /// <param name="weights">A list of weights for each edge.</param>
    /// <returns>A new `BellmanFord` instance with the specified edges.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Graph.AddEdge.png)
    /// </example>
    public static BellmanFord AddEdge(List<double> sources, List<double> destinations, List<double> weights)
    {
        //Get unique number in sources
        var uniqueSources = sources.Distinct();
        //Get unique number in targets
        var uniqueTargets = destinations.Distinct();
        //Get unique number in sources and targets
        var vertex = uniqueSources.Union(uniqueTargets).Count();
        var edge = sources.Count;
        BellmanFord bf = new BellmanFord(vertex, edge);
        //greatest common divisor of sources and targets
        var gcd = GCD(sources, destinations);
        // new sources and targets
        for (int i = 0; i < edge; i++)
        {
            sources[i] = sources[i] / gcd;
            destinations[i] = destinations[i] / gcd;
        }
        // add edge
        for (int i = 0; i < edge; i++)
        {
            bf.AddEdge((int) sources[i], (int) destinations[i], (int) weights[i]);
        }
        return bf;
    }
    private static double GCD(List<double> sources, List<double> destinations)
    {
        // greatest common divisor of sources and targets
        double gcd = 1;
        for (int i = 0; i < sources.Count; i++)
        {
            gcd = GCD(sources[i], destinations[i]);
        }
        return gcd;
    }
    private static double GCD(double a, double b)
    {
        // greatest common divisor of sources and targets
        if (a == 0)
            return b;
        return GCD(b % a, a);
    }

    /// <summary>
    /// Get shortest path and distance from start node to end node By Bellman-Ford algorithm
    /// </summary>
    /// <param name="bellmanFord">a class define include edge,vertex added</param>
    /// <param name="startNode">first location of node</param>
    /// <param name="endNode">end location of node</param>
    /// <returns name="shortestPath">the shortest path</returns>
    /// <returns name="distance">the value distance shortest of path</returns>
    /// <exception cref="Exception"></exception>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Graph.GetShortestPath.png)
    /// </example>
    [MultiReturn("shortestPath", "distance")]
    public static Dictionary<string,object> GetShortestPath(BellmanFord bellmanFord, double startNode, double endNode)
    {
       
        // check start node and end node is integer
        if (System.Math.Abs(startNode - System.Math.Floor(startNode)) > 0.001 || System.Math.Abs(endNode - System.Math.Floor(endNode)) > 0.001)
        {
            throw new Exception("Start node and end node must be integer");
        }
        int start = (int) startNode;
        int end = (int) endNode;
        (List<int>, int) shortestPath = bellmanFord.GetShortestPathAndDistance(start, end);
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
    /// ![](../OpenMEPPage/geometry/dyn/pic/Graph.GetDistancesPathFromNode.png)
    /// </example>
    [MultiReturn("path", "distance")]
    public static Dictionary<string,object> GetDistancesPathFromNode(double from,List<double> sources, List<double> destinations, List<double> weights)
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
        int fromInt = (int) from;
        Dictionary<int, Tuple<List<int>, int>> shortestPaths = bf.GetShortestPathsAndDistances(fromInt);
        List<int> vertexes = new List<int>();
        List<List<int>> paths = new List<List<int>>();
        List<int> distances = new List<int>();
        foreach (KeyValuePair<int, Tuple<List<int>, int>> entry in shortestPaths)
        {
            int vertex = entry.Key;
            List<int> path = entry.Value.Item1;
            int distance = entry.Value.Item2;
            // Console.WriteLine("Shortest path from vertex 0 to vertex " + vertex + ": " + string.Join(" -> ", path));
            // Console.WriteLine("Distance: " + distance);
            // Console.WriteLine();
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