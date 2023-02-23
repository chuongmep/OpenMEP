using GShark.Geometry;
using OpenMEP.Helpers;

namespace OpenMEP.Geometry;

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
        Autodesk.DesignScript.Geometry.Vector planeNormal )
    {
        double a = planeNormal.X;
        double b = planeNormal.Y;
        double c = planeNormal.Z;

        double dx = ( b * b + c * c ) * point.X - ( a * b ) * point.Y - ( a * c ) * point.Z;
        double dy = -( b * a ) * point.X + ( a * a + c * c ) * point.Y - ( b * c ) * point.Z;
        double dz = -( c * a ) * point.X - ( c * b ) * point.Y + ( a * a + b * b ) * point.Z;
        return Autodesk.DesignScript.Geometry.Point.ByCoordinates( dx, dy, dz );
    }
    
    /// <summary>
    /// Get the centroid of a list of points
    /// </summary>
    /// <param name="points">list of points</param>
    /// <returns name="point">centroid</returns>
    public static Autodesk.DesignScript.Geometry.Point Centroid( List<Autodesk.DesignScript.Geometry.Point> points )
    {
        double x = 0;
        double y = 0;
        double z = 0;
        foreach ( Autodesk.DesignScript.Geometry.Point point in points )
        {
            x += point.X;
            y += point.Y;
            z += point.Z;
        }
        return Autodesk.DesignScript.Geometry.Point.ByCoordinates( x / points.Count, y / points.Count, z / points.Count );
    }

    /// <summary>Test whether a point lies on a line.</summary>
    /// <param name="point">a point to check</param>
    /// <param name="line">The line to test against.</param>
    /// <param name="tolerance">Default is use 1e-6</param>
    /// <returns name="bool">Returns true if point is on line.</returns>
    public static bool IsOnLine(Autodesk.DesignScript.Geometry.Point point,Autodesk.DesignScript.Geometry.Line line,double tolerance = 0.001)
    {
        Point3 point3 = new Point3(point.X, point.Y,point.Z);
        return point3.IsOnLine(line.ToGSharkType(), tolerance);
    }

    /// <summary>Test whether a point lies on a plane.</summary>
    /// <param name="point">point to check</param>
    /// <param name="plane">The plane to test against.</param>
    /// <param name="tolerance">Default is use 1e-6</param>
    /// <returns>Returns true if point is on plane.</returns>
    public static bool IsOnPlane(Autodesk.DesignScript.Geometry.Point point,Autodesk.DesignScript.Geometry.Plane plane,double tolerance = 0.001)
    {
        Point3 point3 = new Point3(point.X, point.Y,point.Z);
        return point3.IsOnPlane(plane.ToGSharkType(), tolerance);
    }
}