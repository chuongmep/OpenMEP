using Autodesk.Revit.DB;

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
}