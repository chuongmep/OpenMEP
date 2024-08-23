using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using OpenMEPRevit.ClassModel;
using OpenMEPRevit.Helpers;
using ProtoCore.Lang;
using Revit.Elements;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;
using Grid = Autodesk.Revit.DB.Grid;
using Level = Autodesk.Revit.DB.Level;
using Point = Autodesk.DesignScript.Geometry.Point;
using Revision = Revit.Elements.Revision;

namespace OpenMEPRevit.Element;

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
        if (element.InternalElement is IndependentTag)
        {
            XYZ min = element.BoundingBox.MinPoint.ToXyz();
            XYZ max = element.BoundingBox.MaxPoint.ToXyz();
            return min.Add(max).Divide(2).ToPoint();
        }

        if (element.InternalElement is Autodesk.Revit.DB.FabricationPart fabricationPart)
        {
            return fabricationPart.Origin.ToPoint();
        }

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

        BoundingBoxXYZ? bb = element.InternalElement.get_BoundingBox(null);
        if (bb == null)
        {
            return null;
        }

        return bb.Max.Add(bb.Min).Divide(0.5).ToPoint();
    }

    /// <summary>
    /// Get the location of the element belong to the grid line with the closest distance
    /// the location include the top, bottom, left, right grid line
    /// </summary>
    /// <param name="element">the elements</param>
    /// <returns></returns>
    [NodeCategory("Query")]
    [MultiReturn("TopGrid", "BottomGrid", "LeftGrid", "RightGrid")]
    public static Dictionary<string, object?> GetLocationGridLine(Revit.Elements.Element element)
    {
        var doc = DocumentManager.Instance.CurrentDBDocument;
        List<GridItem> grids = GetGrids(doc);
        XYZ? location = GetLocation(element)?.ToXyz();
        if (location == null)
        {
            return new Dictionary<string, object?>()
            {
                { "TopGrid", null },
                { "BottomGrid", null },
                { "LeftGrid", null },
                { "RightGrid", null }
            };
        }

        Grid? topGrid = null;
        Grid? bottomGrid = null;
        Grid? leftGrid = null;
        Grid? rightGrid = null;

        var xGrids = grids.Where(x => x.IsHorizontal).ToList();
        var yGrids = grids.Where(x => !x.IsHorizontal).ToList();
        // top
        double locationY = location.Y;
        double yDistance = double.MaxValue;
        foreach (var item in xGrids)
        {
            double yCurrent = item.Grid.Curve.GetEndPoint(0).Y;
            double distanceTo = item.DistanceTo(location);
            if (yCurrent >= locationY && distanceTo <= yDistance)
            {
                yDistance = distanceTo;
                topGrid = item.Grid;
            }
        }

        // bottom
        yDistance = double.MaxValue;
        foreach (var item in xGrids)
        {
            double yCurrent = item.Grid.Curve.GetEndPoint(0).Y;
            double distanceTo = item.DistanceTo(location);
            if (yCurrent <= locationY && distanceTo <= yDistance)
            {
                yDistance = distanceTo;
                bottomGrid = item.Grid;
            }
        }

        // left
        double locationX = location.X;
        double xDistance = double.MaxValue;
        foreach (var item in yGrids)
        {
            double xCurrent = item.Grid.Curve.GetEndPoint(0).X;
            double distanceTo = item.DistanceTo(location);
            if (xCurrent <= locationX && distanceTo <= xDistance)
            {
                xDistance = distanceTo;
                leftGrid = item.Grid;
            }
        }

        // right
        xDistance = double.MaxValue;
        foreach (var item in yGrids)
        {
            double xCurrent = item.Grid.Curve.GetEndPoint(0).X;
            double distanceTo = item.DistanceTo(location);
            if (xCurrent >= locationX && distanceTo <= xDistance)
            {
                xDistance = distanceTo;
                rightGrid = item.Grid;
            }
        }

        return new Dictionary<string, object?>()
        {
            { "TopGrid", topGrid?.ToDynamoType() },
            { "BottomGrid", bottomGrid?.ToDynamoType() },
            { "LeftGrid", leftGrid?.ToDynamoType() },
            { "RightGrid", rightGrid?.ToDynamoType() }
        };
    }

    /// <summary>
    /// Get the closest grid line to the element, return the list closest grid line with 4 direction:
    /// e.g: top, bottom, left, right
    /// </summary>
    /// <param name="element"></param>
    /// <param name="grids"></param>
    /// <returns></returns>
    [NodeCategory("Query")]
    [MultiReturn("TopGrids", "BottomGrids", "LeftGrids", "RightGrids")]
    public static Dictionary<string, object?> GetClosestGridLine(Revit.Elements.Element element,
        List<Revit.Elements.Element> grids)
    {
        List<Grid?> topGrids = null;
        List<Grid?> bottomGrids = null;
        List<Grid?> leftGrids = null;
        List<Grid?> rightGrids = null;
        // group top : from element to top view plan
        XYZ location = GetLocation(element).ToXyz();
        if (location == null)
        {
            return new Dictionary<string, object?>()
            {
                { "TopGrids", null },
                { "BottomGrids", null },
                { "LeftGrids", null },
                { "RightGrids", null }
            };
        }
        List<GridItem> gridItems = GetGrids(DocumentManager.Instance.CurrentDBDocument);
        var xGrids = gridItems.Where(x => x.IsHorizontal).ToList();
        var yGrids = gridItems.Where(x => x.IsVertical).ToList();
        topGrids = xGrids.Where(x => x?.Grid?.Curve.GetEndPoint(0).Y >= location.Y)
            .OrderBy(x => x.DistanceTo(location)).Select(x => x.Grid).ToList();
        bottomGrids = xGrids.Where(x => x?.Grid?.Curve.GetEndPoint(0).Y <= location.Y)
            .OrderBy(x => x.DistanceTo(location)).Select(x => x.Grid).ToList();
        leftGrids = yGrids.Where(x => x?.Grid?.Curve.GetEndPoint(0).X <= location.X)
            .OrderBy(x => x.DistanceTo(location)).Select(x => x.Grid).ToList();
        rightGrids = yGrids.Where(x => x?.Grid?.Curve.GetEndPoint(0).X >= location.X)
            .OrderBy(x => x.DistanceTo(location)).Select(x => x.Grid).ToList();
        return new Dictionary<string, object?>()
        {
            { "TopGrids", topGrids.Select(x => x?.ToDynamoType()).ToList() },
            { "BottomGrids", bottomGrids.Select(x => x?.ToDynamoType()).ToList() },
            { "LeftGrids", leftGrids.Select(x => x?.ToDynamoType()).ToList() },
            { "RightGrids", rightGrids.Select(x => x?.ToDynamoType()).ToList() }
        };
    }

    /// <summary>
    /// Get the location of the element belong to the grid line with the closest distance
    /// the location include the top, bottom, left, right grid line
    /// </summary>
    /// <param name="element">the elements</param>
    /// <param name="grids">list grids need to check with</param>
    /// <returns></returns>
    [NodeCategory("Query")]
    [MultiReturn("TopGrid", "BottomGrid", "LeftGrid", "RightGrid")]
    public static Dictionary<string, object?> GetLocationGridLine(Revit.Elements.Element element,List<Revit.Elements.Element> grids)
    {
        var gridItems = grids.Select(x => new GridItem(x.InternalElement as Grid)).ToList();
        XYZ? location = GetLocation(element)?.ToXyz();
        if (location == null)
        {
            return new Dictionary<string, object?>()
            {
                { "TopGrid", null },
                { "BottomGrid", null },
                { "LeftGrid", null },
                { "RightGrid", null }
            };
        }

        Grid? topGrid = null;
        Grid? bottomGrid = null;
        Grid? leftGrid = null;
        Grid? rightGrid = null;

        var xGrids = gridItems.Where(x => x.IsHorizontal).ToList();
        var yGrids = gridItems.Where(x => !x.IsHorizontal).ToList();
        // top
        double locationY = location.Y;
        double yDistance = double.MaxValue;
        foreach (var item in xGrids)
        {
            double yCurrent = item.Grid.Curve.GetEndPoint(0).Y;
            double distanceTo = item.DistanceTo(location);
            if (yCurrent >= locationY && distanceTo <= yDistance)
            {
                yDistance = distanceTo;
                topGrid = item.Grid;
            }
        }

        // bottom
        yDistance = double.MaxValue;
        foreach (var item in xGrids)
        {
            double yCurrent = item.Grid.Curve.GetEndPoint(0).Y;
            double distanceTo = item.DistanceTo(location);
            if (yCurrent <= locationY && distanceTo <= yDistance)
            {
                yDistance = distanceTo;
                bottomGrid = item.Grid;
            }
        }

        // left
        double locationX = location.X;
        double xDistance = double.MaxValue;
        foreach (var item in yGrids)
        {
            double xCurrent = item.Grid.Curve.GetEndPoint(0).X;
            double distanceTo = item.DistanceTo(location);
            if (xCurrent <= locationX && distanceTo <= xDistance)
            {
                xDistance = distanceTo;
                leftGrid = item.Grid;
            }
        }

        // right
        xDistance = double.MaxValue;
        foreach (var item in yGrids)
        {
            double xCurrent = item.Grid.Curve.GetEndPoint(0).X;
            double distanceTo = item.DistanceTo(location);
            if (xCurrent >= locationX && distanceTo <= xDistance)
            {
                xDistance = distanceTo;
                rightGrid = item.Grid;
            }
        }

        return new Dictionary<string, object?>()
        {
            { "TopGrid", topGrid?.ToDynamoType() },
            { "BottomGrid", bottomGrid?.ToDynamoType() },
            { "LeftGrid", leftGrid?.ToDynamoType() },
            { "RightGrid", rightGrid?.ToDynamoType() }
        };
    }

    private static List<GridItem> GetGrids(Autodesk.Revit.DB.Document doc)
    {
        List<GridItem> gridItems = new List<GridItem>();
        FilteredElementCollector collector = new FilteredElementCollector(doc);
        List<Grid?> grids = collector.OfClass(typeof(Grid)).Cast<Autodesk.Revit.DB.Grid>().ToList();
        foreach (Grid? grid in grids)
        {
            gridItems.Add(new GridItem(grid));
        }

        return gridItems;
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
            { "Revit Document", element.InternalElement.Document },
            { "Dynamo Document", element.InternalElement.Document.ToDynamoType() }
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
    [NodeCategory("Action")]
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
    /// <param name="elements">the collection elements want move</param>
    /// <param name="newLocations">the collection translate</param>
    /// <returns name="elements">the collection of elements moved</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.MoveElements.png)
    /// </example>
    [NodeCategory("Action")]
    public static List<Revit.Elements.Element> MoveElements(List<Revit.Elements.Element> elements,
        List<Point> newLocations)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        if (elements.Count != newLocations.Count)
            throw new Exception("The number of elements and new locations must be the same.");
        for (int i = 0; i < elements.Count; i++)
        {
            ElementTransformUtils.MoveElement(doc, elements[i].InternalElement.Id,
                newLocations[i].ToXyz().Subtract(GetLocation(elements[i]).ToXyz()));
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
            (Autodesk.Revit.DB.Line)lineAxis.ToRevitType(), degree2Radian);
        TransactionManager.Instance.TransactionTaskDone();
        return element;
    }

    /// <summary>
    /// Set Rotate multiple family instances
    /// This will be help save time when you have a lot of elements to rotate because just one transaction
    /// </summary>
    /// <param name="elements">the list collection of elements</param>
    /// <param name="lineAxis">the collection line Axis</param>
    /// <param name="angles">the collection angle to rotate(Degrees)</param>
    /// <returns name="elements">collection of list elements rotated</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.RotateMultipleByLine.png)
    /// </example>
    [NodeCategory("Action")]
    public static List<Revit.Elements.Element> RotateMultiple(List<Revit.Elements.Element> elements,
        List<Autodesk.DesignScript.Geometry.Line> lineAxis,
        List<double> angles)
    {
        if (elements.Count != lineAxis.Count)
            throw new Exception("The number of elements and line axis must be the same");
        TransactionManager.Instance.ForceCloseTransaction();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        for (int i = 0; i < elements.Count; i++)
        {
            double degree2Radian = angles[i] * Math.PI / 180;
            ElementTransformUtils.RotateElement(doc, elements[i].InternalElement.Id,
                (Autodesk.Revit.DB.Line)lineAxis[i].ToRevitType(), degree2Radian);
        }

        TransactionManager.Instance.TransactionTaskDone();
        return elements;
    }

    /// <summary>
    /// Set Rotate multiple elements by vector
    /// </summary>
    /// <param name="elements">the collection of elements</param>
    /// <param name="vectorAxis">the collection of direction Axis</param>
    /// <param name="angles">the collection angle to rotate(Degrees)</param>
    /// <returns name="elements">collection of list elements rotated</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Element.RotateMultiple.png)
    /// </example>
    [NodeCategory("Action")]
    public static List<Revit.Elements.Element> RotateMultiple(List<Revit.Elements.Element> elements,
        List<Autodesk.DesignScript.Geometry.Vector> vectorAxis,
        List<double> angles)
    {
        if (elements.Count != vectorAxis.Count)
            throw new Exception("The number of elements and vector axis must be the same");
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);

        for (int i = 0; i < elements.Count; i++)
        {
            double degree2Radian = angles[i] * Math.PI / 180;
            LocationPoint? locationPoint = elements[i].InternalElement.Location as LocationPoint;
            var location = locationPoint?.Point;
            Autodesk.Revit.DB.Line line =
                Autodesk.Revit.DB.Line.CreateBound(location!.Add(vectorAxis[i].ToRevitType().Multiply(2)), location);
            ElementTransformUtils.RotateElement(doc, elements[i].InternalElement.Id,
                (Autodesk.Revit.DB.Line)line, degree2Radian);
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
            (Autodesk.Revit.DB.Line)line, degree2Radian);
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
#if R20 || R21 || R22 || R23
            if (levelId.IntegerValue == -1)

#else
            if (levelId.Value == -1)
#endif
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
        if (connectors.Count == 0) return null;
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
            if (connector == null) continue;
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