using Autodesk.DesignScript.Runtime;
using Point = Autodesk.DesignScript.Geometry.Point;

namespace OpenMEPSandbox.Algo;
[IsVisibleInDynamoLibrary(false)]
public class DijkstraGraph
{
    private Dictionary<Point, Dictionary<Point, double>> adjList;

    public DijkstraGraph()
    {
        adjList = new Dictionary<Point, Dictionary<Point, double>>();
    }

    public void AddEdge(Point u, Point v)
    {
        double distance = u.DistanceTo(v);
        if (!adjList.ContainsKey(u))
        {
            adjList[u] = new Dictionary<Point, double>();
        }
        adjList[u][v] = distance;
        if (!adjList.ContainsKey(v))
        {
            adjList[v] = new Dictionary<Point, double>();
        }
        adjList[v][u] = distance;
    }

    /// <summary>
    /// Shortest path from start to end.
    /// </summary>
    /// <param name="start">start point</param>
    /// <param name="end">end point</param>
    /// <returns name="points">shortest path</returns>
    public List<Point> ShortestPath(Point start, Point end)
    {
        Dictionary<Point, double> distances = new Dictionary<Point, double>();
        Dictionary<Point, Point> predecessors = new Dictionary<Point, Point>();
        HashSet<Point> visited = new HashSet<Point>();
        PriorityQueue<Point> queue = new PriorityQueue<Point>();

        distances[start] = 0;
        queue.Enqueue(start, 0);

        while (queue.Count > 0)
        {
            Point u = queue.Dequeue();

            if (visited.Contains(u))
            {
                continue;
            }

            visited.Add(u);

            if (u == end)
            {
                break;
            }

            foreach (Point v in adjList[u].Keys)
            {
                double alt = distances[u] + adjList[u][v];
                if (!distances.ContainsKey(v) || alt < distances[v])
                {
                    distances[v] = alt;
                    predecessors[v] = u;
                    queue.Enqueue(v, alt);
                }
            }
        }

        if (!predecessors.ContainsKey(end))
        {
            return null;
        }

        List<Point> path = new List<Point>();
        Point curr = end;
        while (curr != start)
        {
            path.Insert(0, curr);
            curr = predecessors[curr];
        }
        path.Insert(0, start);

        return path;
    }
}
[IsVisibleInDynamoLibrary(false)]
class PriorityQueue<T>
{
    private List<Tuple<T, double>> elements = new List<Tuple<T, double>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, double priority)
    {
        elements.Add(Tuple.Create(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}