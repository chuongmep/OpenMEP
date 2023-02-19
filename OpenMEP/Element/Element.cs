using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;
using Revit.GeometryConversion;
using Point = Autodesk.DesignScript.Geometry.Point;

namespace OpenMEP.Element;

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
    /// <summary>
    /// Returns the Document in which the Element resides
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    [NodeCategory("Query")]
    [MultiReturn("Revit Document", "Dynamo Document")]
    public static Dictionary<string,object?> GetDocument(global::Revit.Elements.Element element)
    {
        return new Dictionary<string, object?>
        {
            {"Revit Document", element.InternalElement.Document},
            {"Dynamo Document", element.InternalElement.Document.ToDynamoType()}
        };
    }
}