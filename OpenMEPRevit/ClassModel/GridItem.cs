using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;

namespace OpenMEPRevit.ClassModel;

[IsVisibleInDynamoLibrary(false)]
public class GridItem
{
    public Grid? Grid { get; set; }

    public bool IsHorizontal => Direction(Grid.Curve)!.IsAlmostEqualTo(XYZ.BasisX);

    public GridItem(Grid? grid)
    {
        Grid = grid;
    }

    public double DistanceTo(XYZ? point)
    {
        Curve curve = this.Grid.Curve;
        var plane = Plane.CreateByNormalAndOrigin(new XYZ(0, 0, 1), XYZ.Zero);
        Curve newcurve = ProjectLineOnPlane(plane, curve);
        XYZ p = ProjectPointOnPlane(plane, point);
        // get distance curve to point
        double distance = newcurve.Distance(p);
        return distance;
    }

    XYZ ProjectPointOnPlane(Autodesk.Revit.DB.Plane plane, XYZ? point)
    {
        var origin = plane.Origin;
        var normal = plane.Normal;
        var v = point - origin;
        var d = normal.DotProduct(v);
        return point - d * normal;
    }

    Autodesk.Revit.DB.Curve ProjectLineOnPlane(Autodesk.Revit.DB.Plane plane, Autodesk.Revit.DB.Curve line)
    {
        var origin = plane.Origin;
        var normal = plane.Normal;
        var v = line.GetEndPoint(0) - origin;
        var d = normal.DotProduct(v);
        var p0 = line.GetEndPoint(0) - d * normal;
        v = line.GetEndPoint(1) - origin;
        d = normal.DotProduct(v);
        var p1 = line.GetEndPoint(1) - d * normal;
        return Autodesk.Revit.DB.Line.CreateBound(p0, p1);
    }
    XYZ? Direction(Autodesk.Revit.DB.Curve curve)
    {
        Line? line = curve as Line;
        return line?.Direction;
    }
}