using Autodesk.DesignScript.Geometry;

namespace OpenMEPSandbox.Graph;

public class Matrix
{
    private Matrix()
    {
        
    }
    /// <summary>
    /// Builds an adjacency matrix for an undirected graph based on a list of source points and destination points.
    /// The weight of each edge is the Euclidean distance between the source and destination points, rounded to the nearest integer.
    /// </summary>
    /// <param name="sources">The list of source points for the edges.</param>
    /// <param name="destinations">The list of destination points for the edges.</param>
    /// <returns name="adjacency matrix">An adjacency matrix for the graph.</returns>
    /// <example>
    /// ![](../OpenMEPPage/graph/pic/Matrix.BuildAdjMatrixByPoints.png)
    /// </example>
    public static int[][] BuildAdjMatrixByPoints(List<Point> sources, List<Point> destinations)
    {
        int n = sources.Count;
        int[][] adjMatrix = new int[n][];

        for (int i = 0; i < n; i++)
        {
            adjMatrix[i] = new int[n];
        }

        for (int i = 0; i < n; i++)
        {
            for (int j = i; j < n; j++)
            {
                double distance = sources[i].DistanceTo(destinations[j]);
                adjMatrix[i][j] = (int)System.Math.Round(distance);
                adjMatrix[j][i] = (int)System.Math.Round(distance);
            }
        }
        return adjMatrix;
    }
    /// <summary>
    /// Builds an adjacency matrix for a graph given its source vertices, destination vertices, and edge weights.
    /// </summary>
    /// <param name="sources">A list of source vertices.</param>
    /// <param name="destinations">A list of destination vertices.</param>
    /// <param name="weights">A list of edge weights.</param>
    /// <returns name="adjacency matrix">An adjacency matrix representing the graph.</returns>
    /// <example>
    /// ![](../OpenMEPPage/graph/pic/Matrix.BuildAdjMatrixByGraph.png)
    /// </example>
    public static int[][] BuildAdjMatrixByGraph(List<int> sources, List<int> destinations, List<int> weights)
    {
        // Determine the maximum vertex ID in the graph
        int maxVertexId = System.Math.Max(sources.Max(), destinations.Max());

        // Initialize the adjacency matrix as a jagged array of size (maxVertexId + 1) x (maxVertexId + 1)
        int[][] adjMatrix = new int[maxVertexId + 1][];

        for (int i = 0; i <= maxVertexId; i++)
        {
            adjMatrix[i] = new int[maxVertexId + 1];
        }
        // Add each edge to the adjacency matrix by setting the corresponding element to the edge weight
        for (int i = 0; i < sources.Count; i++)
        {
            int src = sources[i];
            int dest = destinations[i];
            int weight = weights[i];

            adjMatrix[src][dest] = weight;

            // Uncomment the following line if the graph is undirected:
            // adjMatrix[dest][src] = weight;
        }

        return adjMatrix;
    }
}