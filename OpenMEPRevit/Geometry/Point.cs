using Autodesk.Revit.DB;
using Revit.GeometryConversion;
using RevitServices.Persistence;

namespace OpenMEPRevit.Geometry;

public class Point
{
    private Point()
    {
        
    }
    /// <summary>
    /// Converts a point from the origin coordinate system to the shared coordinate system in Revit.
    /// </summary>
    /// <param name="point">The point in the origin coordinate system to be converted.</param>
    /// <returns>The converted point in the shared coordinate system.</returns>
    public static Autodesk.DesignScript.Geometry.Point ConvertOriginToSharedCoordinate(Autodesk.DesignScript.Geometry.Point point)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        XYZ ofPoint = doc.ActiveProjectLocation.GetTotalTransform().Inverse.OfPoint(point.ToXyz());
        return ofPoint.ToPoint();
    }
    /// <summary>
    /// Converts a point from the shared coordinate system to the origin coordinate system in Revit.
    /// </summary>
    /// <param name="point">The point in the shared coordinate system to be converted.</param>
    /// <returns>The converted point in the origin coordinate system.</returns>
    public static Autodesk.DesignScript.Geometry.Point ConvertSharedCoordinateToOrigin(Autodesk.DesignScript.Geometry.Point point)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        XYZ ofPoint = doc.ActiveProjectLocation.GetTotalTransform().OfPoint(point.ToXyz());
        return ofPoint.ToPoint();
    }
}