using Autodesk.Revit.DB;
using Revit.Elements;
using RevitServices.Persistence;
using RevitServices.Transactions;
using Point = Autodesk.DesignScript.Geometry.Point;
using Wall = Autodesk.Revit.DB.Wall;

namespace OpenMEPRevit.Element;

public class WallOpening
{
    private WallOpening()
    {

    }
    /// <summary>
    /// Creates a wall opening at a specified point with a given width and height.
    /// </summary>
    /// <param name="wall">The wall in which the opening is to be created.</param>
    /// <param name="point">The point at which the opening is to be created.</param>
    /// <param name="width">The width of the opening.</param>
    /// <param name="height">The height of the opening.</param>
    /// <returns>The created wall opening as a Revit element, or null if the opening could not be created.</returns>
    public static Revit.Elements.Element? ByPointWidthHeight(Revit.Elements.Wall wall, Point point, double width, double height)
    {
        Wall? wall2 = wall.InternalElement as Wall;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Units units = doc.GetUnits();
#if R20 || R21
        var displayUnits = units.GetFormatOptions(UnitType.UT_Length).DisplayUnits;
#else
        var displayUnits = units.GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();

#endif
        double num2 = UnitUtils.ConvertToInternalUnits(point.X, displayUnits);
        double num3 = UnitUtils.ConvertToInternalUnits(point.Y, displayUnits);
        double num4 = UnitUtils.ConvertToInternalUnits(point.Z, displayUnits);
        double num5 = UnitUtils.ConvertToInternalUnits(width, displayUnits);
        double num6 = UnitUtils.ConvertToInternalUnits(height, displayUnits);
        LocationCurve? locationCurve = wall2?.Location as LocationCurve;
        Curve? curve = locationCurve?.Curve;
        if (curve == null)
        {
            return null;
        }
        XYZ xyz = curve.Evaluate(0.0, true);
        XYZ xyz2 = curve.Evaluate(1.0, true);
        Line line = Line.CreateBound(xyz, xyz2);
        XYZ xyz3 = line.Direction.Normalize();
        XYZ basisZ = XYZ.BasisZ;
        XYZ basisY = xyz3.CrossProduct(basisZ).Normalize();
        XYZ origin = new XYZ(num2, num3, num4);
        Transform identity = Transform.Identity;
        identity.Origin = origin;
        identity.BasisX = xyz3;
        identity.BasisY = basisY;
        identity.BasisZ = basisZ;
        XYZ xyz4 = new XYZ(num5 / 2.0, 0.0, num6 / 2.0);
        XYZ xyz5 = new XYZ(-(num5 / 2.0), 0.0, -(num6 / 2.0));
        XYZ xyz6 = identity.OfPoint(xyz4);
        XYZ xyz7 = identity.OfPoint(xyz5);
        TransactionManager.Instance.EnsureInTransaction(doc);
        Opening opening = doc.Create.NewOpening(wall2, xyz6, xyz7);
        TransactionManager.Instance.TransactionTaskDone();
        return opening.ToDSType(true);
    }
    /// <summary>
    /// Creates a wall opening between two specified points.
    /// </summary>
    /// <param name="wall">The wall in which the opening is to be created.</param>
    /// <param name="startPoint">The start point of the opening.</param>
    /// <param name="endPoint">The end point of the opening.</param>
    /// <returns>The created wall opening as a Revit element.</returns>
    public static Revit.Elements.Element ByStartPointEndPoint(Revit.Elements.Wall wall, Point startPoint, Point endPoint)
    {
        Wall? wall2 = wall.InternalElement as Wall;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Units units = doc.GetUnits();
#if R20 || R21
       var displayUnits = units.GetFormatOptions(UnitType.UT_Length).DisplayUnits;

#else
        var displayUnits = units.GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();
#endif
        double xStart = UnitUtils.ConvertToInternalUnits(startPoint.X, displayUnits);
        double yStart = UnitUtils.ConvertToInternalUnits(startPoint.Y, displayUnits);
        double zStart = UnitUtils.ConvertToInternalUnits(startPoint.Z, displayUnits);
        double xEnd = UnitUtils.ConvertToInternalUnits(endPoint.X, displayUnits);
        double yEnd = UnitUtils.ConvertToInternalUnits(endPoint.Y, displayUnits);
        double zEnd = UnitUtils.ConvertToInternalUnits(endPoint.Z, displayUnits);
        XYZ start = new XYZ(xStart, yStart, zStart);
        XYZ end = new XYZ(xEnd, yEnd, zEnd);
        TransactionManager.Instance.EnsureInTransaction(doc);
        Opening opening = doc.Create.NewOpening(wall2, start, end);
        TransactionManager.Instance.TransactionTaskDone();
        return opening.ToDSType(true);
    }

    public static Revit.Elements.Element ByPointAndDiameter(Revit.Elements.Wall wall, Point point, double diameter)
    {
        Wall? wall2 = wall.InternalElement as Wall;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Units units = doc.GetUnits();
#if R20 || R21
          var displayUnits = units.GetFormatOptions(UnitType.UT_Length).DisplayUnits;
#else
        var displayUnits = units.GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();
#endif
        double num2 = UnitUtils.ConvertToInternalUnits(point.X, displayUnits);
        double num3 = UnitUtils.ConvertToInternalUnits(point.Y, displayUnits);
        double num4 = UnitUtils.ConvertToInternalUnits(point.Z, displayUnits);
        double num5 = UnitUtils.ConvertToInternalUnits(diameter, displayUnits);
        LocationCurve? locationCurve = wall2?.Location as LocationCurve;
        Curve? curve = locationCurve?.Curve;
        if (curve == null)
        {
            return null;
        }
        XYZ xyz = curve.Evaluate(0.0, true);
        XYZ xyz2 = curve.Evaluate(1.0, true);
        Line line = Line.CreateBound(xyz, xyz2);
        XYZ xyz3 = line.Direction.Normalize();
        XYZ basisZ = XYZ.BasisZ;
        XYZ basisY = xyz3.CrossProduct(basisZ).Normalize();
        XYZ origin = new XYZ(num2, num3, num4);
        Transform identity = Transform.Identity;
        identity.Origin = origin;
        identity.BasisX = xyz3;
        identity.BasisY = basisY;
        identity.BasisZ = basisZ;
        XYZ xyz4 = new XYZ(num5 / 2.0, 0.0, 0.0);
        XYZ xyz5 = new XYZ(-(num5 / 2.0), 0.0, 0.0);
        XYZ xyz6 = identity.OfPoint(xyz4);
        XYZ xyz7 = identity.OfPoint(xyz5);
        TransactionManager.Instance.EnsureInTransaction(doc);
        Opening opening = doc.Create.NewOpening(wall2, xyz6, xyz7);
        TransactionManager.Instance.TransactionTaskDone();
        return opening.ToDSType(true);
    }
}