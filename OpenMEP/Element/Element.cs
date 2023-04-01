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

/// <summary>Base class for most persistent data within a Revit document.</summary>
/// <remarks>
///    The data in a Revit document consists primarily of a collection of
///    elements.  An element usually corresponds to a single component of a
///    building or drawing, such as a wall, door, or dimension, but it can
///    also be something more abstract, like a wall type or a view.
///    Every element in a document has a unique ID, represented by the
///    ElementId class.
/// </remarks>
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
    /// <returns name="element">the element moved</returns>
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
    /// Move the list collection of elements to new location
    /// </summary>
    /// <param name="elements">element to move</param>
    /// <param name="newLocation">translate</param>
    /// <returns name="elements">the collection of elements moved</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.MoveElements.png)
    /// </example>
    public static List<Revit.Elements.Element> MoveElements(List<Revit.Elements.Element> elements, Point newLocation)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        foreach (var element in elements)
        {
            ElementTransformUtils.MoveElement(doc, element.InternalElement.Id,
                newLocation.ToXyz().Subtract(GetLocation(element).ToXyz()));
        }
        TransactionManager.Instance.TransactionTaskDone();
        return elements;
    }

    /// <summary>
    /// Set Rotate of family instances by line
    /// </summary>
    /// <param name="element">the element</param>
    /// <param name="lineAxis">Line Axis</param>
    /// <param name="angle">angle to rotate(Degrees)</param>
    /// <returns name="element">family instance</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.RotateByLine.png)
    /// </example>
    [NodeCategory("Action")]
    public static global::Revit.Elements.Element Rotate(global::Revit.Elements.Element element,
        Autodesk.DesignScript.Geometry.Line lineAxis,
        double angle)
    {
        TransactionManager.Instance.ForceCloseTransaction();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        double degree2Radian = angle * Math.PI / 180;
        ElementTransformUtils.RotateElement(doc, element.InternalElement.Id,
            (Autodesk.Revit.DB.Line) lineAxis.ToRevitType(), degree2Radian);
        TransactionManager.Instance.TransactionTaskDone();
        return element;
    }
    
    /// <summary>
    /// Set Rotate multiple family instances
    /// This will be help save time when you have a lot of elements to rotate because just one transaction
    /// </summary>
    /// <param name="elements">the list collection of elements</param>
    /// <param name="lineAxis">Line Axis</param>
    /// <param name="angle">angle to rotate(Degrees)</param>
    /// <returns name="elements">collection of list elements rotated</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.RotateMultipleByLine.png)
    /// </example>
    [NodeCategory("Action")]
    public static List<Revit.Elements.Element> RotateMultiple(List<Revit.Elements.Element> elements,
        Autodesk.DesignScript.Geometry.Line lineAxis,
        double angle)
    {
        TransactionManager.Instance.ForceCloseTransaction();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        double degree2Radian = angle * Math.PI / 180;
        foreach (var element in elements)
        {
            ElementTransformUtils.RotateElement(doc, element.InternalElement.Id,
                (Autodesk.Revit.DB.Line) lineAxis.ToRevitType(), degree2Radian);
        }
        TransactionManager.Instance.TransactionTaskDone();
        return elements;
    }

    /// <summary>
    /// Set Rotate multiple elements by vector
    /// </summary>
    /// <param name="elements">the collection of elements</param>
    /// <param name="vectorAxis">Direction Axis</param>
    /// <param name="angle">angle to rotate(Degrees)</param>
    /// <returns name="elements">collection of list elements rotated</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.RotateMultiple.png)
    /// </example>
    [NodeCategory("Action")]
    public static List<Revit.Elements.Element> RotateMultiple(List<Revit.Elements.Element> elements,
        Autodesk.DesignScript.Geometry.Vector vectorAxis,
        double angle)
    {
        TransactionManager.Instance.ForceCloseTransaction();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        double degree2Radian = angle * Math.PI / 180;
        foreach (var element in elements)
        {
            LocationPoint? locationPoint = element.InternalElement.Location as LocationPoint;
            var location = locationPoint?.Point;
            Autodesk.Revit.DB.Line line =
                Autodesk.Revit.DB.Line.CreateBound(location!.Add(vectorAxis.ToRevitType().Multiply(2)), location);
            ElementTransformUtils.RotateElement(doc, element.InternalElement.Id,
                (Autodesk.Revit.DB.Line) line, degree2Radian);
        }
        TransactionManager.Instance.TransactionTaskDone();
        return elements;
    }
    
    /// <summary>
    /// Set Rotate of fitting By Vector
    /// </summary>
    /// <param name="element">the element</param>
    /// <param name="vectorAxis">Direction Axis</param>
    /// <param name="angle">angle to rotate(Degrees)</param>
    /// <returns name="fitting">family instance</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.RotateByDirection.png)
    /// </example>
    [NodeCategory("Action")]
    public static global::Revit.Elements.Element Rotate(global::Revit.Elements.Element element,
        Autodesk.DesignScript.Geometry.Vector vectorAxis,
        double angle)
    {
        TransactionManager.Instance.ForceCloseTransaction();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        double degree2Radian = angle * Math.PI / 180;
        LocationPoint? locationPoint = element.InternalElement.Location as LocationPoint;
        var location = locationPoint?.Point;
        Autodesk.Revit.DB.Line line =
            Autodesk.Revit.DB.Line.CreateBound(location!.Add(vectorAxis.ToRevitType().Multiply(2)), location);
        ElementTransformUtils.RotateElement(doc, element.InternalElement.Id,
            (Autodesk.Revit.DB.Line) line, degree2Radian);
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
    public static global::Revit.Elements.Element? GetLevel(global::Revit.Elements.Element? element)
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        Level? level = GetLevel(element.InternalElement);
        if (level == null) return null;
        return level.ToDynamoType();
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
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.System.png)
    /// </example>
    public static global::Revit.Elements.Element? System(global::Revit.Elements.Element element)
    {
        List<Connector> connectors = ConnectorManager.Connector.GetConnectors(element);
        if (connectors == null) throw new ArgumentNullException(nameof(element));
        foreach (var connector in connectors)
        {
            if (connector == null) continue;
            if (connector.MEPSystem != null)
            {
                return connector.MEPSystem.ToDynamoType();
            }
        }

        return null;
    }

    /// <summary>
    /// return type of MEP System
    /// </summary>
    /// <param name="element">the element of mep</param>
    /// <returns name="systemType">system type of element from connector</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.SystemType.png)
    /// </example>
    public static global::Revit.Elements.Element? SystemType(global::Revit.Elements.Element element)
    {
        global::Revit.Elements.Element? system = System(element);
        if (system == null) return null;
        return system!.ElementType;
    }

    /// <summary>
    /// Returns the MEP System Type from connectors of the element
    /// </summary>
    /// <param name="element">the element of mep</param>
    /// <returns name="systemType">system type from connector of element</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.ConnectorSystemType.png)
    /// </example>
    public static dynamic? ConnectorSystemType(global::Revit.Elements.Element element)
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        List<Connector> connectors = ConnectorManager.Connector.GetConnectors(element);
        if (connectors.Count==0) return null;
        dynamic? systemType = connectors.Select(x => ConnectorManager.Connector.SystemType(x)).FirstOrDefault();
        return systemType;
    }

    /// <summary>
    /// This method takes in an element and returns a list of elements that are connected to it.
    /// </summary>
    /// <param name="elem">the element need get connected</param>
    /// <returns name="elements">the list elements connected with this element</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.GetConnectedElements.png)
    /// </example>
    public static List<Revit.Elements.Element> GetConnectedElements(Revit.Elements.Element elem)
    {
        List<Autodesk.Revit.DB.Element> connectedElements = new List<Autodesk.Revit.DB.Element>();
        Autodesk.Revit.DB.Element internalElement = elem.InternalElement;
        if (internalElement == null) throw new ArgumentNullException(nameof(elem));
        List<Connector> connectors = ConnectorManager.Connector.GetConnectors(elem);
        if (connectors.Count == 0) return new List<Revit.Elements.Element>();
        foreach (var connector in connectors)
        {
            if(connector== null) continue;
            ConnectorSet connectedConnectors = connector.AllRefs;
            foreach (Connector connectedConnector in connectedConnectors)
            {
                Autodesk.Revit.DB.Element connectedElem = connectedConnector.Owner;
                if (connectedElem.Id != internalElement.Id && !connectedElements.Contains(connectedElem))
                {
                    connectedElements.Add(connectedElem);
                }
            }
        }
        if (connectedElements.Count == 0) return new List<Revit.Elements.Element>();
        return connectedElements.Select(x => x.ToDynamoType()).ToList();
    }
}