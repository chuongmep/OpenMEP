using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Algo;

[IsVisibleInDynamoLibrary(false)]
public class BellmanFord
{
    private readonly int _vertices;
    private readonly int _edges;
    private readonly List<(int, int, int)> _edgeList;

    public BellmanFord(int vertices, int edges)
    {
        _vertices = vertices;
        _edges = edges;
        _edgeList = new List<(int, int, int)>();
    }

    /// <summary>
    /// Adds a directed edge from the given source vertex to the given destination vertex with the given weight.
    /// </summary>
    /// <param name="source">The source vertex of the edge.</param>
    /// <param name="destination">The destination vertex of the edge.</param>
    /// <param name="weight">The weight of the edge.</param>
    public void AddEdge(int source, int destination, int weight)
    {
        _edgeList.Add((source, destination, weight));
    }

    /// <summary>
    /// Finds the shortest path from the given source vertex to the given destination vertex using the Bellman-Ford algorithm,
    /// and returns an array of the vertices along the path in the order they are visited, as well as the total distance of the path.
    /// If there is no path from the source to the destination, or if there is a negative weight cycle in the graph, returns null.
    /// </summary>
    /// <param name="source">The source vertex for the shortest path.</param>
    /// <param name="destination">The destination vertex for the shortest path.</param>
    /// <returns>An array of the vertices along the shortest path, in order, and the total distance of the path; or null if no such path exists.</returns>
    public (List<int>, int) GetShortestPathAndDistance(int source, int destination)
    {
        int[] distance = new int[_vertices];
        int[] predecessor = new int[_vertices];

        for (int i = 0; i < _vertices; i++)
        {
            distance[i] = int.MaxValue;
            predecessor[i] = -1;
        }

        distance[source] = 0;

        for (int i = 1; i < _vertices; i++)
        {
            foreach ((int u, int v, int w) in _edgeList)
            {
                if (distance[u] != int.MaxValue && distance[u] + w < distance[v])
                {
                    distance[v] = distance[u] + w;
                    predecessor[v] = u;
                }
            }
        }

        List<int> path = new List<int>();
        int current = destination;
        while (current != source)
        {
            path.Add(current);
            current = predecessor[current];
        }

        path.Add(source);
        path.Reverse();

        return (path, distance[destination]);
    }
}

[IsVisibleInDynamoLibrary(false)]
public class BellmanFordDistance
{
    private int V; // number of vertices
    private int E; // number of edges
    private List<Edge> edges; // list of edges
    private int[] distances; // array to store the distance from the source vertex to each vertex
    private int[] predecessors; // array to store the predecessor of each vertex in the shortest path

    /// <summary>
    /// Create a new instance of the BellmanFordDistance class.
    /// </summary>
    /// <param name="V">number of vertices</param>
    /// <param name="E">number of edges</param>
    public BellmanFordDistance(int V, int E)
    {
        this.V = V;
        this.E = E;
        edges = new List<Edge>();
        distances = new int[V];
        predecessors = new int[V];
    }

    public void AddEdge(int src, int dest, int weight)
    {
        edges.Add(new Edge(src, dest, weight));
    }

    public void CalculateShortestPath(int src)
    {
        // initialize distances and predecessors
        for (int i = 0; i < V; i++)
        {
            distances[i] = int.MaxValue;
            predecessors[i] = -1;
        }

        distances[src] = 0;

        // relax edges V-1 times
        for (int i = 1; i < V; i++)
        {
            foreach (Edge edge in edges)
            {
                if (distances[edge.Src] != int.MaxValue && distances[edge.Src] + edge.Weight < distances[edge.Dest])
                {
                    distances[edge.Dest] = distances[edge.Src] + edge.Weight;
                    predecessors[edge.Dest] = edge.Src;
                }
            }
        }

        // check for negative-weight cycles
        foreach (Edge edge in edges)
        {
            if (distances[edge.Src] != int.MaxValue && distances[edge.Src] + edge.Weight < distances[edge.Dest])
            {
                throw new Exception("Graph contains negative-weight cycle");
            }
        }
    }

    public List<int> GetShortestPath(int dest)
    {
        List<int> path = new List<int>();
        int curr = dest;
        while (curr != -1)
        {
            path.Insert(0, curr);
            curr = predecessors[curr];
        }

        return path;
    }

    public int GetDistance(int dest)
    {
        return distances[dest];
    }

    public Dictionary<int, Tuple<List<int>, int>> GetShortestPathsAndDistances(int src)
    {
        // initialize dictionary
        Dictionary<int, Tuple<List<int>, int>> shortestPaths = new Dictionary<int, Tuple<List<int>, int>>();

        // calculate shortest path and distance for each vertex
        for (int i = 0; i < V; i++)
        {
            CalculateShortestPath(src);
            List<int> path = GetShortestPath(i);
            int distance = GetDistance(i);
            shortestPaths.Add(i, new Tuple<List<int>, int>(path, distance));
        }

        return shortestPaths;
    }

    private class Edge
    {
        public int Src { get; }
        public int Dest { get; }
        public int Weight { get; }

        public Edge(int src, int dest, int weight)
        {
            Src = src;
            Dest = dest;
            Weight = weight;
        }
    }
}