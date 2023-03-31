using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Algo;

[IsVisibleInDynamoLibrary(false)]
public static class TravellingSalesman
{
    
    /// <summary>
    /// takes a list of 3D points as input and returns the shortest route that visits each point exactly once
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public static List<Point> FindShortestRoute(List<Point> points)
    {
        List<Point> shortestRoute = null;
        double shortestDistance = double.MaxValue;

        // Generate all possible permutations of the points
        List<List<Point>> permutations = Permute(points);

        // For each permutation, calculate the total distance and keep track of the shortest route
        foreach (List<Point> route in permutations)
        {
            double totalDistance = CalculateTotalDistance(route);
            if (totalDistance < shortestDistance)
            {
                shortestDistance = totalDistance;
                shortestRoute = route;
            }
        }

        return shortestRoute;
    }

    /// <summary>
    /// takes a list of 3D points as input and returns a list of all possible permutations of the input list
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    static List<List<Point>> Permute(List<Point> points)
    {
        List<List<Point>> result = new List<List<Point>>();
        PermuteHelper(points, 0, result);
        return result;
    }

    /// <summary>
    /// takes three arguments: the input list of points, the current index to swap, and the list to store the permutations
    /// </summary>
    /// <param name="points"></param>
    /// <param name="index"></param>
    /// <param name="result"></param>
    static void PermuteHelper(List<Point> points, int index, List<List<Point>> result)
    {
        if (index >= points.Count)
        {
            result.Add(new List<Point>(points));
        }
        else
        {
            for (int i = index; i < points.Count; i++)
            {
                Swap(points, index, i);
                PermuteHelper(points, index + 1, result);
                Swap(points, index, i);
            }
        }
    }

    /// <summary>
    ///  takes a list of points and two indices and swaps the elements at those indices in the list.
    /// </summary>
    /// <param name="points"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    static void Swap(List<Point> points, int i, int j)
    {
        (points[i], points[j]) = (points[j], points[i]);
    }

    static double CalculateTotalDistance(List<Point> route)
    {
        double totalDistance = 0;
        for (int i = 0; i < route.Count - 1; i++)
        {
            totalDistance += CalculateDistance(route[i], route[i + 1]);
        }

        totalDistance += CalculateDistance(route[route.Count - 1], route[0]);
        return totalDistance;
    }

    /// <summary>
    /// calculates the distance between two points by using euclidean distance formula
    /// </summary>
    /// <param name="point1">the first point</param>
    /// <param name="point2">the second point</param>
    /// <returns>distance between two point</returns>
    static double CalculateDistance(Autodesk.DesignScript.Geometry.Point point1,
        Autodesk.DesignScript.Geometry.Point point2)
    {
        double dx = point2.X - point1.X;
        double dy = point2.Y - point1.Y;
        double dz = point2.Z - point1.Z;
        return System.Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }
}