using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;
using Level = Autodesk.Revit.DB.Level;
using Point = Autodesk.DesignScript.Geometry.Point;

namespace OpenMEP.Element;

public class Element
{
    private Element()
    {
    }

    /// <summary>
    /// Return A Location Of Element
    /// </summary>
    /// <param name="element"></param>
    /// <returns name="point">location of element</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.GetLocation.png)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Point? GetLocation(Revit.Elements.Element? element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element));
        if (element.InternalElement.Location is LocationPoint)
        {
            LocationPoint? lc = element.InternalElement.Location as LocationPoint;
            return lc?.Point.ToPoint();
        }
        if (element.InternalElement.Location is LocationCurve)
        {
            LocationCurve? lc = element.InternalElement.Location as LocationCurve;
            return lc?.Curve.Evaluate(0.5, false).ToPoint();
        }
        BoundingBoxXYZ bb = element.InternalElement.get_BoundingBox(null);
        return bb.Max.Add(bb.Min).Divide(0.5).ToPoint();
    }

    /// <summary>
    /// Returns the Document in which the Element resides
    /// </summary>
    /// <param name="element">the element</param>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.GetDocument.png)
    /// </example>
    [NodeCategory("Query")]
    [MultiReturn("Revit Document", "Dynamo Document")]
    public static Dictionary<string, object?> GetDocument(global::Revit.Elements.Element element)
    {
        return new Dictionary<string, object?>
        {
            {"Revit Document", element.InternalElement.Document},
            {"Dynamo Document", element.InternalElement.Document.ToDynamoType()}
        };
    }

    /// <summary>
    /// Move element to new location
    /// </summary>
    /// <param name="element">element to move</param>
    /// <param name="newLocation">translate</param>
    /// <returns name="element">family instance</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.MoveElement.png)
    /// </example>
    public static global::Revit.Elements.Element MoveElement(global::Revit.Elements.Element element, Point newLocation)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        ElementTransformUtils.MoveElement(doc, element.InternalElement.Id,
            newLocation.ToXyz().Subtract(GetLocation(element).ToXyz()));
        TransactionManager.Instance.TransactionTaskDone();
        return element;
    }
    
    /// <summary>
    /// Return Level Of Element
    /// </summary>
    /// <param name="element">element to get level</param>
    /// <returns name="level">level of element</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.GetLevel.png)
    /// </example>
    public static global::Revit.Elements.Element? GetLevel(global::Revit.Elements.Element element)
    {
        return GetLevel(element.InternalElement).ToDynamoType();
    }

    private static Level? GetLevel(Autodesk.Revit.DB.Element element)
    {
        var doc = element.Document;
        ElementId levelId = element.LevelId;
        if (doc.GetElement(levelId) == null)
        {
            var levelPara = element.get_Parameter(BuiltInParameter.RBS_START_LEVEL_PARAM);
            if (levelPara == null)
            {
                levelPara = element.get_Parameter(BuiltInParameter.SCHEDULE_LEVEL_PARAM);
            }
            if (levelPara != null)
            {
                levelId = levelPara.AsElementId();
            }
            if (levelId.IntegerValue == -1)
            {
                // General get level method
                var bbox = element.get_BoundingBox(null);
                double zValue = (bbox.Min.Z + bbox.Max.Z) / 2;

                FilteredElementCollector levelFilter = new FilteredElementCollector(doc);
                List<Level> levels = levelFilter.OfClass(typeof(Level)).ToElements().Cast<Level>().ToList();
                levels.Sort((a, b) => a.Elevation.CompareTo(b.Elevation));
                if (zValue <= levels.FirstOrDefault()?.Elevation)
                {
                    return levels.FirstOrDefault();
                }

                if (zValue >= levels.LastOrDefault()?.Elevation)
                {
                    return levels.LastOrDefault();
                }

                for (int i = 0; i < levels.Count; i++)
                {
                    if (levels[i].Elevation >= zValue)
                    {
                        return levels[i - 1];
                    }
                }
            }
        }

        return doc.GetElement(levelId) as Autodesk.Revit.DB.Level;
    }
    
    /// <summary>
    /// The system of the MEP element belong to.
    /// </summary>
    /// <param name="element">the element to get system</param>
    /// <returns name="system">mep system of element</returns>
    public static global::Revit.Elements.Element? System(global::Revit.Elements.Element element)
    {
        List<Connector?> connectors = ConnectorManager.Connector.GetConnectors(element);
        if (connectors == null) throw new ArgumentNullException(nameof(element));
        return connectors.Select(x => x!.MEPSystem.ToDynamoType()).FirstOrDefault();
    }

    /// <summary>
    /// return type of MEP System
    /// </summary>
    /// <param name="element">the element of mep</param>
    /// <returns name="systemType">system type of element from connector</returns>
    public static global::Revit.Elements.Element SystemType(global::Revit.Elements.Element element)
    {
        global::Revit.Elements.Element? system = System(element);
        return system!.ElementType;
    }

    /// <summary>
    /// Returns the MEP System Type from connectors of the element
    /// </summary>
    /// <param name="element">the element of mep</param>
    /// <returns name="systemType">system type from connector of element</returns>
    public static dynamic? ConnectorSystemType(global::Revit.Elements.Element element)
    {
        List<Connector?> connectors = ConnectorManager.Connector.GetConnectors(element);
        dynamic? systemType = connectors.Select(x => ConnectorManager.Connector.SystemType(x)).FirstOrDefault();
        return systemType;
    }

    

}