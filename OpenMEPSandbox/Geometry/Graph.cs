using OpenMEPSandbox.Algo;

namespace OpenMEPSandbox.Geometry;
using Autodesk.DesignScript.Geometry;
public class Graph
{
    private Graph()
    {
        
    }
    /// <summary>
    /// Adds an edge between two vertices in the graph, with a specified weight.
    /// </summary>
    /// <param name="sources">The starting vertex of the edge.</param>
    /// <param name="destinations">The ending vertex of the edge.</param>
    /// <param name="weight">The weight of the edge.</param>
    /// <returns name="DijkstraGraph">DijkstraGraph</returns>
    public static Algo.DijkstraGraph AddEdge(
        List<Autodesk.DesignScript.Geometry.Point> sources,
        List<Autodesk.DesignScript.Geometry.Point> destinations,List<double> weight)
    {
        if (sources.Count != weight.Count)
            throw new ArgumentNullException($"The number of points in u and weight must be equal to can crate a edge.");
        if(sources.Count != destinations.Count)
        {
            throw new Exception($"The number of points in u and v must be equal to can crate a edge.");
        }
        DijkstraGraph graph = new Algo.DijkstraGraph();
        for(int i = 0; i < sources.Count; i++)
        {
            graph.AddEdge(sources[i], destinations[i],weight[i]);
        }
        return graph;
    }
    
    /// <summary>
    /// Shortest path from start to end by Dijkstra algorithm.
    /// </summary>
    /// <param name="dijkstraGraph">graph init </param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns anme="points">the shortest path</returns>
    public static List<Autodesk.DesignScript.Geometry.Point>? ShortestPath(
        Algo.DijkstraGraph dijkstraGraph,
        Autodesk.DesignScript.Geometry.Point start,
        Autodesk.DesignScript.Geometry.Point end)
    {
        return dijkstraGraph.DijkstraShortestPath(start, end);
    }
}