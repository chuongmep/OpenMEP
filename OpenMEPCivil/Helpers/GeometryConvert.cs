using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace OpenMEPCivil.Helpers;

[IsVisibleInDynamoLibrary(false)]
public static class GeometryConvert
{
    public static Point ToDSPoint(this Autodesk.AutoCAD.Geometry.Point3d point)
    {
        return Point.ByCoordinates(point.X, point.Y, point.Z);
    }
    public static Point ToDSPoint(this Autodesk.AutoCAD.Geometry.Point2d point)
    {
        return Point.ByCoordinates(point.X, point.Y);
    }
    public static Point ToDSPoint(this Autodesk.AutoCAD.Geometry.Vector3d point)
    {
        return Point.ByCoordinates(point.X, point.Y, point.Z);
    }
    public static Point ToDSPoint(this Autodesk.AutoCAD.Geometry.Vector2d point)
    {
        return Point.ByCoordinates(point.X, point.Y, 0);
    }
    public static Vector ToDSVector(this Autodesk.AutoCAD.Geometry.Vector3d point)
    {
        return Vector.ByCoordinates(point.X, point.Y, point.Z);
    }
    public static Vector ToDSVector(this Autodesk.AutoCAD.Geometry.Vector2d point)
    {
        return Vector.ByCoordinates(point.X, point.Y, 0);
    }
    public static Autodesk.AutoCAD.Geometry.Point3d ToAcadPoint(this Point point)
    {
        return new Autodesk.AutoCAD.Geometry.Point3d(point.X, point.Y, point.Z);
    }
    public static Autodesk.AutoCAD.Geometry.Vector3d ToAcadVector(this Vector point)
    {
        return new Autodesk.AutoCAD.Geometry.Vector3d(point.X, point.Y, point.Z);
    }
    
}