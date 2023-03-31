using Autodesk.DesignScript.Runtime;
using Point = Autodesk.DesignScript.Geometry.Point;

namespace OpenMEPSandbox.Algo;

[IsVisibleInDynamoLibrary(false)]
public class DijkstraGraph
{
    
    Dictionary<Autodesk.DesignScript.Geometry.Point, Dictionary<Autodesk.DesignScript.Geometry.Point, double>>
        graph { get; set; }
    public DijkstraGraph()
    {
        
       graph = new Dictionary<Autodesk.DesignScript.Geometry.Point,
            Dictionary<Autodesk.DesignScript.Geometry.Point, double>>();
    }

    /// <summary>
    /// Adds an edge between two vertices in the graph, with a specified weight.
    /// </summary>
    /// <param name="source">The starting vertex of the edge.</param>
    /// <param name="destination">The ending vertex of the edge.</param>
    /// <param name="weight">The weight of the edge.</param>
    public void AddEdge(Autodesk.DesignScript.Geometry.Point source, Autodesk.DesignScript.Geometry.Point destination,
        double weight)
    {
        
        if (!graph.ContainsKey(source))
            graph[source] = new Dictionary<Point, double>();

        graph[source][destination] = weight;
    }

    public List<Point>? DijkstraShortestPath(Point source, Point destination)
    {
        var distances = new Dictionary<Point, double>();
        var visited = new HashSet<Point>();
        var previous = new Dictionary<Point, Point>();
        var nodes = new List<Point>();

        foreach (var vertex in graph)
        {
            if (vertex.Key == source)
                distances[vertex.Key] = 0;
            else
                distances[vertex.Key] = double.MaxValue;

            nodes.Add(vertex.Key);
        }

        nodes.Sort((x, y) => distances[x].CompareTo(distances[y]));

        while (nodes.Count > 0)
        {
            var smallest = nodes[0];
            nodes.Remove(smallest);

            if (smallest == destination)
            {
                var path = new List<Point>();
                while (previous.ContainsKey(smallest))
                {
                    path.Add(smallest);
                    smallest = previous[smallest];
                }

                path.Add(source);
                path.Reverse();
                return path;
            }
            if (Math.Abs(distances[smallest] - double.MaxValue) < 0.00001)
                break;
            foreach (var neighbor in graph[smallest])
            {
                var alt = distances[smallest] + neighbor.Value;
                if (alt < distances[neighbor.Key])
                {
                    distances[neighbor.Key] = alt;
                    previous[neighbor.Key] = smallest;
                    nodes.Sort((x, y) => distances[x].CompareTo(distances[y]));
                }
            }
        }
        return null;
    }
}
