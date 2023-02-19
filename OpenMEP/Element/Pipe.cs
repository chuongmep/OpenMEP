using System.Collections;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;

namespace OpenMEP.Element;

public class Pipe
{
    private Pipe(){}
    /// <summary>
    /// create new pipe with connectors
    /// </summary>
    /// <param name="pipeType">type of pipe</param>
    /// <param name="level">level</param>
    /// <param name="connector1">first connector</param>
    /// <param name="connector2">second connector</param>
    /// <returns name="pipe">new pipe</returns>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? Create(global::Revit.Elements.Element pipeType, global::Revit.Elements.Element level,
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
    /// create new pipe with start connector and end point
    /// </summary>
    /// <param name="pipeType">the element type of pipe</param>
    /// <param name="level">the element level</param>
    /// <param name="connector1">first connector to define first pipe draw pipe</param>
    /// <param name="endPoint">end point to draw pipe</param>
    /// <returns name="pipe">new pipe</returns>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? Create(global::Revit.Elements.Element pipeType, global::Revit.Elements.Element level,
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
    /// create new pipe with connectors
    /// </summary>
    /// <param name="systemType">The Element of the piping system type.</param>
    /// <param name="pipeType">The Element of the pipe type.</param>
    /// <param name="level">The Element level.</param>
    /// <param name="startPoint">The start point of the pipe.</param>
    /// <param name="endPoint">The end point of the pipe.</param>
    /// <returns name="pipe">new pipe</returns>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? Create(global::Revit.Elements.Element systemType,
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
    /// Create a new pipe by line
    /// </summary>
    /// <param name="systemType">the element of pipe system type</param>
    /// <param name="pipeType">the element type of pipe</param>
    /// <param name="level">element level</param>
    /// <param name="line">line to draw pipe</param>
    /// <returns name="Pipe">new pipe</returns>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? Create(global::Revit.Elements.Element systemType,
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
    /// split a pipe at a point
    /// </summary>
    /// <param name="pipe">pipe will be break</param>
    /// <param name="point">point on pipe to break</param>
    /// <returns name="pipe">new pipe has split</returns>
    public static global::Revit.Elements.Element? Split(global::Revit.Elements.Element pipe, Autodesk.DesignScript.Geometry.Point point
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

#if !R20
     /// <summary>
    /// return information diameter of pipe
    /// </summary>
    /// <param name="pipe">pipe</param>
    [MultiReturn("OverallSize", "Diameter", "OutsideDiameter", "InsideDiameter")]
    public static Dictionary<string, object?> GetDiameter(global::Revit.Elements.Element pipe)
    {
        Autodesk.Revit.DB.Element internalElement = pipe.InternalElement;
        string overallSize = internalElement.get_Parameter(BuiltInParameter.RBS_REFERENCE_OVERALLSIZE).AsString();
        double diameter = internalElement.get_Parameter(BuiltInParameter.RBS_PIPE_DIAMETER_PARAM).AsDouble();
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            internalElement.Document.GetUnits().GetFormatOptions(SpecTypeId.PipeSize).GetUnitTypeId();
        double value = UnitUtils.ConvertFromInternalUnits(diameter, unitTypeId);
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
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolJunctionsByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.Junctions);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

    /// <summary>
    /// return Caps family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolCapsByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.Caps);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

    /// <summary>
    /// return Crosses family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolCrossesByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.Crosses);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

    /// <summary>
    /// return Elbows family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolElbowsByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.Elbows);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

    /// <summary>
    /// return Transitions family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolTransitionsByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius =ConnectorManager.Connector.GetConnectors(pipe).Select(x => x.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.Transitions);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

    /// <summary>
    /// return Segments family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolSegmentsByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.Segments);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

    /// <summary>
    /// return Undefined family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolUndefinedByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.Undefined);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

    /// <summary>
    /// return MechanicalJoints family of pipe get in routing setting
    /// </summary>
    /// <remarks>it will be help you get correct family junction in setting map with size of pipe</remarks>
    /// <param name="pipe">pipe to get family routing</param>
    /// <returns name="familysymbol">family symbol</returns>
    [MultiReturn("FamilySymbol", "Family")]
    public static IDictionary? GetFamilySymbolMechanicalJointsByRouting(global::Revit.Elements.Element? pipe)
    {
        double radius = ConnectorManager.Connector.GetConnectors(pipe).Select(x => x.Radius).FirstOrDefault();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.PipeDimension).GetUnitTypeId();
        double raInternalUnits = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        FamilySymbol? symbolByRouting =
            GetSymbolByRouting(pipe, raInternalUnits * 2, RoutingPreferenceRuleGroupType.MechanicalJoints);
        return new Dictionary<string, object?>()
        {
            {"FamilySymbol", symbolByRouting.ToDynamoType() ?? null},
            {"Family", symbolByRouting?.Family.ToDynamoType() ?? null}
        };
    }

#endif
    
   
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
        PipeType? pipeType = internalElement?.PipeType;
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
    ///  Return three points closest inside three pipes
    /// </summary>
    /// <param name="pipe1">first pipe</param>
    /// <param name="pipe2">second pipe</param>
    /// <param name="pipe3">three pipe</param>
    /// <returns name="connectors">return an collection list connectors closet check by 3 pipe</returns>
    [MultiReturn("Connector1", "Connector2", "Connector3")]
    public static IDictionary<string, object?> GetThreeConnectorsClosest(global::Revit.Elements.Element? pipe1,
        global::Revit.Elements.Element? pipe2, global::Revit.Elements.Element? pipe3)
    {
        List<Connector?> connectorsPipe1 = ConnectorManager.Connector.GetConnectors(pipe1);
        if (!connectorsPipe1.Any()) throw new ArgumentException(nameof(pipe1));
        List<Connector?> connectorsPipe2 = ConnectorManager.Connector.GetConnectors(pipe2);
        if (!connectorsPipe2.Any()) throw new ArgumentException(nameof(pipe2));
        List<Connector?> connectorsPipe3 = ConnectorManager.Connector.GetConnectors(pipe3);
        if (!connectorsPipe3.Any()) throw new ArgumentException(nameof(pipe3));
        Connector? c2 = ConnectorManager.Connector.GetConnectorClosest(pipe1, pipe2);
        Connector? c1 = ConnectorManager.Connector.GetConnectorClosest(pipe2, pipe1);
        Connector? c3 = ConnectorManager.Connector.GetConnectorClosest(pipe3, pipe1);
        return new Dictionary<string, object?>()
        {
            {"Connector1", c1},
            {"Connector2", c2},
            {"Connector3", c3}
        };
    }

    /// <summary>
    /// return two connectors closet of two pipes
    /// </summary>
    /// <param name="pipe1">the first pipe</param>
    /// <param name="pipe2">the second pipe</param>
    /// <returns></returns>
    [MultiReturn("Connector1", "Connector2")]
    public static Dictionary<string, object?> GetTwoConnectorClosest(global::Revit.Elements.Element pipe1,
        global::Revit.Elements.Element pipe2)
    {
        Connector? connector1 = ConnectorManager.Connector.GetConnectorClosest(pipe1,pipe2);
        Connector? connector2 =ConnectorManager.Connector.GetConnectorClosest(pipe2,pipe1);
        return new Dictionary<string, object?>()
        {
            {"Connector1", connector1},
            {"Connector2", connector2}
        };
    }

    /// <summary>
    /// Return Tee Of Pipe In Routing Preferences
    /// </summary>
    /// <param name="pipe">pipe</param>
    /// <returns name="tee">tee</returns>
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
    /// <returns name="union">union</returns>
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
    /// <returns name="cross">cross</returns>
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
    /// <returns name="tap">tap</returns>
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
    /// <returns name="transition">transition</returns>
    public static global::Revit.Elements.Element? GetTransition(global::Revit.Elements.Element pipe)
    {
        Autodesk.Revit.DB.Plumbing.Pipe? pipeInternalElement = pipe.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        FamilySymbol? familySymbol = pipeInternalElement?.PipeType.Transition;
        global::Revit.Elements.Element? Transition = familySymbol.ToDynamoType();
        return Transition;
    }

    /// <summary>
    /// Return The roughness of the MEP curve type
    /// </summary>
    /// <param name="pipe">pipe</param>
    /// <returns name="roughness">roughness</returns>
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
    public static global::Revit.Elements.Element SetSystemType(global::Revit.Elements.Element pipe, int systemTypeId)
    {
        Autodesk.Revit.DB.Plumbing.Pipe? pipeInternalElement = pipe.InternalElement as Autodesk.Revit.DB.Plumbing.Pipe;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        pipeInternalElement?.SetSystemType(new ElementId(systemTypeId));
        TransactionManager.Instance.TransactionTaskDone();
        return pipe;
    }

    
}