using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Algo;

[IsVisibleInDynamoLibrary(false)]
public static class DijkstraAlgorithm
{
    /// <summary>
    /// Finds the shortest path from the source vertex to the destination vertex in a graph represented by an adjacency matrix using Dijkstra's algorithm.
    /// </summary>
    /// <param name="adjMatrix">The adjacency matrix representation of the graph.</param>
    /// <param name="source">The source vertex ID.</param>
    /// <param name="destination">The destination vertex ID.</param>
    /// <returns>A tuple containing the shortest path from the source vertex to the destination vertex and its total weight.</returns>
    public static (List<int> path, int weight) FindShortestPath(int[][] adjMatrix, int source, int destination)
    {
        int numVertices = adjMatrix.Length;

        int[] distances = new int[numVertices];
        for (int i = 0; i < numVertices; i++)
        {
            distances[i] = int.MaxValue;
        }

        distances[source] = 0;

        bool[] visited = new bool[numVertices];

        int[] previous = new int[numVertices];
        for (int i = 0; i < numVertices; i++)
        {
            previous[i] = -1;
        }

        for (int i = 0; i < numVertices - 1; i++)
        {
            int current = GetMinDistanceVertex(distances, visited);
            visited[current] = true;

            for (int j = 0; j < numVertices; j++)
            {
                int edgeDistance = adjMatrix[current][j];

                if (edgeDistance > 0 && (distances[current] + edgeDistance < distances[j]))
                {
                    distances[j] = distances[current] + edgeDistance;
                    previous[j] = current;
                }
            }
        }

        List<int> path = new List<int>();
        int dest = destination;
        while (previous[dest] != -1)
        {
            path.Add(dest);
            dest = previous[dest];
        }
        path.Add(source);
        path.Reverse();

        return (path, distances[destination]);
    }
    /// <summary>
    /// Builds an adjacency matrix for a graph given the source vertices, destination vertices, and edge weights.
    /// </summary>
    /// <param name="sources">The list of source vertices for each edge.</param>
    /// <param name="destinations">The list of destination vertices for each edge.</param>
    /// <param name="weights">The list of edge weights.</param>
    /// <returns>An adjacency matrix representation of the graph.</returns>
    public static int[][] BuildGraphAdjMatrix(List<int> sources, List<int> destinations, List<int> weights)
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
    /// <summary>
    /// Finds the vertex with the minimum distance in the distance array that has not been visited.
    /// </summary>
    /// <param name="distances">The array of distances from the source vertex to each vertex in the graph.</param>
    /// <param name="visited">The array indicating whether each vertex in the graph has been visited.</param>
    /// <returns>The vertex ID with the minimum distance that has not been visited.</returns>
    private static int GetMinDistanceVertex(int[] distances, bool[] visited)
    {
        int minDistance = int.MaxValue;
        int minVertex = -1;

        for (int i = 0; i < distances.Length; i++)
        {
            if (!visited[i] && distances[i] < minDistance)
            {
                minDistance = distances[i];
                minVertex = i;
            }
        }

        return minVertex;
    }
}