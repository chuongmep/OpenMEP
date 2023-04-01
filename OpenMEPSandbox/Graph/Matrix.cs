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
    /// <returns>An adjacency matrix for the graph.</returns>
    public static int[][] BuildGraphAdjMatrix(List<Point> sources, List<Point> destinations)
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
    /// <returns>An adjacency matrix representing the graph.</returns>
    public static int[][] BuildGraphAdjMatrix(List<int> sources, List<int> destinations, List<int> weights)
    {
        int numVertices = sources.Max() + 1; // determine the number of vertices based on the highest vertex ID
        int[][] adjMatrix = new int[numVertices][];
        for (int i = 0; i < numVertices; i++)
        {
            adjMatrix[i] = new int[numVertices];
            for (int j = 0; j < numVertices; j++)
            {
                adjMatrix[i][j] = -1; // initialize all edge weights to -1 (indicating no edge)
            }
        }
        for (int k = 0; k < sources.Count; k++)
        {
            int i = sources[k];
            int j = destinations[k];
            int weight = weights[k];
            adjMatrix[i][j] = weight;
            adjMatrix[j][i] = weight; // assuming an undirected graph
        }
        return adjMatrix;
    }
}