using Autodesk.Revit.DB;
using Revit.GeometryConversion;
using Point = Autodesk.DesignScript.Geometry.Point;

namespace Element;

public class Element
{
    private Element()
    {
        
    }
    /// <summary>
    /// Return A Location Center Of Element
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static Point LocationCenter(Revit.Elements.Element? element)
    {
        if (element.InternalElement.Location is LocationPoint)
        {
            LocationPoint? lc = element.InternalElement.Location as LocationPoint;
            return lc.Point.ToPoint();
        }
        if (element.InternalElement.Location is LocationCurve)
        {
            LocationCurve? lc = element.InternalElement.Location as LocationCurve;
            return lc.Curve.Evaluate(0.5,false).ToPoint();
        }
        BoundingBoxXYZ bb = element.InternalElement.get_BoundingBox(null);
        return bb.Max.Add(bb.Min).Divide(0.5).ToPoint();
    } 
}