using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;

namespace OpenMEPRevit.ClassModel;

[IsVisibleInDynamoLibrary(false)]
public class GridItem
{
    private readonly double Tolerance = 1.0e-9;
    public Grid? Grid { get; set; }

    public bool IsHorizontal => IsGridHorizontal();
    public bool IsVertical => IsGridVertical();
    public GridItem(Grid? grid)
    {
        Grid = grid;
    }
    public double DistanceTo(XYZ? point)
    {
        Curve curve = this.Grid!.Curve;
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
    private bool IsGridHorizontal()
    {
        Outline? extents = this.Grid?.GetExtents();
        if (extents == null)
        {
            return false;
        }
        XYZ maximumPoint = extents.MaximumPoint;
        XYZ minimumPoint = extents.MinimumPoint;
        Line line = GetMiddleLine(minimumPoint, maximumPoint);
        return line.Direction.IsAlmostEqualTo(XYZ.BasisX,Tolerance);
    }
    private bool IsGridVertical()
    {
        Outline? extents = this.Grid?.GetExtents();
        if (extents == null)
        {
            return false;
        }
        XYZ maximumPoint = extents.MaximumPoint;
        XYZ minimumPoint = extents.MinimumPoint;
        Line line = GetMiddleLine(minimumPoint, maximumPoint);
        return line.Direction.IsAlmostEqualTo(XYZ.BasisY, Tolerance);
    }
    private Line GetMiddleLine(XYZ? minPoint, XYZ? maxPoint)
    {
        double x1 = minPoint.X;
        double y1 = minPoint.Y;
        double x2 = maxPoint.X;
        double y2 = maxPoint.Y;
        double width = x2 - x1;
        double height = y2 - y1;
        if (width > height)
        {
            double midY = (y1 + y2) / 2;
            return Line.CreateBound(new XYZ(x1, midY, 0), new XYZ(x2, midY, 0));
        }
        // Vertical middle line
        double midX = (x1 + x2) / 2;
        return Line.CreateBound(new XYZ(midX, y1, 0), new XYZ(midX, y2, 0));
    }
    XYZ? Direction(Autodesk.Revit.DB.Curve curve)
    {
        Line? line = curve as Line;
        return line?.Direction;
    }
}