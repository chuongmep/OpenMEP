using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Graph;

public class GraphVisualizer
{
    private GraphVisualizer()
    {
    }

    /// <summary>
    /// Visualize the graph with edge
    /// </summary>
    /// <param name="line">edge created by two node</param>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/graph/pic/GraphVisualizer.VisualizeByEdge.png)
    /// </example>
    public static Modifiers.GeometryColor VisualizeByEdge(Line line)
    {
        var redColor = DSCore.Color.ByARGB(255, 255, 0, 0);
        return Modifiers.GeometryColor.ByGeometryColor(line, redColor);
    }

    /// <summary>
    /// Visualize the graph with source and destination points
    /// </summary>
    /// <param name="source">the first node of edge</param>
    /// <param name="destination">the second node of edge</param>
    /// <returns name="geometryColor"></returns>
    /// <example>
    /// ![](../OpenMEPPage/graph/pic/GraphVisualizer.VisualizeByDestination.png)
    /// </example>
    public static Modifiers.GeometryColor VisualizeByDestination(Point source, Point destination)
    {
        Line line = Line.ByStartPointEndPoint(source, destination);
        var redColor = DSCore.Color.ByARGB(255, 255, 0, 0);
        Modifiers.GeometryColor geometryColor = Modifiers.GeometryColor.ByGeometryColor(line, redColor);
        return geometryColor;
    }

    /// <summary>
    /// Visualizes a graph with the given vertex connections and edge weights using 3D points. Determines the maximum weight 
    /// and creates a 3D point for each vertex, then calculates the coordinates of each point based on their position along a 
    /// circle with radius proportional to their weight. Returns a list of the generated points.
    /// </summary>
    /// <param name="sources">A list of integers representing the starting vertices for each edge.</param>
    /// <param name="destinations">A list of integers representing the ending vertices for each edge.</param>
    /// <param name="weights">A list of doubles representing the weight of each edge.</param>
    /// <returns>A list of Point3D objects representing the vertices of the graph.</returns>
    /// <example>
    /// ![](../OpenMEPPage/graph/pic/GraphVisualizer.VisualizeByGraph.gif)
    /// </example>
    [MultiReturn("Node", "Point", "Display")]
    public static Dictionary<string,object> VisualizeByGraph(List<int> sources, List<int> destinations,
        List<double> weights)
    {
        // Create a dictionary to store the points for each vertex
        Dictionary<int, Vertex> points = new Dictionary<int, Vertex>();
        List<Modifiers.GeometryColor> display = new List<Modifiers.GeometryColor>();
        // Determine the maximum weight
        double maxWeight = double.MinValue;
        foreach (double weight in weights)
        {
            if (weight > maxWeight)
            {
                maxWeight = weight;
            }
        }

        // Create a 3D point for each vertex
        Random random = new Random();
        for (int i = 0; i < sources.Count; i++)
        {
            int source = sources[i];
            int destination = destinations[i];
            if (!points.ContainsKey(source))
            {
                Vertex vertex = new Vertex(Point.ByCoordinates(random.NextDouble() * maxWeight,
                    random.NextDouble() * maxWeight,
                    random.NextDouble() * maxWeight), source.ToString());
                points.Add(source, vertex);
            }

            if (!points.ContainsKey(destination))
            {
                Vertex vertex = new Vertex(Point.ByCoordinates(random.NextDouble() * maxWeight,
                    random.NextDouble() * maxWeight,
                    random.NextDouble() * maxWeight), destination.ToString());
                points.Add(destination, vertex);
            }
        }

        for (int i = 0; i < sources.Count; i++)
        {
            int source = sources[i];
            int destination = destinations[i];
            Vertex startVerText = points[source];
            Vertex endVerText = points[destination];
            display.Add(VisualizeByDestination(startVerText.Point, endVerText.Point));
        }
        return new Dictionary<string, object>()
        {
            {"Node", points.Values.Select(x=>x.Label).ToList()},
            {"Point", points.Values.Select(x=>x.Point).ToList()},
            {"Display", display}
        };
    }
   
    
}

[IsVisibleInDynamoLibrary(false)]
public class Vertex
{
    public Point Point { get; set; }
    public string Label { get; set; }

    public Vertex(Point point, string label)
    {
        Point = point;
        Label = label;
    }
}