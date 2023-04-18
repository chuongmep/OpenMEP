using System.Text.RegularExpressions;
using Autodesk.DesignScript.Runtime;
using Dynamo.Graph.Nodes;
using GShark.Geometry;
using Microsoft.Expression.Interactivity.Media;
using OpenMEPSandbox.Algo;
using OpenMEPSandbox.Helpers;
using Circle = Autodesk.DesignScript.Geometry.Circle;

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
    /// [Point.ProjectOntoPlane.dyn](../OpenMEPPage/geometry/dyn/Point.ProjectOntoPlane.dyn)
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
    /// [Point.ProjectOnToLine.dyn](../OpenMEPPage/geometry/dyn/Point.ProjectOnToLine.dyn)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Point ProjectOnToLine(Autodesk.DesignScript.Geometry.Point? point,
        Autodesk.DesignScript.Geometry.Line? line)
    {
        if (point is null) throw new ArgumentNullException(nameof(point));
        if (line is null) throw new ArgumentNullException(nameof(line));
        Autodesk.DesignScript.Geometry.Vector lineDirection = line.Direction.Normalized();
        Autodesk.DesignScript.Geometry.Point start = line.StartPoint;
        Autodesk.DesignScript.Geometry.Vector vector = point.AsVector().Subtract(start.AsVector());
        double projectionLength = lineDirection.Dot(vector);
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
    /// [Point.Centroid.dyn](../OpenMEPPage/geometry/dyn/Point.Centroid.dyn)
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
    /// [Point.IsOnLine.dyn](../OpenMEPPage/geometry/dyn/Point.IsOnLine.dyn)
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
    /// [Point.IsOnPlane.dyn](../OpenMEPPage/geometry/dyn/Point.IsOnPlane.dyn)
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
    /// [Point.Deconstruct.dyn](../OpenMEPPage/geometry/dyn/Point.Deconstruct.dyn)
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
    /// [Point.IsInPolygons.dyn](../OpenMEPPage/geometry/dyn/Point.IsInPolygons.dyn)
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
    /// [Point.Origin.dyn](../OpenMEPPage/geometry/dyn/Point.Origin.dyn)
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
    /// [Point.Offset.dyn](../OpenMEPPage/geometry/dyn/pic/Point.Offset.dyn)
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
        Autodesk.DesignScript.Geometry.Point? result = null;
        int index = 0;

        for (int i = 0; i < lcDevices.Count; i++)
        {
            double distance = Manhattan(lcMachine.X, lcDevices[index].X, lcMachine.Y, lcDevices[index].Y, lcMachine.Z,
                lcDevices[index].Z);
            if (distance <= min)
            {
                if (System.Math.Abs(limit - double.MaxValue) < 0.001) continue;

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
        return System.Math.Abs(x1 - x2) + System.Math.Abs(y1 - y2) + System.Math.Abs(z1 - z2);
    }

    /// <summary>
    /// return distance between two points by Manhattan distance
    /// </summary>
    /// <para name="p2">point</para>
    /// <returns name="double">manhattan distance between two point</returns>
    public static double Manhattan(Autodesk.DesignScript.Geometry.Point p1, Autodesk.DesignScript.Geometry.Point p2)
    {
        return System.Math.Abs(p1.X - p2.X) + System.Math.Abs(p1.Y - p2.Y) + System.Math.Abs(p1.Z - p2.Z);
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
        return System.Math.Sqrt(System.Math.Pow(x1 - x2, 2) + System.Math.Pow(y1 - y2, 2) +
                                System.Math.Pow(z1 - z2, 2));
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
            mincost += originCost![i, assignment[i]];
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

    /// <summary>
    /// takes a list of 3D points as input and returns the shortest route that visits each point exactly once'
    /// https://en.wikipedia.org/wiki/Travelling_salesman_problem
    /// </summary>
    /// <param name="points">the list 3d points</param>
    /// <returns name="lines"> shortest route</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.FindShortestRoute.gif)
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Line> FindShortestRoute(
        List<Autodesk.DesignScript.Geometry.Point> points)
    {
        List<Autodesk.DesignScript.Geometry.Point> shortestRoute = TravellingSalesman.FindShortestRoute(points);
        // connect line 
        List<Autodesk.DesignScript.Geometry.Line> lines = new List<Autodesk.DesignScript.Geometry.Line>();
        for (int i = 0; i < shortestRoute.Count - 1; i++)
        {
            Autodesk.DesignScript.Geometry.Line line =
                Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(shortestRoute[i], shortestRoute[i + 1]);
            lines.Add(line);
        }

        return lines;
    }

    /// <summary>
    /// Generates a given number of random points within a sphere of the given radius.
    /// </summary>
    /// <param name="radius">The radius of the sphere.</param>
    /// <param name="numPoints">The number of random points to generate.</param>
    /// <returns>A list of randomly generated points within the sphere.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.GenerateRandomPointsInSphere.png)
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Point> GenerateRandomPointsInSphere(double radius, int numPoints)
    {
        List<Autodesk.DesignScript.Geometry.Point> points = new List<Autodesk.DesignScript.Geometry.Point>();

        for (int i = 0; i < numPoints; i++)
        {
            // Generate random coordinates within a unit sphere
            double u = RandomNumber(-1.0, 1.0);
            double v = RandomNumber(-1.0, 1.0);
            double w = RandomNumber(-1.0, 1.0);

            double x = System.Math.Sqrt(1 - System.Math.Pow(w, 2)) * System.Math.Sin(2 * System.Math.PI * u);
            double y = System.Math.Sqrt(1 - System.Math.Pow(w, 2)) * System.Math.Cos(2 * System.Math.PI * u);
            double z = w;

            x *= radius;
            y *= radius;
            z *= radius;

            points.Add(Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y, z));
        }

        return points;
    }

    /// <summary>
    /// Generates a given number of random points within a rectangular prism (i.e., a cube with different dimensions) of the given size.
    /// </summary>
    /// <param name="width">The width of the rectangular prism (i.e., the length of the x-axis).</param>
    /// <param name="height">The height of the rectangular prism (i.e., the length of the y-axis).</param>
    /// <param name="length">The length of the rectangular prism (i.e., the length of the z-axis).</param>
    /// <param name="numPoints">The number of random points to generate.</param>
    /// <returns>A list of randomly generated points within the rectangular prism.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.GenerateRandomPointsInCube.png)
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Point> GenerateRandomPointsInCube(double width, double height,
        double length, int numPoints)
    {
        List<Autodesk.DesignScript.Geometry.Point> points = new List<Autodesk.DesignScript.Geometry.Point>();

        for (int i = 0; i < numPoints; i++)
        {
            double x = RandomNumber(-width / 2, width / 2);
            double y = RandomNumber(-height / 2, height / 2);
            double z = RandomNumber(-length / 2, length / 2);

            points.Add(Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y, z));
        }

        return points;
    }

    /// <summary>
    /// Generates an array of random 3D points on the circumference of a specified circle.
    /// </summary>
    /// <param name="circle">The circle to generate points on the circumference of.</param>
    /// <param name="numPoints">The number of random points to generate.</param>
    /// <returns>An array of Point3d objects representing the generated random points.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.GenerateRandomPointsOnCircle.png)
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Point> GenerateRandomPointsOnCircle(Circle circle,
        double numPoints)
    {
        List<Autodesk.DesignScript.Geometry.Point> points = new List<Autodesk.DesignScript.Geometry.Point>();

        // Generate random points on the circle
        Random random = new Random();
        for (int i = 0; i < numPoints; i++)
        {
            double theta = random.NextDouble() * 2 * System.Math.PI;
            var n = circle.Normal;
            var radius = circle.Radius;
            var center = circle.CenterPoint;
            var u = Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0, -n.Z, n.Y);
            var v = Autodesk.DesignScript.Geometry.Vector.ByCoordinates(
                System.Math.Pow(n.Y, 2) + System.Math.Pow(n.Z, 2), -n.X * n.Y, -n.X * n.Z);
            var x = center.X + radius * v.X * System.Math.Cos(theta) + radius * u.X * System.Math.Sin(theta);
            var y = center.Y + radius * v.Y * System.Math.Cos(theta) + radius * u.Y * System.Math.Sin(theta);
            var z = center.Z + radius * v.Z * System.Math.Cos(theta) + radius * u.Z * System.Math.Sin(theta);
            points.Add(Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y, z));
        }

        return points;
    }

    /// <summary>
    /// Generates an array of random 3D points inside a specified circle.
    /// </summary>
    /// <param name="circle">The circle to generate points inside of.</param>
    /// <param name="numPoints">The number of random points to generate.</param>
    /// <returns>An array of Point3d objects representing the generated random points.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.GenerateRandomPointInCircle.png)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Point[] GenerateRandomPointInCircle(Circle circle, int numPoints)
    {
        Random random = new Random();
        Autodesk.DesignScript.Geometry.Point[] points = new Autodesk.DesignScript.Geometry.Point[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            double radius =
                circle.Radius * System.Math.Sqrt(random.NextDouble()); // random radius between 0 and circle radius
            double angle = 2 * System.Math.PI * random.NextDouble(); // random angle between 0 and 2π radians

            double x = circle.CenterPoint.X + radius * System.Math.Cos(angle); // x-coordinate of the point
            double y = circle.CenterPoint.Y + radius * System.Math.Sin(angle); // y-coordinate of the point

            // calculate the z-coordinate of the point by generating a random value between -1 and 1
            // and scaling it by the distance from the circle's center to the point in the x-y plane
            double z = circle.CenterPoint.Z + 2 * (random.NextDouble() - 0.5) *
                System.Math.Sqrt(circle.Radius * circle.Radius - radius * radius);
            Autodesk.DesignScript.Geometry.Point coordinates =
                Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y, z);
            Autodesk.DesignScript.Geometry.Point point = ProjectOntoPlane(coordinates,
                Autodesk.DesignScript.Geometry.Plane.ByOriginNormal(circle.CenterPoint, circle.Normal));
            points[i] = point;
        }

        return points;
    }

    private static Random random = new Random();

    private static double RandomNumber(double min, double max)
    {
        return random.NextDouble() * (max - min) + min;
    }

    /// <summary>
    /// Returns a new point with the smallest integer values that are greater than or equal to the X, Y, and Z coordinates of the input point.
    /// </summary>
    /// <param name="point">The input point.</param>
    /// <returns>The point with the smallest integer values that are greater than or equal to the X, Y, and Z coordinates of the input point.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.Floor.png)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Point Floor(Autodesk.DesignScript.Geometry.Point point)
    {
        int x = (int) System.Math.Floor(point.X);
        int y = (int) System.Math.Floor(point.Y);
        int z = (int) System.Math.Floor(point.Z);
        return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y, z);
    }

    /// <summary>
    /// Returns a new point with the larger integer values that are greater than or equal to the X, Y, and Z coordinates of the input point.
    /// </summary>
    /// <param name="point">The input point.</param>
    /// <returns>The point with the larger integer values that are greater than or equal to the X, Y, and Z coordinates of the input point.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.Ceiling.png)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Point Ceiling(Autodesk.DesignScript.Geometry.Point point)
    {
        int x = (int) System.Math.Floor(point.X);
        int y = (int) System.Math.Floor(point.Y);
        int z = (int) System.Math.Floor(point.Z);
        return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y, z);
    }

    /// <summary>
    /// Sorts a list of 3D points by their direction relative to a specified direction vector.
    /// </summary>
    /// <param name="points">The list of points to be sorted.</param>
    /// <param name="direction">The direction vector relative to which the points will be sorted.</param>
    /// <returns name="points">A new list of points sorted by their direction relative to the specified direction vector.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.SortPointsByDirection.png)
    /// [Point.SortPointsByDirection.dyn](../OpenMEPPage/geometry/dyn/Point.SortPointsByDirection.dyn)
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Point> SortPointsByDirection(
        List<Autodesk.DesignScript.Geometry.Point> points, Autodesk.DesignScript.Geometry.Vector direction)
    {
        if (direction == null) throw new ArgumentNullException(nameof(direction));
        if (points == null) throw new ArgumentNullException(nameof(points));
        if (points.Count == 1 || points.Count == 0) return points;
        // Normalize the direction vector
        double length =
            System.Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y + direction.Z * direction.Z);
        Autodesk.DesignScript.Geometry.Point normalizedDirection =
            Autodesk.DesignScript.Geometry.Point.ByCoordinates(direction.X / length, direction.Y / length,
                direction.Z / length);

        // Calculate the dot product of each point with the normalized direction vector
        List<double> dotProducts = points.Select(p =>
            p.X * normalizedDirection.X + p.Y * normalizedDirection.Y + p.Z * normalizedDirection.Z).ToList();

        // Combine the dot products and points into a list of tuples
        List<(double dotProduct, Autodesk.DesignScript.Geometry.Point point)> dotProductsAndPoints =
            dotProducts.Zip(points, (dotProduct, point) => (dotProduct, point)).ToList();

        // Sort the list by dot product, which will sort the points by direction
        dotProductsAndPoints.Sort();

        // Extract the sorted points from the sorted list of tuples
        List<Autodesk.DesignScript.Geometry.Point> sortedPoints = dotProductsAndPoints.Select(dp => dp.point).ToList();

        return sortedPoints;
    }

    /// <summary>
    /// Sorts a list of Point3D objects by their clockwise order relative to a center point
    /// and a specified starting angle in degrees.
    /// </summary>
    /// <param name="points">The list of Point3D objects to be sorted</param>
    /// <param name="startAngle">The starting angle in degrees for the clockwise ordering</param>
    /// <returns name="points">The sorted list of Point3D objects</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Point.SortPointsByClockwise.png)
    /// [Point.SortPointsByClockwise.dyn](../OpenMEPPage/geometry/dyn/Point.SortPointsByClockwise.dyn)
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Point> SortPointsByClockwise(
        List<Autodesk.DesignScript.Geometry.Point> points,
        double  startAngle =0)
    {
        // find the center point of the polygon
        Point3 center = new Point3(0, 0, 0);
        foreach (var point in points)
        {
            center.X += point.X;
            center.Y += point.Y;
            center.Z += point.Z;
        }
        center.X /= points.Count;
        center.Y /= points.Count;
        center.Z /= points.Count;
        // sort the points by their clockwise order
        points.Sort((p1, p2) => {
            double angle1 = GetAngle(p1,center, startAngle);
            double angle2 = GetAngle(p2,center, startAngle);
            if (angle1 < angle2) return -1;
            if (angle1 > angle2) return 1;
            return 0;
        });
        return points;
    }
    private static double GetAngle(Autodesk.DesignScript.Geometry.Point point,Point3 center, double startAngle)
    {
        double angle = System.Math.Atan2(point.Y - center.Y, point.X - center.X) * 180 / System.Math.PI;
        if (angle < startAngle) angle += 360;
        return angle;
    }
}