using System.Collections;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Dynamo.Graph.Nodes;
using OpenMEPRevit.Helpers;
using OpenMEPSandbox.Algo;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;
using Point = OpenMEPSandbox.Geometry.Point;

namespace OpenMEPRevit.Element;

/// <summary>A pipe in the Autodesk Revit MEP product.</summary>
/// <remarks>The pipe is only available in the Autodesk Revit MEP product.</remarks>
public class Pipe
{
    private Pipe()
    {
    }

    /// <summary>
    /// create new pipe with connectors
    /// </summary>
    /// <param name="pipeType">type of pipe</param>
    /// <param name="level">level</param>
    /// <param name="connector1">first connector</param>
    /// <param name="connector2">second connector</param>
    /// <returns name="pipe">new pipe</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.CreateByTwoConnector.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? CreateByTwoConnector(global::Revit.Elements.Element pipeType,
        global::Revit.Elements.Element level,
        Autodesk.Revit.DB.Connector connector1, Autodesk.Revit.DB.Connector connector2)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Plumbing.Pipe pipe =
            Autodesk.Revit.DB.Plumbing.Pipe.Create(doc, pipeType.InternalElement.Id, level.InternalElement.Id,
                connector1, connector2);
        TransactionManager.Instance.TransactionTaskDone();
        return pipe.ToDynamoType();
    }

    /// <summary>
    /// create new pipe with connectors
    /// </summary>
    /// <param name="pipeType">type of pipe</param>
    /// <param name="level">level</param>
    /// <param name="connector1">first connector</param>
    /// <param name="connector2">second connector</param>
    /// <param name="diameter">size of pipe</param>
    /// <returns name="pipe">new pipe</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.CreateByTwoConnectorDiameter.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? CreateByTwoConnector(global::Revit.Elements.Element pipeType,
        global::Revit.Elements.Element level,
        Autodesk.Revit.DB.Connector connector1, Autodesk.Revit.DB.Connector connector2, double diameter)
    {
        Revit.Elements.Element? element = CreateByTwoConnector(pipeType, level, connector1, connector2);
        Revit.Elements.Element? pipe = SetDiameter(element, diameter);
        return pipe;
    }

    /// <summary>
    /// create new pipe with start connector and end point
    /// </summary>
    /// <param name="pipeType">the element type of pipe</param>
    /// <param name="level">the element level</param>
    /// <param name="connector1">first connector to define first pipe draw pipe</param>
    /// <param name="endPoint">end point to draw pipe</param>
    /// <returns name="pipe">new pipe</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.CreateByConnectorAndPoint.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? CreateByConnectorAndPoint(global::Revit.Elements.Element pipeType,
        global::Revit.Elements.Element level,
        Autodesk.Revit.DB.Connector connector1, Autodesk.DesignScript.Geometry.Point endPoint)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Plumbing.Pipe pipe =
            Autodesk.Revit.DB.Plumbing.Pipe.Create(doc, pipeType.InternalElement.Id, level.InternalElement.Id,
                connector1,
                endPoint.ToXyz());
        TransactionManager.Instance.TransactionTaskDone();
        return pipe.ToDynamoType();
    }

    /// <summary>
    /// create new pipe with start connector and end point
    /// </summary>
    /// <param name="pipeType">the element type of pipe</param>
    /// <param name="level">the element level</param>
    /// <param name="connector1">first connector to define first pipe draw pipe</param>
    /// <param name="endPoint">end point to draw pipe</param>
    /// <param name="diameter">size of new pipe</param>
    /// <returns name="pipe">new pipe</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.CreateByConnectorAndPointDiameter.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? CreateByConnectorAndPoint(global::Revit.Elements.Element pipeType,
        global::Revit.Elements.Element level,
        Autodesk.Revit.DB.Connector connector1, Autodesk.DesignScript.Geometry.Point endPoint, double diameter)
    {
        Revit.Elements.Element? element = CreateByConnectorAndPoint(pipeType, level, connector1, endPoint);
        Revit.Elements.Element? pipe = SetDiameter(element, diameter);
        return pipe;
    }

    /// <summary>
    /// create new pipe with two points
    /// </summary>
    /// <param name="systemType">The Element of the piping system type.</param>
    /// <param name="pipeType">The Element of the pipe type.</param>
    /// <param name="level">The Element level.</param>
    /// <param name="startPoint">The start point of the pipe.</param>
    /// <param name="endPoint">The end point of the pipe.</param>
    /// <returns name="pipe">new pipe</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.CreateByTwoPoint.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? CreateByTwoPoint(global::Revit.Elements.Element systemType,
        global::Revit.Elements.Element pipeType,
        global::Revit.Elements.Element level, Autodesk.DesignScript.Geometry.Point startPoint,
        Autodesk.DesignScript.Geometry.Point endPoint)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Plumbing.Pipe pipe = Autodesk.Revit.DB.Plumbing.Pipe.Create(doc, new ElementId(systemType.Id),
            new ElementId(pipeType.Id),
            new ElementId(level.Id), startPoint.ToXyz(), endPoint.ToXyz());
        TransactionManager.Instance.TransactionTaskDone();
        return pipe.ToDynamoType();
    }

    /// <summary>
    /// create new pipe with two points
    /// </summary>
    /// <param name="systemType">The Element of the piping system type.</param>
    /// <param name="pipeType">The Element of the pipe type.</param>
    /// <param name="level">The Element level.</param>
    /// <param name="startPoint">The start point of the pipe.</param>
    /// <param name="endPoint">The end point of the pipe.</param>
    /// <param name="diameter">size of new pipe</param>
    /// <returns name="pipe">new pipe</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.CreateByTwoPointDiameter.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? CreateByTwoPoint(global::Revit.Elements.Element systemType,
        global::Revit.Elements.Element pipeType,
        global::Revit.Elements.Element level, Autodesk.DesignScript.Geometry.Point startPoint,
        Autodesk.DesignScript.Geometry.Point endPoint, double diameter)
    {
        Revit.Elements.Element? pipe = CreateByTwoPoint(systemType, pipeType, level, startPoint, endPoint);
        Revit.Elements.Element? newPipe = SetDiameter(pipe, diameter);
        return newPipe;
    }

    /// <summary>
    /// Create pipe shortest route by the list points use TravellingSalesman algorithm
    /// </summary>
    /// <param name="systemType">system type of pipe</param>
    /// <param name="pipeType">type of pipe</param>
    /// <param name="level">level of pipe</param>
    /// <param name="points">list point ordered of pipe</param>
    /// <param name="diameter">value diameter for create pipe</param>
    /// <returns name="elements">the elements include new pipe and new elbow of route</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.CreateRouteShortestByPoints.png)
    /// [](../OpenMEPPage/element/dyn/Pipe.CreateRouteShortestByPoints.dyn)
    /// </example>
    [NodeSearchTags("create", "pipe", "network", "list", "route")]
    [NodeCategory("Create")]
    public static List<global::Revit.Elements.Element?> CreateRouteShortestByPoints(global::Revit.Elements.Element systemType,
        global::Revit.Elements.Element pipeType,
        global::Revit.Elements.Element level, List<Autodesk.DesignScript.Geometry.Point> points, double diameter)
    {
        List<global::Revit.Elements.Element?> elements = new List<global::Revit.Elements.Element?>();
        if (points.Count == 0) throw new Exception("List point is empty");
        if (points.Count == 1) throw new Exception("List point is not enough to create a route");
        var routePoints = TravellingSalesman.FindShortestRoute(points);
        for (int i = 0; i < routePoints.Count - 1; i++)
        {
            Autodesk.DesignScript.Geometry.Point startPoint = routePoints[i];
            Autodesk.DesignScript.Geometry.Point endPoint = routePoints[i + 1];
            var newPipe = CreateByTwoPoint(systemType, pipeType, level, startPoint, endPoint);
            SetDiameter(newPipe, diameter);
            elements.Add(newPipe);
        }
        return elements;
    }

    /// <summary>
    /// create new pipe by direction and length
    /// </summary>
    /// <param name="systemType">The Element of the piping system type.</param>
    /// <param name="pipeType">The Element of the pipe type.</param>
    /// <param name="level">The Element level.</param>
    /// <param name="startPoint">The start point of the pipe.</param>
    /// <param name="length"></param>
    /// <param name="diameter">size of new pipe</param>
    /// <param name="direction">direction of new pipe</param>
    /// <returns name="pipe">new pipe</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.CreateByPointAndDirection.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? CreateByPointAndDirection(global::Revit.Elements.Element systemType,
        global::Revit.Elements.Element pipeType,
        global::Revit.Elements.Element level, Autodesk.DesignScript.Geometry.Point startPoint,
        Autodesk.DesignScript.Geometry.Vector direction, double length, double diameter)
    {
        var endPoint = Point.Offset(startPoint, length, direction);
        Revit.Elements.Element? pipe = CreateByTwoPoint(systemType, pipeType, level, startPoint, endPoint);
        Revit.Elements.Element? newPipe = SetDiameter(pipe, diameter);
        return newPipe;
    }

    /// <summary>
    /// Create a new pipe by line
    /// </summary>
    /// <param name="systemType">the element of pipe system type</param>
    /// <param name="pipeType">the element type of pipe</param>
    /// <param name="level">element level</param>
    /// <param name="line">line to draw pipe</param>
    /// <returns name="Pipe">new pipe</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.CreateByLine.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? CreateByLine(global::Revit.Elements.Element systemType,
        global::Revit.Elements.Element pipeType,
        global::Revit.Elements.Element level, Autodesk.DesignScript.Geometry.Line line)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Plumbing.Pipe pipe = Autodesk.Revit.DB.Plumbing.Pipe.Create(doc, new ElementId(systemType.Id),
            new ElementId(pipeType.Id),
            new ElementId(level.Id), line.StartPoint.ToXyz(), line.EndPoint.ToXyz());
        TransactionManager.Instance.TransactionTaskDone();
        return pipe.ToDynamoType();
    }

    /// <summary>
    /// Create a new pipe by line
    /// </summary>
    /// <param name="systemType">the element of pipe system type</param>
    /// <param name="pipeType">the element type of pipe</param>
    /// <param name="level">element level</param>
    /// <param name="line">line to draw pipe</param>
    /// <param name="diameter">size of new pipe</param>
    /// <returns name="Pipe">new pipe</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.CreateByLineDiameter.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? CreateByLine(global::Revit.Elements.Element systemType,
        global::Revit.Elements.Element pipeType,
        global::Revit.Elements.Element level, Autodesk.DesignScript.Geometry.Line line, double diameter)
    {
        Revit.Elements.Element? element = CreateByLine(systemType, pipeType, level, line);
        Revit.Elements.Element? pipe = SetDiameter(element, diameter);
        return pipe;
    }

    /// <summary>
    /// split a pipe at a point
    /// </summary>
    /// <param name="pipe">pipe will be break</param>
    /// <param name="point">point on pipe to break</param>
    /// <returns name="pipe">new pipe has split</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.Split.png)
    /// </example>
    [Obsolete("This node is deprecated, please use the new node named 'MepCurve.BreakCurve' instead.")]
    public static global::Revit.Elements.Element? Split(global::Revit.Elements.Element pipe,
        Autodesk.DesignScript.Geometry.Point point
    )
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        ElementId elementId =
            Autodesk.Revit.DB.Plumbing.PlumbingUtils.BreakCurve(doc, pipe.InternalElement.Id, point.ToXyz());
        global::Revit.Elements.Element? dynampipe = doc.GetElement(elementId).ToDynamoType() ?? null;
        if (dynampipe == null) return null;
        TransactionManager.Instance.TransactionTaskDone();
        return dynampipe;
    }

    /// <summary>
    /// Set diameter of pipe
    /// </summary>
    /// <param name="pipe">pipe need to set</param>
    /// <param name="diameter">diameter to set</param>
    /// <returns name="pipe">pipe</returns>
    /// <exception cref="Exception"></exception>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.SetDiameter.png)
    /// </example>
    public static Revit.Elements.Element? SetDiameter(Revit.Elements.Element? pipe, double diameter)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Plumbing.Pipe? internalElement = pipe!.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        if (internalElement == null) throw new Exception("element request input is pipe");
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_PipeSize).DisplayUnits;
        double value = UnitUtils.ConvertToInternalUnits(diameter, unitTypeId);
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.PipeSize).GetUnitTypeId();
        double value = UnitUtils.ConvertToInternalUnits(diameter, unitTypeId);
#endif

        ConnectorManager.Connector.GetConnectors(pipe).ForEach(delegate(Connector c) { c!.Radius = value / 2; });
        TransactionManager.Instance.TransactionTaskDone();
        return pipe;
    }


    /// <summary>
    /// return information diameter of pipe
    /// </summary>
    /// <param name="pipe">pipe</param>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetDiameter.png)
    /// </example>
    [MultiReturn("OverallSize", "Diameter", "OutsideDiameter", "InsideDiameter")]
    public static Dictionary<string, object?> GetDiameter(global::Revit.Elements.Element pipe)
    {
        Autodesk.Revit.DB.Element internalElement = pipe.InternalElement;
        string overallSize = internalElement.get_Parameter(BuiltInParameter.RBS_REFERENCE_OVERALLSIZE).AsString();
        double diameter = internalElement.get_Parameter(BuiltInParameter.RBS_PIPE_DIAMETER_PARAM).AsDouble();
#if R20
        DisplayUnitType unitTypeId =
            internalElement.Document.GetUnits().GetFormatOptions(UnitType.UT_PipeSize).DisplayUnits;
        double value = UnitUtils.ConvertFromInternalUnits(diameter, unitTypeId);
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            internalElement.Document.GetUnits().GetFormatOptions(SpecTypeId.PipeSize).GetUnitTypeId();
        double value = UnitUtils.ConvertFromInternalUnits(diameter, unitTypeId);
#endif
        double outsideDiameter = internalElement.get_Parameter(BuiltInParameter.RBS_PIPE_OUTER_DIAMETER).AsDouble();
        double valueoutsideDiamter = UnitUtils.ConvertFromInternalUnits(outsideDiameter, unitTypeId);
        double insideDiameter = internalElement.get_Parameter(BuiltInParameter.RBS_PIPE_INNER_DIAM_PARAM).AsDouble();
        double valueinsideDiameter = UnitUtils.ConvertFromInternalUnits(insideDiameter, unitTypeId);
        return new Dictionary<string, object?>()
        {
            {"OverallSize", overallSize},
            {"Diameter", value},
            {"OutsideDiameter", valueoutsideDiamter},
            {"InsideDiameter", valueinsideDiameter}
        };
    }

    /// <summary>
    /// return Junctions family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetFamilySymbolJunctionsByRouting.png)
    /// </example>
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolJunctionsByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x!.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Pipe_Dimension).DisplayUnits;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
#endif
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.Junctions);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting?.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

    /// <summary>
    /// return Caps family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetFamilySymbolCapsByRouting.png)
    /// </example>
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolCapsByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x!.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Pipe_Dimension).DisplayUnits;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
#endif
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.Caps);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting?.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

    /// <summary>
    /// return Crosses family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetFamilySymbolCrossesByRouting.png)
    /// </example>
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolCrossesByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x!.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Pipe_Dimension).DisplayUnits;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
#endif
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.Crosses);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting?.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

    /// <summary>
    /// return Elbows family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetFamilySymbolElbowsByRouting.png)
    /// </example>
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolElbowsByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x!.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Pipe_Dimension).DisplayUnits;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
#endif
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.Elbows);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting?.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

    /// <summary>
    /// return Transitions family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetFamilySymbolTransitionsByRouting.png)
    /// </example>
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolTransitionsByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x!.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Pipe_Dimension).DisplayUnits;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
#endif
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.Transitions);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting?.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

    /// <summary>
    /// return Segments family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetFamilySymbolSegmentsByRouting.png)
    /// </example>
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolSegmentsByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x!.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Pipe_Dimension).DisplayUnits;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
#endif
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.Segments);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting?.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

    /// <summary>
    /// return Undefined family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetFamilySymbolUndefinedByRouting.png)
    /// </example>
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolUndefinedByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x!.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Pipe_Dimension).DisplayUnits;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
#endif
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.Undefined);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting?.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

    /// <summary>
    /// return MechanicalJoints family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetFamilySymbolMechanicalJointsByRouting.png)
    /// </example>
    [MultiReturn("FamilySymbol", "Family")]
    public static Dictionary<string, object?> GetFamilySymbolMechanicalJointsByRouting(
        global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x!.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Pipe_Dimension).DisplayUnits;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
#endif
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.MechanicalJoints);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting?.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }


    /// <summary>
    /// Return Family Symbol In Setting Of Pipe
    /// </summary>
    /// <param name="pipe">pipe setting</param>
    /// <param name="size">size to check</param>
    /// <param name="routingtype">type routing</param>
    /// <returns></returns>
    internal static FamilySymbol? GetSymbolByRouting(global::Revit.Elements.Element? pipe, double size,
        RoutingPreferenceRuleGroupType routingtype)
    {
        Autodesk.Revit.DB.Plumbing.Pipe? internalElement = pipe!.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        Autodesk.Revit.DB.Plumbing.PipeType? pipeType = internalElement?.PipeType;
        RoutingPreferenceManager? routePrefManager = pipeType?.RoutingPreferenceManager;
        RoutingConditions rou = new RoutingConditions(RoutingPreferenceErrorLevel.None);
        //rou.PreferredJunctionType = PreferredJunctionType.Tee;
        RoutingCondition rc = new RoutingCondition(size);
        rou.AppendCondition(rc);
        ElementId? id = routePrefManager?.GetMEPPartId(routingtype, rou);
        Autodesk.Revit.DB.Element? e = internalElement?.Document.GetElement(id);
        FamilySymbol? familySymbol = e as FamilySymbol;
        return familySymbol;
    }

    /// <summary>
    /// return system type of pipe
    /// </summary>
    /// <param name="pipe">pipe to get</param>
    /// <returns name="systemtype">system type of pipe</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.SystemType.png)
    /// </example>
    [NodeCategory("Query")]
    public static global::Revit.Elements.Element? SystemType(global::Revit.Elements.Element pipe)
    {
        if (pipe == null) throw new ArgumentNullException(nameof(pipe));
        Autodesk.Revit.DB.Plumbing.Pipe? element = pipe.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        ElementId systemTypeId = element!.get_Parameter(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM).AsElementId();
        var systemType = doc.GetElement(systemTypeId).ToDynamoType();
        return systemType;
    }

    /// <summary>
    /// Return Tee Of Pipe In Routing Preferences
    /// </summary>
    /// <param name="pipe">pipe</param>
    /// <returns name="familyType">tee</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetTee.png)
    /// </example>
    public static global::Revit.Elements.Element? GetTee(global::Revit.Elements.Element pipe)
    {
        Autodesk.Revit.DB.Plumbing.Pipe? pipeInternalElement = pipe.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        FamilySymbol familySymbol = pipeInternalElement!.PipeType.Tee;
        global::Revit.Elements.Element? tee = familySymbol.ToDynamoType();
        return tee;
    }

    /// <summary>
    /// Return Union Of Pipe In Routing Preferences
    /// </summary>
    /// <param name="pipe">pipe</param>
    /// <returns name="familyType">union</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetUnion.png)
    /// </example>
    public static global::Revit.Elements.Element? GetUnion(global::Revit.Elements.Element pipe)
    {
        Autodesk.Revit.DB.Plumbing.Pipe? pipeInternalElement = pipe.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        FamilySymbol familySymbol = pipeInternalElement!.PipeType.Union;
        global::Revit.Elements.Element? union = familySymbol.ToDynamoType();
        return union;
    }

    /// <summary>
    /// Return Cross Of Pipe In Routing Preferences
    /// </summary>
    /// <param name="pipe">pipe</param>
    /// <returns name="familyType">cross</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetCross.png)
    /// </example>
    public static global::Revit.Elements.Element? GetCross(global::Revit.Elements.Element pipe)
    {
        Autodesk.Revit.DB.Plumbing.Pipe? pipeInternalElement = pipe.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        FamilySymbol familySymbol = pipeInternalElement!.PipeType.Cross;
        global::Revit.Elements.Element? cross = familySymbol.ToDynamoType();
        return cross;
    }

    /// <summary>
    /// Return Tap Of Pipe In Routing Preferences
    /// </summary>
    /// <param name="pipe">pipe</param>
    /// <returns name="familyType">tap</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetTap.png)
    /// </example>
    public static global::Revit.Elements.Element? GetTap(global::Revit.Elements.Element pipe)
    {
        Autodesk.Revit.DB.Plumbing.Pipe? pipeInternalElement = pipe.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        FamilySymbol familySymbol = pipeInternalElement!.PipeType.Tap;
        if (familySymbol == null) return null;
        global::Revit.Elements.Element? tap = familySymbol.ToDynamoType();
        return tap;
    }

    /// <summary>
    /// Return Transition Of Pipe In Routing Preferences
    /// </summary>
    /// <param name="pipe">pipe</param>
    /// <returns name="familyType">transition</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetTransition.png)
    /// </example>
    public static global::Revit.Elements.Element? GetTransition(global::Revit.Elements.Element pipe)
    {
        Autodesk.Revit.DB.Plumbing.Pipe? pipeInternalElement = pipe.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        if (pipeInternalElement == null) return null;
        FamilySymbol? familySymbol = pipeInternalElement.PipeType.Transition;
        if (familySymbol == null) return null;
        global::Revit.Elements.Element? Transition = familySymbol.ToDynamoType();
        return Transition;
    }

    /// <summary>
    /// Return The roughness of the MEP curve type
    /// </summary>
    /// <param name="pipe">pipe</param>
    /// <returns name="roughness">roughness</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.GetRoughness.png)
    /// </example>
    public static double GetRoughness(global::Revit.Elements.Element pipe)
    {
        Autodesk.Revit.DB.Plumbing.Pipe? pipeInternalElement = pipe.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        double value = pipeInternalElement!.PipeType.Roughness;
        return value;
    }

    /// <summary>
    /// return The shape of the profile.
    /// </summary>
    /// <param name="pipe">pipe to get shape</param>
    /// <returns name="connectorProfileType">connectorProfileType</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.Shape.png)
    /// </example>
    [NodeCategory("Query")]
    public static dynamic? Shape(global::Revit.Elements.Element pipe)
    {
        Autodesk.Revit.DB.Plumbing.Pipe? pipeInternalElement = pipe.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        ConnectorProfileType? value = pipeInternalElement?.PipeType.Shape;
        return value;
    }

    /// <summary>
    /// The system of the MEP curve. 
    /// </summary>
    /// <param name="pipe">pipe</param>
    /// <returns name="MEPSystem">Returns the system of this MEP curve</returns>
    ///<remarks>
    /// If the curve does not belong to any systems, the value will be <see langword="null" />.
    /// If the curve belongs to more than one system, the first available value is returned. </remarks>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.MEPSystem.png)
    /// </example>
    [NodeCategory("Query")]
    public static global::Revit.Elements.Element? MEPSystem(global::Revit.Elements.Element pipe)
    {
        Autodesk.Revit.DB.Plumbing.Pipe? pipeInternalElement = pipe.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        global::Revit.Elements.Element? MepSystem = pipeInternalElement?.MEPSystem.ToDynamoType();
        return MepSystem;
    }

    /// <summary>Updates the associated system type for the pipe.</summary>
    /// <remarks>
    ///    If the pipe previously did not have a system associated to it, this will create a new system.
    /// </remarks>
    /// <param name="pipe">pipe to set system type</param>
    /// <param name="systemTypeId">The ElementId of the piping system type.</param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The systemTypeId is not valid piping system type.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <since>2015</since>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.SetSystemType.png)
    /// </example>
    public static global::Revit.Elements.Element SetSystemType(global::Revit.Elements.Element pipe, int systemTypeId)
    {
        Autodesk.Revit.DB.Plumbing.Pipe? pipeInternalElement = pipe.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        pipeInternalElement?.SetSystemType(new ElementId(systemTypeId));
        TransactionManager.Instance.TransactionTaskDone();
        return pipe;
    }

    /// <summary>
    ///    Places caps on the open connectors of the pipe curve, pipe fitting or pipe accessory.
    /// </summary>
    /// <remarks>
    ///    In order to place the cap, the cap type should be defined in the routing preferences that associates with the pipe type of the given element.
    ///    If the typeId is a valid element id, it will be used to override the pipe type that associates with the pipe type of the given element.
    /// </remarks>
    /// <param name="pipe">Element of pipe curve, pipe fitting or pipe accessory.</param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The element elemId does not exist in the document
    ///    -or-
    ///    The element elemId is neither an object of pipe curve, pipe fitting, nor pipe accessory.
    ///    -or-
    ///    The element elemId has no opened piping connector.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    ///    this operation failed.
    /// </exception>
    /// <since>2014</since>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Pipe.EndCap.png)
    /// </example>
    public static void EndCap(Revit.Elements.Element pipe)
    {
        if (pipe == null) throw new ArgumentNullException(nameof(pipe));
        Autodesk.Revit.DB.Plumbing.Pipe? internalElement = pipe.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        PlumbingUtils.PlaceCapOnOpenEnds(doc, internalElement?.Id, internalElement?.GetTypeId());
        TransactionManager.Instance.TransactionTaskDone();
    }
}