namespace OpenMEPSandbox.Geometry;
using Autodesk.DesignScript.Geometry;
public class Graph
{
    private Graph()
    {
        
    }
    
    /// <summary>
    /// Add list of edges to the graph.
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    /// <exception cref="Exception"></exception>
    public static Algo.DijkstraGraph AddEdge(
        List<Autodesk.DesignScript.Geometry.Point> u,
        List<Autodesk.DesignScript.Geometry.Point> v)
    {
        Algo.DijkstraGraph dijkstraGraph = new Algo.DijkstraGraph();
        if(u.Count != v.Count)
        {
            throw new Exception("The number of points in u and v must be equal to can crate a edge.");
        }
        for(int i = 0; i < u.Count; i++)
        {
            dijkstraGraph.AddEdge(u[i], v[i]);
        }
        return dijkstraGraph;
    }
    
    /// <summary>
    /// Shortest path from start to end by Dijkstra algorithm.
    /// </summary>
    /// <param name="dijkstraGraph">graph init </param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static List<Autodesk.DesignScript.Geometry.Point> ShortestPath(
        Algo.DijkstraGraph dijkstraGraph,
        Autodesk.DesignScript.Geometry.Point start,
        Autodesk.DesignScript.Geometry.Point end)
    {
        return dijkstraGraph.ShortestPath(start, end);
    }
}