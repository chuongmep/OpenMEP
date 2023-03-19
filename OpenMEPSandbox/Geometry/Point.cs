using Autodesk.DesignScript.Runtime;
using Dynamo.Graph.Nodes;
using GShark.Geometry;
using Microsoft.Expression.Interactivity.Media;
using OpenMEPSandbox.Algo;
using OpenMEPSandbox.Helpers;

namespace OpenMEPSandbox.Geometry;

public class Point
{
    private Point()
    {
    }

    /// <summary>
    /// Project a point onto a plane
    /// </summary>
    /// <param name="point">point need to project</param>
    /// <param name="plane">plane to be project</param>
    /// <returns name="point">new point projected on plane</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.ProjectOntoPlane.gif)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Point ProjectOntoPlane(
        Autodesk.DesignScript.Geometry.Point point,
        Autodesk.DesignScript.Geometry.Plane plane)
    {
        Point3 point3 = point.ToGSharkType();
        GShark.Geometry.Plane plane1 = plane.ToGSharkType();
        Point3 projectedPoint = point3.ProjectToPlan(plane1);
        return Autodesk.DesignScript.Geometry.Point.ByCoordinates(projectedPoint.X, projectedPoint.Y, projectedPoint.Z);
    }

    /// <summary>
    /// Project a point onto a line
    /// </summary>
    /// <param name="point">Point need to project</param>
    /// <param name="line">Line to project the point</param>
    /// <returns name="point">projected point</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.ProjectOnToLine.gif)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Point ProjectOnToLine(Autodesk.DesignScript.Geometry.Point? point,
        Autodesk.DesignScript.Geometry.Line? line)
    {
        Autodesk.DesignScript.Geometry.Vector lineDirection = line.Direction.Normalized();
        Autodesk.DesignScript.Geometry.Point start = line.StartPoint;
        Autodesk.DesignScript.Geometry.Vector vector = point.AsVector().Subtract(start.AsVector());
        double projectionLength  = lineDirection.Dot(vector);
        var ProjectedPoint = start.Add(lineDirection.Scale(projectionLength));
        return ProjectedPoint;
    }
    
    /// <summary>
    /// Get the centroid of a list of points
    /// </summary>
    /// <param name="points">list of points</param>
    /// <returns name="point">centroid</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.Centroid.gif)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Point Centroid(List<Autodesk.DesignScript.Geometry.Point> points)
    {
        double x = 0;
        double y = 0;
        double z = 0;
        foreach (Autodesk.DesignScript.Geometry.Point point in points)
        {
            x += point.X;
            y += point.Y;
            z += point.Z;
        }

        return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x / points.Count, y / points.Count, z / points.Count);
    }

    /// <summary>Test whether a point lies on a line.</summary>
    /// <param name="point">a point to check</param>
    /// <param name="line">The line to test against.</param>
    /// <param name="tolerance">Default is use 1e-6</param>
    /// <returns name="bool">Returns true if point is on line.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.IsOnLine.gif)
    /// </example>
    [NodeCategory("Query")]
    public static bool IsOnLine(Autodesk.DesignScript.Geometry.Point point, Autodesk.DesignScript.Geometry.Line line,
        double tolerance = 0.001)
    {
        Point3 point3 = new Point3(point.X, point.Y, point.Z);
        return point3.IsOnLine(line.ToGSharkType(), tolerance);
    }

    /// <summary>Test whether a point lies on a plane.</summary>
    /// <param name="point">point to check</param>
    /// <param name="plane">The plane to test against.</param>
    /// <param name="tolerance">Default is use 1e-6</param>
    /// <returns>Returns true if point is on plane.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.IsOnPlane.gif)
    /// </example>
    [NodeCategory("Query")]
    public static bool IsOnPlane(Autodesk.DesignScript.Geometry.Point point, Autodesk.DesignScript.Geometry.Plane plane,
        double tolerance = 0.001)
    {
        Point3 point3 = new Point3(point.X, point.Y, point.Z);
        return point3.IsOnPlane(plane.ToGSharkType(), tolerance);
    }


    /// <summary>
    ///  Deconstruct a point into its components
    /// </summary>
    /// <param name="point">the point</param>
    /// <returns name="X">X point</returns>
    /// <returns name="Y">Y point</returns>
    /// <returns name="Z">Z point</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.Deconstruct.png)
    /// </example>
    [MultiReturn("X", "Y", "Z")]
    public static Dictionary<string, object?> Deconstruct(
        [DefaultArgument("Autodesk.DesignScript.Geometry.Point.ByCoordinates(0,0,0)")]
        Autodesk.DesignScript.Geometry.Point point)
    {
        if (point == null) throw new ArgumentNullException(nameof(point));
        return new Dictionary<string, object?>
        {
            {"X", point.X},
            {"Y", point.Y},
            {"Z", point.Z}
        };
    }

    // //
    // // //TODO : Got some bug from G-Shark Library
    // /// <summary>
    // /// Tests whether a point is inside, outside, or coincident with a polygon.
    // /// </summary>
    // /// <returns name="double">Returns -1 if point is outside the polygon, 0 if it is coincident with a polygon edge, or 1 if it is inside the polygon.</returns>
    // public static double IsInPolygon2(Autodesk.DesignScript.Geometry.Point point,
    //     Autodesk.DesignScript.Geometry.Polygon polygon)
    // {
    //     if (point == null) throw new ArgumentNullException(nameof(point));
    //     if (polygon == null) throw new ArgumentNullException(nameof(polygon));
    //     Point3 point3 = point.ToGSharkType();
    //     Polygon polyGshark = polygon.ToGSharkType();
    //     return point3.InPolygon(polyGshark);
    // }

    /// <summary>
    /// Returns whether an input point is contained within the polygon. If the polygon is not planar then the point will be projected onto the best-fit plane and the containment will be computed using the projection of the polygon onto the best-fit plane. This will return a failed status if the polygon self-intersects.
    /// </summary>
    /// <param name="point">the point</param>
    /// <param name="polygon">the polygon</param>
    /// <returns name="bool">true if point is in polygon</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.IsInPolygons.gif)
    /// </example>
    [NodeCategory("Query")]
    public static bool IsInPolygon(Autodesk.DesignScript.Geometry.Point point,
        Autodesk.DesignScript.Geometry.Polygon polygon)
    {
        if (point == null) throw new ArgumentNullException(nameof(point));
        if (polygon == null) throw new ArgumentNullException(nameof(polygon));
        return polygon.ContainmentTest(point);
    }

    /// <summary>
    ///     Gets a point with X,Y,Z = 0
    /// </summary>
    /// <returns name="point">point</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.Origin.png)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Point Origin()
    {
        return Autodesk.DesignScript.Geometry.Point.ByCoordinates(0, 0, 0);
    }

    /// <summary>
    /// Offset a point by a distance and a direction
    /// </summary>
    /// <param name="point">point to offset</param>
    /// <param name="distance">distance from start point to end point</param>
    /// <param name="direction">direction to direct to</param>
    /// <returns name="point">new point</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.Offset.gif)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Point Offset(Autodesk.DesignScript.Geometry.Point point,
        double distance, Autodesk.DesignScript.Geometry.Vector direction)
    {
        return point.Add(direction.ToGSharkType().Amplify(distance).ToDynamoType());
    }


    /// <summary>
    /// return the closest point from a list of points by manhattan distance
    /// </summary>
    /// <param name="lcMachine">location of machine</param>
    /// <param name="lcDevices">location of devide</param>
    /// <param name="limit">max limit</param>
    /// <returns></returns>
    [MultiReturn("point", "distance")]
    public static Dictionary<string, object?> FindLocationShortest(Autodesk.DesignScript.Geometry.Point lcMachine,
        List<Autodesk.DesignScript.Geometry.Point> lcDevices, double limit = double.MaxValue)
    {
        double min = double.MaxValue;
        Autodesk.DesignScript.Geometry.Point result = null;
        int index = 0;

        for (int i = 0; i < lcDevices.Count; i++)
        {
            double distance = Manhattan(lcMachine.X, lcDevices[index].X, lcMachine.Y, lcDevices[index].Y, lcMachine.Z,
                lcDevices[index].Z);
            if (distance <= min)
            {
                if (Math.Abs(limit - double.MaxValue) < 0.001) continue;

                min = distance;
                result = lcDevices[index];
                index++;
            }
            else
            {
                index++;
            }
        }

        return new Dictionary<string, object?>()
        {
            {"point", result},
            {"distance", min}
        };
    }

    internal static double Manhattan(double x1, double x2, double y1, double y2, double z1, double z2)
    {
        return Math.Abs(x1 - x2) + Math.Abs(y1 - y2) + Math.Abs(z1 - z2);
    }

    /// <summary>
    /// return distance between two points by Manhattan distance
    /// </summary>
    /// <para name="p2">point</para>
    /// <returns name="double">manhattan distance between two point</returns>
    public static double Manhattan(Autodesk.DesignScript.Geometry.Point p1, Autodesk.DesignScript.Geometry.Point p2)
    {
        return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y) + Math.Abs(p1.Z - p2.Z);
    }

    /// <summary>
    /// return distance two points by Euclidean distance
    /// </summary>
    /// <param name="p1">the first point</param>
    /// <param name="p2">the second point</param>
    /// <returns name="double">euclidean between two point</returns>
    public static double Euclidean(Autodesk.DesignScript.Geometry.Point p1, Autodesk.DesignScript.Geometry.Point p2)
    {
        return Euclidean(p1.X, p2.X, p1.Y, p2.Y, p1.Z, p2.Z);
    }


    /// <summary>
    /// return distance between two points by Euclidean distance
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="x2"></param>
    /// <param name="y1"></param>
    /// <param name="y2"></param>
    /// <param name="z1"></param>
    /// <param name="z2"></param>
    /// <returns></returns>
    internal static double Euclidean(double x1, double x2, double y1, double y2, double z1, double z2)
    {
        return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2) + Math.Pow(z1 - z2, 2));
    }

    /// <summary>
    /// Assignments Optimize by using Hungarian Algorithm
    /// </summary>
    /// <param name="lcMachines">list location of machine</param>
    /// <param name="lcDevices">list location of devices</param>
    /// <param name="limit">number distance limit to break</param>
    [MultiReturn("lines", "machines", "devices")]
    public static Dictionary<string, object?> AssignmentMatching(List<Autodesk.DesignScript.Geometry.Point> lcMachines,
        List<Autodesk.DesignScript.Geometry.Point> lcDevices, double limit = double.MaxValue)
    {
        int[,] cost = new int[lcMachines.Count, lcDevices.Count];
        for (int i = 0; i < cost.GetLength(0); i++)
        {
            for (int j = 0; j < cost.GetLength(1); j++)
            {
                int manhattan = (int) Manhattan(lcMachines[i], lcDevices[j]);
                if (manhattan > limit) cost[i, j] = int.MaxValue;
                cost[i, j] = manhattan;
            }
        }

        int[] result = cost.FindAssignments();
        List<Autodesk.DesignScript.Geometry.Line> lines = new List<Autodesk.DesignScript.Geometry.Line>();
        List<Autodesk.DesignScript.Geometry.Point> machines = new List<Autodesk.DesignScript.Geometry.Point>();
        List<Autodesk.DesignScript.Geometry.Point> devices = new List<Autodesk.DesignScript.Geometry.Point>();
        for (int i = 0; i < result.Length; i++)
        {
            Autodesk.DesignScript.Geometry.Line line =
                Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(lcMachines[i], lcDevices[result[i]]);
            if (line.Length <= limit)
            {
                lines.Add(line);
                machines.Add(lcMachines[i]);
                devices.Add(lcDevices[result[i]]);
            }
        }

        return new Dictionary<string, object?>()
        {
            {"lines", lines},
            {"machines", machines},
            {"devices", devices}
        };
    }

    /// <summary>
    /// Assignments Optimize by using Hungarian Algorithm
    /// </summary>
    /// <param name="lcMachines">list location of machine</param>
    /// <param name="lcDevices">list location of devices</param>
    /// <returns name="Permutation">Permutation of list index matching optimize</returns>
    /// <returns name="mincost">minimum cost can optimize</returns>
    /// <returns name="assignment">index optimize can assignment</returns>
    [MultiReturn("assignment", "mincost")]
    public static Dictionary<string, object?> AssignmentMatching(List<Autodesk.DesignScript.Geometry.Point> lcMachines,
        List<Autodesk.DesignScript.Geometry.Point> lcDevices)
    {
        int[,] cost = new int[lcMachines.Count, lcDevices.Count];
        for (int i = 0; i < cost.GetLength(0); i++)
        {
            for (int j = 0; j < cost.GetLength(1); j++)
            {
                int manhattan = (int) Manhattan(lcMachines[i], lcDevices[j]);
                cost[i, j] = manhattan;
            }
        }

        // TODO : Array Matching Index 
        int[,]? originCost = cost.Clone() as int[,];
        List<int> assignment = cost.FindAssignments().ToList();
        // fin min cost
        int mincost = 0;
        for (int i = 0; i < assignment.Count; i++)
        {
            mincost += originCost[i, assignment[i]];
        }

        return new Dictionary<string, object?>()
        {
            {"assignment", assignment},
            {"mincost", mincost},
        };
    }

    /// <summary>
    /// Assignments Optimize by using Brute Force
    /// </summary>
    /// <param name="lcMachines">list location of machine</param>
    /// <param name="lcDevices">list location of devices</param>
    /// <returns name="mincost">minimum cost can optimize</returns>
    /// <returns name="assignment">index optimize can assignment</returns>
    [MultiReturn("assignment", "mincost")]
    public static Dictionary<string, object?> BruteForceMatching(List<Autodesk.DesignScript.Geometry.Point> lcMachines,
        List<Autodesk.DesignScript.Geometry.Point> lcDevices)
    {
        int[,] cost = new int[lcMachines.Count, lcDevices.Count];
        for (int i = 0; i < cost.GetLength(0); i++)
        {
            for (int j = 0; j < cost.GetLength(1); j++)
            {
                int manhattan = (int) Manhattan(lcMachines[i], lcDevices[j]);
                cost[i, j] = manhattan;
            }
        }

        BruteForceMethod bruteForceMethod = new BruteForceMethod();
        (int minCost, IEnumerable<int> minAssignment) result = bruteForceMethod.BruteForce(cost);
        IEnumerable<int> assignment = result.minAssignment;
        int minCost = result.minCost;
        return new Dictionary<string, object?>()
        {
            {"assignment", assignment},
            {"mincost", minCost},
        };
    }

    /// <summary>
    ///  Reflect Point by Plane
    /// </summary>
    /// <param name="point">point need to reflect</param>
    /// <param name="plane">plane to reflect point</param>
    /// <returns name="point">point has reflected</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.Reflect.gif)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Point Reflect(Autodesk.DesignScript.Geometry.Point point,
        Autodesk.DesignScript.Geometry.Plane plane)
    {
        Autodesk.DesignScript.Geometry.Vector v1 = point.AsVector();
        double dot = v1.Dot(plane.Normal);
        Autodesk.DesignScript.Geometry.Vector v2 = plane.Normal.Scale(2 * dot);
        Autodesk.DesignScript.Geometry.Vector v3 = v1.Subtract(v2);
        return v3.AsPoint();
    }
    /// <summary>
    /// Compares this a point with another point.
    /// 0: if this is identical to other
    /// 1: if this is greater than other
    /// -1: if this is less than other
    /// <para>Component evaluation priority is first X, then Y, then Z.</para>
    /// </summary>
    /// <param name="point1">the first point to use in comparison</param>
    /// <param name="point2">the second point to use in comparison</param>
    /// <returns name="double">value compare</returns>
    /// <returns>The extended line.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.CompareTo.png)
    /// </example>
    public static double CompareTo(Autodesk.DesignScript.Geometry.Point point1,
        Autodesk.DesignScript.Geometry.Point point2)
    {
        int compareTo = point1.ToGSharkType().CompareTo(point2.ToGSharkType());
        return compareTo;
    }
}