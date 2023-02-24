using Autodesk.DesignScript.Runtime;
using Dynamo.Graph.Nodes;
using GShark.Geometry;
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
    /// <param name="planeNormal">vector normal of plane</param>
    /// <returns name="point">new point projected on plane</returns>
    public static Autodesk.DesignScript.Geometry.Point ProjectOntoPlane(
        Autodesk.DesignScript.Geometry.Point point,
        Autodesk.DesignScript.Geometry.Vector planeNormal)
    {
        double a = planeNormal.X;
        double b = planeNormal.Y;
        double c = planeNormal.Z;

        double dx = (b * b + c * c) * point.X - (a * b) * point.Y - (a * c) * point.Z;
        double dy = -(b * a) * point.X + (a * a + c * c) * point.Y - (b * c) * point.Z;
        double dz = -(c * a) * point.X - (c * b) * point.Y + (a * a + b * b) * point.Z;
        return Autodesk.DesignScript.Geometry.Point.ByCoordinates(dx, dy, dz);
    }

    /// <summary>
    /// Get the centroid of a list of points
    /// </summary>
    /// <param name="points">list of points</param>
    /// <returns name="point">centroid</returns>
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
    [MultiReturn("X", "Y", "Z")]
    public static Dictionary<string, object?> Deconstruct(Autodesk.DesignScript.Geometry.Point point)
    {
        if (point == null) throw new ArgumentNullException(nameof(point));
        return new Dictionary<string, object?>
        {
            {"X", point.X},
            {"Y", point.Y},
            {"Z", point.Z}
        };
    }

    //
    // //TODO : Got some bug from G-Shark Library
    // /// <summary>
    // /// Tests whether a point is inside, outside, or coincident with a polygon.
    // /// </summary>
    // /// <returns name="double">Returns -1 if point is outside the polygon, 0 if it is coincident with a polygon edge, or 1 if it is inside the polygon.</returns>
    // public static double IsInPolygon2(Autodesk.DesignScript.Geometry.Point point,Autodesk.DesignScript.Geometry.Polygon polygon)
    // {
    //     if (point == null) throw new ArgumentNullException(nameof(point));
    //     if (polygon == null) throw new ArgumentNullException(nameof(polygon));
    //    return point.ToGSharkType().InPolygon(polygon.ToGSharkType());
    // }

    /// <summary>
    /// Returns whether an input point is contained within the polygon. If the polygon is not planar then the point will be projected onto the best-fit plane and the containment will be computed using the projection of the polygon onto the best-fit plane. This will return a failed status if the polygon self-intersects.
    /// </summary>
    /// <param name="point">the point</param>
    /// <param name="polygon">the polygon</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [NodeCategory("Query")]
    public static bool IsInPolygon(Autodesk.DesignScript.Geometry.Point point,
        Autodesk.DesignScript.Geometry.Polygon polygon)
    {
        if (point == null) throw new ArgumentNullException(nameof(point));
        if (polygon == null) throw new ArgumentNullException(nameof(polygon));
        return polygon.ContainmentTest(point);
    }
}