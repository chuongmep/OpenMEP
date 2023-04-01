using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;
using Revit.Elements;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;
using FamilyInstance = Autodesk.Revit.DB.FamilyInstance;
using MEPCurve = Autodesk.Revit.DB.MEPCurve;
using Point = Autodesk.DesignScript.Geometry.Point;

namespace OpenMEP.ConnectorManager;

/// <summary>A connector in an Autodesk Revit MEP project document.</summary>
/// <remarks>
///    This connector is an item that is a part of another element (duct, pipe, fitting, or equipment etc.).
///    This connector does not represent the connector element that can be created inside a family;
///    for that element, refer to <see cref="T:Autodesk.Revit.DB.ConnectorElement" />.
/// </remarks>
public class Connector
{
    private Connector()
    {
    }

    /// <summary>
    /// return the radius of connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="radius">radius</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Radius.png)
    /// </example>
    public static double Radius(Autodesk.Revit.DB.Connector connector)
    {
        double radius = connector.Radius;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Length).DisplayUnits;
        double value = UnitUtils.ConvertToInternalUnits(radius, unitTypeId);
        return value;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();
        double value = UnitUtils.ConvertFromInternalUnits(radius, unitTypeId);
        return value;
#endif
    }

    /// <summary>
    ///  Return Connector Closet With Connector Current
    /// </summary>
    /// <param name="point">origin</param>
    /// <param name="connectors">list connector to check</param>
    /// <returns name="connector">connector</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetConnectorClosest.png)
    /// </example>
    public static Autodesk.Revit.DB.Connector? GetConnectorClosest(Point point,
        List<Autodesk.Revit.DB.Connector> connectors)
    {
        Autodesk.Revit.DB.Connector? closet = null;
        double? distance = Double.MaxValue;
        foreach (Autodesk.Revit.DB.Connector? connector in connectors)
        {
            double? distanceTo = DistanceTo(connector, point);
            if (distanceTo < distance)
            {
                closet = connector;
                distance = distanceTo;
            }
        }

        return closet;
    }

    /// <summary>
    ///  Return Connector Closet With Connector Current
    /// </summary>
    /// <param name="c">first connector</param>
    /// <param name="connectors">an collection connectors to check</param>
    /// <returns name="connector">closet connector</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetConnectorClosest.png)
    /// </example>
    public static Autodesk.Revit.DB.Connector? GetConnectorClosest(Autodesk.Revit.DB.Connector? c,
        List<Autodesk.Revit.DB.Connector> connectors)
    {
        Autodesk.Revit.DB.Connector? closet = null;
        double? distance = Double.MaxValue;
        foreach (Autodesk.Revit.DB.Connector? connector in connectors)
        {
            double? distanceTo = DistanceTo(connector, c);
            if (distanceTo < distance)
            {
                closet = connector;
                distance = distanceTo;
            }
        }

        return closet;
    }

    /// <summary>
    /// Return Closet Connector between element1 from element2
    /// </summary>
    /// <param name="element1">first element</param>
    /// <param name="element2">second element</param>
    /// <returns name="connector">closet connector of element1</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetConnectorClosest.png)
    /// </example>
    public static Autodesk.Revit.DB.Connector? GetConnectorClosest(Revit.Elements.Element? element1,
        Revit.Elements.Element? element2)
    {
        ConnectorSet? connectorSet = GetConnectorSet(element1);
        if (connectorSet == null)
        {
            return null;
        }

        Autodesk.Revit.DB.Connector? connector = GetConnectorClosest(element2, connectorSet);
        return connector;
    }

    /// <summary>
    /// return connector set of element
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetConnectorSet.png)
    /// </example>
    public static ConnectorSet? GetConnectorSet(Revit.Elements.Element? element)
    {
        Autodesk.Revit.DB.Element e = element!.InternalElement;
        if (e is MEPCurve)
            return ((MEPCurve) e)?
                .ConnectorManager?
                .Connectors;

        if (e is FamilyInstance)
        {
            return ((FamilyInstance) e)?
                .MEPModel?
                .ConnectorManager?
                .Connectors;
        }
        if (e is FabricationPart)
        {
            return ((FabricationPart) e)?
                .ConnectorManager?
                .Connectors;
        }
        return null;
    }

    /// <summary>
    /// Get Closet Connector With Element
    /// </summary>
    /// <param name="element">element to check</param>
    /// <param name="connectorSet">an collection connectors</param>
    /// <returns name="connector">closet connector</returns>
    internal static Autodesk.Revit.DB.Connector? GetConnectorClosest(Revit.Elements.Element? element,
        ConnectorSet? connectorSet)
    {
        Autodesk.Revit.DB.Connector? closet = null;
        Point? locationCenter = global::OpenMEP.Element.Element.GetLocation(element);
        double distance = Double.MaxValue;
        foreach (Autodesk.Revit.DB.Connector? connector in connectorSet!)
        {
            if (locationCenter != null)
            {
                double distanceTo = locationCenter.DistanceTo(connector!.Origin.ToPoint());
                if (distanceTo <= distance)
                {
                    closet = connector;
                    distance = distanceTo;
                }
            }
        }

        return closet;
    }

    /// <summary>
    /// return connector farthest with point current
    /// </summary>
    /// <param name="point"></param>
    /// <param name="connectors"></param>
    /// <returns></returns>
    public static Autodesk.Revit.DB.Connector? GetConnectorFarthest(Point? point,
        List<Autodesk.Revit.DB.Connector?> connectors)
    {
        Autodesk.Revit.DB.Connector? Farthest = null;
        double? distance = Double.MinValue;
        foreach (Autodesk.Revit.DB.Connector? connector in connectors)
        {
            double? distanceTo = DistanceTo(connector, point);
            if (distanceTo > distance)
            {
                Farthest = connector;
                distance = distanceTo;
            }
        }

        return Farthest;
    }

    /// <summary>
    /// return connector farthest with connector current
    /// </summary>
    /// <param name="c">origin connector</param>
    /// <param name="connectors">an collection connector to check</param>
    /// <returns name="connector">connector</returns>
    public static Autodesk.Revit.DB.Connector? GetConnectorFarthest(Autodesk.Revit.DB.Connector? c,
        List<Autodesk.Revit.DB.Connector?> connectors)
    {
        Autodesk.Revit.DB.Connector? Farthest = null;
        double? distance = Double.MinValue;
        foreach (Autodesk.Revit.DB.Connector? connector in connectors)
        {
            double? distanceTo = DistanceTo(connector, c);
            if (distanceTo > distance)
            {
                Farthest = connector;
                distance = distanceTo;
            }
        }

        return Farthest;
    }

    /// <summary>
    /// Return Farthest Connector between element1 with element2
    /// </summary>
    /// <param name="element1">first element</param>
    /// <param name="element2">second element</param>
    /// <returns name="connector">Farthest connector of element1</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetConnectorFarthest.png)
    /// </example>
    public static Autodesk.Revit.DB.Connector? GetConnectorFarthest(Revit.Elements.Element? element1,
        Revit.Elements.Element? element2)
    {
        if (element1 == null) throw new ArgumentNullException(nameof(element1));
        if (element2 == null) throw new ArgumentNullException(nameof(element2));
        List<Autodesk.Revit.DB.Connector> connectorSet = GetConnectors(element1);
        Autodesk.Revit.DB.Connector? connector = GetConnectorFarthest(element2, connectorSet);
        return connector;
    }

    /// <summary>
    /// Get Farthest Connector With Element
    /// </summary>
    /// <param name="element">element to check</param>
    /// <param name="connectors">an collection connectors</param>
    /// <returns name="connector">farthest connector</returns>
    internal static Autodesk.Revit.DB.Connector? GetConnectorFarthest(Revit.Elements.Element element,
        List<Autodesk.Revit.DB.Connector> connectors)
    {
        Autodesk.Revit.DB.Connector? farthest = null;
        Point? locationCenter = global::OpenMEP.Element.Element.GetLocation(element);
        double distance = Double.MinValue;
        foreach (Autodesk.Revit.DB.Connector? connector in connectors!)
        {
            if (locationCenter != null)
            {
                double distanceTo = locationCenter.DistanceTo(connector!.Origin.ToPoint());
                if (distanceTo >= distance)
                {
                    farthest = connector;
                    distance = distanceTo;
                }
            }
        }

        return farthest;
    }

    /// <summary>
    /// return list connector from connector manager
    /// </summary>
    /// <param name="connectorManager">connector manager</param>
    /// <returns name="connectors">connectors</returns>
    public static List<Autodesk.Revit.DB.Connector> GetConnectors(
        Autodesk.Revit.DB.ConnectorManager connectorManager)
    {
        List<Autodesk.Revit.DB.Connector> connectors = new List<Autodesk.Revit.DB.Connector>();
        foreach (Autodesk.Revit.DB.Connector? c in connectorManager.Connectors)
        {
            if(c == null) continue;
            connectors.Add(c);
        }
        return connectors;
    }

    /// <summary>
    /// return list connector from element
    /// </summary>
    /// <param name="element">element</param>
    /// <returns name="connectors">connectors</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetConnectors.png)
    /// </example>
    public static List<Autodesk.Revit.DB.Connector> GetConnectors(Revit.Elements.Element? element)
    {
        if (element == null) throw new ArgumentException(nameof(element));
        Autodesk.Revit.DB.ConnectorManager? connectorManager =
            global::OpenMEP.ConnectorManager.ConnectorManager.GetConnectorManager(element);
        if (connectorManager == null) return new List<Autodesk.Revit.DB.Connector>();
        return GetConnectors(connectorManager);
    }

    /// <summary>
    /// return list connector unused from element
    /// </summary>
    /// <param name="element">element</param>
    /// <returns name="connectors">unused connectors</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetUnusedConnectors.png)
    /// </example>
    public static List<Autodesk.Revit.DB.Connector?> GetUnusedConnectors(Revit.Elements.Element? element)
    {
        Autodesk.Revit.DB.ConnectorManager? connectorManager =
            global::OpenMEP.ConnectorManager.ConnectorManager.GetConnectorManager(element);
        if (connectorManager == null) throw new ArgumentNullException(nameof(connectorManager));
        return GetUnusedConnectors(connectorManager);
    }

    /// <summary>
    /// return list connector used from element
    /// </summary>
    /// <param name="element">element</param>
    /// <returns name="connectors">used connectors</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetUsedConnectors.png)
    /// </example>
    public static List<Autodesk.Revit.DB.Connector> GetUsedConnectors(Revit.Elements.Element? element)
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        List<Autodesk.Revit.DB.Connector> connectors = GetConnectors(element);
        if (!connectors.Any()) return new List<Autodesk.Revit.DB.Connector>();
        return connectors.Where(x => x!.IsConnected).ToList();
    }

    /// <summary>
    /// return list connector from element
    /// </summary>
    /// <param name="connectorManager">connector manager</param>
    /// <returns name="connectors">connectors</returns>
    public static List<Autodesk.Revit.DB.Connector?> GetUnusedConnectors(
        Autodesk.Revit.DB.ConnectorManager? connectorManager)
    {
        if (connectorManager == null) throw new ArgumentNullException(nameof(connectorManager));
        return connectorManager.UnusedConnectors.Cast<Autodesk.Revit.DB.Connector>().ToList()!;
    }
    
    /// <summary>
    /// return all connectors of element except connector same id with connector input
    /// </summary>
    /// <param name="element">the element</param>
    /// <param name="connector">connector</param>
    /// <returns name="connectors">list of connectors</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetRemainingConnector.png)
    /// </example>
    public static List<Autodesk.Revit.DB.Connector> GetRemainingConnector(Revit.Elements.Element? element ,Autodesk.Revit.DB.Connector connector)
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        if (connector == null) throw new ArgumentNullException(nameof(connector));
        List<Autodesk.Revit.DB.Connector> connectors = GetConnectors(element);
        if (connectors.Count == 0) return new List<Autodesk.Revit.DB.Connector>();
        return connectors.Where(c => c!.Id != connector.Id).ToList();
    }

    /// <summary>
    /// return all connectors of list connectors except connector same id with connector input
    /// </summary>
    /// <param name="connectors">list connectors need to check</param>
    /// <param name="connector">connector need to remove</param>
    /// <returns name="connectors">list of connectors</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetRemainingConnector2.png)
    /// </example>
    public static List<Autodesk.Revit.DB.Connector> GetRemainingConnector(List<Autodesk.Revit.DB.Connector> connectors,Autodesk.Revit.DB.Connector connector)
    {
        return connectors.Where(c => c!.Id != connector.Id).ToList();
    }

    /// <summary>
    /// return system type of connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="domain">domain</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.SystemType.png)
    /// </example>
    public static dynamic? SystemType(Autodesk.Revit.DB.Connector connector)
    {
        Domain domain = connector.Domain;
        if (domain == Autodesk.Revit.DB.Domain.DomainHvac)
        {
            return connector.DuctSystemType;
        }

        if (domain == Autodesk.Revit.DB.Domain.DomainPiping)
        {
            return connector.PipeSystemType;
        }

        if (domain == Autodesk.Revit.DB.Domain.DomainElectrical)
        {
            return connector.ElectricalSystemType;
        }

        return null;
    }

    /// <summary>
    /// Get direction of connector
    /// </summary>
    /// <param name="connector"></param>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Direction.png)
    /// </example>
    public static Point? Direction(Autodesk.Revit.DB.Connector? connector)
    {
        Transform? t = connector?.CoordinateSystem;
        if (t == null) return null;
        return Autodesk.DesignScript.Geometry.Point.ByCoordinates(t.BasisX.X, t.BasisX.Y, t.BasisX.Z);
    }
    
    /// <summary>
    /// Get direction BasisZ of connector
    /// </summary>
    /// <param name="connector"></param>
    /// <returns></returns>
    public static Point? GetDirection(Autodesk.Revit.DB.Connector? connector)
    {
        Transform? t = connector?.CoordinateSystem;
        if (t == null) return null;
        return Autodesk.DesignScript.Geometry.Point.ByCoordinates(t.BasisX.X, t.BasisX.Y, t.BasisX.Z);
    }

    /// <summary>
    /// Get origin of connector
    /// </summary>
    /// <param name="connector">the connector</param>
    /// <returns name="point">location origin of connector</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Origin.png)
    /// </example>
    public static Point? Origin(Autodesk.Revit.DB.Connector? connector)
    {
        return connector?.Origin?.ToPoint();
    }

    /// <summary>
    /// check connector is connected or not
    /// </summary>
    /// <param name="connector">the connector</param>
    /// <returns name="bool">true if connector is connected</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.IsConnected.png)
    /// </example>
    [NodeCategory("Query")]
    public static bool? IsConnected(Autodesk.Revit.DB.Connector? connector)
    {
        return connector?.IsConnected;
    }


    /// <summary>
    /// return distance between one connector with another point
    /// </summary>
    /// <param name="connector">the connector</param>
    /// <param name="point">point to get distance from this to the connector</param>
    /// <returns name="double">distance from connector to point</returns>
    public static double? DistanceTo(Autodesk.Revit.DB.Connector? connector,
        Autodesk.DesignScript.Geometry.Point? point)
    {
        return connector?.Origin.DistanceTo(point.ToXyz()) ?? null;
    }

    /// <summary>
    /// return distance between one connector with another connector
    /// </summary>
    /// <param name="connector1"></param>
    /// <param name="connector2"></param>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.DistanceTo.png)
    /// </example>
    public static double? DistanceTo(Autodesk.Revit.DB.Connector? connector1,
        Autodesk.Revit.DB.Connector? connector2)
    {
        XYZ? origin = connector1?.Origin;
        XYZ? target = connector2?.Origin;
        double? distanceTo = origin?.DistanceTo(target);
        return distanceTo;
    }

    /// <summary>
    /// The host of the connector.
    /// The element that contains  connector. It may also contain other connectors.
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="element">element</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Owner.png)
    /// </example>
    public static Revit.Elements.Element Owner(Autodesk.Revit.DB.Connector connector)
    {
        Revit.Elements.Element dsType = connector.Owner.ToDSType(true);
        return dsType;
    }

    /// <summary>
    /// return id of connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="double">Id of connector</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Id.png)
    /// </example>
    public static double Id(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Id;
    }

    /// <summary>
    /// return angle of connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="double">radian</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Angle.png)
    /// </example>
    public static double Angle(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Angle;
    }

    /// <summary>
    /// The coefficient of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">Coefficient</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Coefficient.png)
    /// </example>
    public static double Coefficient(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Coefficient;
    }

    /// <summary>
    /// The demand of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">Demand</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Demand.png)
    /// </example>
    public static double Demand(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Demand;
    }

    /// <summary>
    /// The Flow of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">Flow of connector</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Flow.png)
    /// </example>
    public static double Flow(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Flow;
    }

    /// <summary>
    /// The height of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">Height</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Height.png)
    /// </example>
    public static double Height(Autodesk.Revit.DB.Connector connector)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Length).DisplayUnits;
        double value = UnitUtils.ConvertToInternalUnits(connector.Height, unitTypeId);
        return value;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();
        double value = UnitUtils.ConvertFromInternalUnits(connector.Height, unitTypeId);
        return value;
#endif
    }

    /// <summary>
    /// The Width of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">the width of connector</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Width.png)
    /// </example>
    public static double Width(Autodesk.Revit.DB.Connector connector)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Length).DisplayUnits;
        double value = UnitUtils.ConvertToInternalUnits(connector.Width, unitTypeId);
        return value;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();
        double value = UnitUtils.ConvertFromInternalUnits(connector.Width, unitTypeId);
        return value;
#endif
    }

    /// <summary>
    /// The assigned flow of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">AssignedFlow</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.AssignedFlow.png)
    /// </example>
    public static double AssignedFlow(Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedFlow;
    }

    /// <summary>
    /// Connector engagement length
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">Connector engagement length</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.EngagementLength.png)
    /// </example>
    public static double EngagementLength(Autodesk.Revit.DB.Connector connector)
    {
        return connector.EngagementLength;
    }

    /// <summary>
    /// The pressure drop of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">PressureDrop</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.PressureDrop.png)
    /// </example>
    public static double PressureDrop(Autodesk.Revit.DB.Connector connector)
    {
        return connector.PressureDrop;
    }

    /// <summary>
    /// All references of the connector.
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="connectors">A set of connectors that the connectors is connected to, including both physical connection and logical connection.</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.AllRefs.png)
    /// </example>
    public static List<Autodesk.Revit.DB.Connector> AllRefs(Autodesk.Revit.DB.Connector connector)
    {
        return connector.AllRefs.Cast<Autodesk.Revit.DB.Connector>().ToList();
    }

    /// <summary>
    /// The domain of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="Autodesk.Revit.DB.Domain">Domain</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Domain.png)
    /// </example>
    public static dynamic Domain(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Domain;
    }

    /// <summary>
    /// The velocity pressure of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">VelocityPressure</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.VelocityPressure.png)
    /// </example>
    public static double VelocityPressure(Autodesk.Revit.DB.Connector connector)
    {
        return connector.VelocityPressure;
    }

    /// <summary>
    /// The assigned fixture units of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">AssignedFixtureUnits</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.AssignedFixtureUnits.png)
    /// </example>
    public static double AssignedFixtureUnits(Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedFixtureUnits;
    }

    /// <summary>
    /// The assigned flow factor of  connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">AssignedFlowFactor</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.AssignedFlowFactor.png)
    /// </example>
    public static double AssignedFlowFactor(Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedFlowFactor;
    }

    /// <summary>
    /// The assigned kCoefficient of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">AssignedKCoefficient</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.AssignedKCoefficient.png)
    /// </example>
    public static double AssignedKCoefficient(Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedKCoefficient;
    }

    /// <summary>
    /// return element connected with  connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="element">element has connected with connector</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetElementConnectedWith.png)
    /// </example>
    public static Revit.Elements.Element? GetElementConnectedWith(Autodesk.Revit.DB.Connector? connector)
    {
        if (connector == null) throw new ArgumentNullException(nameof(connector));
        if (connector.IsConnected)
        {
            return connector.AllRefs.Cast<Autodesk.Revit.DB.Connector>()
                .Where(x => x.Owner.Id != connector.Owner.Id)
                .Where(x => x.ConnectorType != ConnectorType.Logical)
                .Select(x => x.Owner.ToDynamoType()).FirstOrDefault();
        }

        return null;
    }
    
    /// <summary>
    /// Return All Element Connected Continuous In Branch
    /// Be careful, because this node require recursive to check all connected.
    /// </summary>
    /// <param name="element">element</param>
    /// <returns name="elements">list element connected Continuous from element</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetElementConnectedContinuous.png)
    /// </example>
    public static List<Revit.Elements.Element> GetElementConnectedContinuous(Revit.Elements.Element? element)
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        Dictionary<string, Revit.Elements.Element> OutElements =
            new Dictionary<string, Revit.Elements.Element>();
        List<Revit.Elements.Element> collector = Collector(element, OutElements);
        return collector;
    }
    /// <summary>
    /// Return All Element Connected Continuous In Branch
    /// Be careful, because this node require recursive to check all connected.
    /// </summary>
    /// <param name="connector">element</param>
    /// <returns name="elements">list element connected Continuous from element</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetElementConnectedContinuous.png)
    /// </example>
    public static List<Revit.Elements.Element> GetElementConnectedContinuous(Autodesk.Revit.DB.Connector connector)
    {
        if (connector == null) throw new ArgumentNullException(nameof(connector));
        Dictionary<string, Revit.Elements.Element> OutElements =
            new Dictionary<string, Revit.Elements.Element>();
        List<Revit.Elements.Element> collector = Collector(connector.Owner.ToDynamoType(), OutElements);
        return collector;
    }
    //recursive element from e to end branch side by side other
    private static List<Revit.Elements.Element> Collector(Revit.Elements.Element? e,
        Dictionary<string, Revit.Elements.Element> OutElements)
    {
        double count = 0;
        List<Revit.Elements.Element?> elements = GetConnectors(e).Select(GetElementConnectedWith).ToList();
        foreach (var item in elements)
        {
            if(item==null) continue;
            if (OutElements.ContainsKey(item.Id.ToString()))
            {
                count += 1;
            }
            else
            {
                OutElements[item.Id.ToString()] = item;
                Collector(item, OutElements);
            }
        }
        if (System.Math.Abs(count - elements.Count) < 0.0001)
        {
            return new List<Revit.Elements.Element>(OutElements.Values);
        }
        return new List<Revit.Elements.Element>(OutElements.Values);
    }
    
    /// <summary>
    /// return element connected with  connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="element">element has connected with connector</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetConnectorConnectedWith.png)
    /// </example>
    public static Autodesk.Revit.DB.Connector? GetConnectorConnectedWith(Autodesk.Revit.DB.Connector connector)
    {
        if (connector.IsConnected)
        {
            return connector.AllRefs.Cast<Autodesk.Revit.DB.Connector>().First();
        }

        return null;
    }

    /// <summary>
    /// The assigned loss coefficient of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">AssignedLossCoefficient</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.AssignedLossCoefficient.png)
    /// </example>
    public static double AssignedLossCoefficient(Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedLossCoefficient;
    }

    /// <summary>
    /// The assigned pressure drop of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">AssignedPressureDrop</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.AssignedPressureDrop.png)
    /// </example>
    public static double AssignedPressureDrop(Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedPressureDrop;
    }

    /// <summary>
    /// The description of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="string">Description</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Description.png)
    /// </example>
    public static string Description(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Description;
    }

    /// <summary>
    /// The shape of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="ConnectorProfileType">ConnectorProfileType</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Shape.png)
    /// </example>
    public static dynamic Shape(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Shape;
    }


    /// <summary>
    /// Return CoordinateSystem of the connector.
    /// </summary>
    /// <param name="connector"></param>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.CoordinateSystem.png)
    /// </example>
    [MultiReturn("Origin", "BasisX", "BasisY", "BasisZ", "Determinant", "Scale", "HasReflection", "IsConformal",
        "IsTranslation", "IsIdentity")]
    public static Dictionary<string, object?> CoordinateSystem(Autodesk.Revit.DB.Connector connector)
    {
        if (connector == null) throw new ArgumentNullException(nameof(connector));
        Transform transform;
        Point? origin = null;
        Vector? BasisX = null;
        Vector? BasisY = null;
        Vector? BasisZ = null;
        double? determinant = null;
        double? scale = null;
        bool? hasReflection = null;
        bool? isConformal = null;
        bool? isTranslation = null;
        bool? isIdentity = null;
        try
        {
            transform = connector.CoordinateSystem;
            origin = transform.Origin.ToPoint() ?? null;
            BasisX = transform.BasisX.ToVector() ?? null;
            BasisY = transform.BasisY.ToVector() ?? null;
            BasisZ = transform.BasisZ.ToVector() ?? null;
            determinant = transform.Determinant;
            scale = transform.Scale;
            hasReflection = transform.HasReflection;
            isConformal = transform.IsConformal;
            isTranslation = transform.IsTranslation;
            isIdentity = transform.IsIdentity;
            return new Dictionary<string, object?>()
            {
                {"Origin", origin},
                {"BasisX", BasisX},
                {"BasisY", BasisY},
                {"BasisZ", BasisZ},
                {"Determinant", determinant},
                {"Scale", scale},
                {"HasReflection", hasReflection},
                {"IsConformal", isConformal},
                {"IsTranslation", isTranslation},
                {"IsIdentity", isIdentity},
            };
        }
        catch (Exception e)
        {
            return new Dictionary<string, object?>()
            {
                {"Origin", origin},
                {"BasisX", BasisX},
                {"BasisY", BasisY},
                {"BasisZ", BasisZ},
                {"Determinant", determinant},
                {"Scale", scale},
                {"HasReflection", hasReflection},
                {"IsConformal", isConformal},
                {"IsTranslation", isTranslation},
                {"IsIdentity", isIdentity},
            };
        }
    }

    /// <summary>
    /// The connector manager of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="ConnectorManager">ConnectorManager</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.ConnectorManager.png)
    /// </example>
    public static Autodesk.Revit.DB.ConnectorManager ConnectorManager(Autodesk.Revit.DB.Connector connector)
    {
        return connector.ConnectorManager;
    }

    /// <summary>
    /// Get area of connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="double">area of connector</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetArea.png)
    /// </example>
    public static double GetArea(Autodesk.Revit.DB.Connector connector)
    {
        switch (connector.Shape)
        {
            case ConnectorProfileType.Round:
                return Math.PI * Math.Pow(connector.Radius, 2);
            case ConnectorProfileType.Rectangular:
                return connector.Width * connector.Height;
            case ConnectorProfileType.Oval:
                return Math.PI * connector.Height * connector.Width / 4;
            default:
                return 0;
        }
    }

    /// <summary>
    /// Gets MEP connector information.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="MEPConnectorInfo">Returns null if there is no MEP connector information associated</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.GetMEPConnectorInfo.png)
    /// </example>
    public static Autodesk.Revit.DB.MEPConnectorInfo GetMEPConnectorInfo(Autodesk.Revit.DB.Connector connector)
    {
        Autodesk.Revit.DB.MEPConnectorInfo mepConnectorInfo = connector.GetMEPConnectorInfo();
        return mepConnectorInfo;
    }

    /// <summary>
    /// set new angle for connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <param name="angle">angle</param>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.SetAngle.png)
    /// </example>
    public static void SetAngle(Autodesk.Revit.DB.Connector connector, double angle)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.ForceCloseTransaction();
        using Autodesk.Revit.DB.Transaction tran = new Autodesk.Revit.DB.Transaction(doc, "set angle connector");
        tran.Start();
        connector.Angle = angle;
        tran.Commit();
    }

    /// <summary>
    /// set new origin for connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <param name="origin">new point</param>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.SetOrigin.png)
    /// </example>
    public static void SetOrigin(Autodesk.Revit.DB.Connector connector, Autodesk.DesignScript.Geometry.Point origin)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.ForceCloseTransaction();
        using Autodesk.Revit.DB.Transaction tran = new Autodesk.Revit.DB.Transaction(doc, "set angle connector");
        tran.Start();
        connector.Origin = origin.ToXyz();
        tran.Commit();
    }

    /// <summary>
    /// Remove connection between two connectors.
    /// </summary>
    /// <param name="connector">connect need disconnect</param>
    /// <param name="connectorFrom">Indicate the connector, connection will be removed from.</param>
    /// <returns name="connector">connector need disconnect</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.DisConnectFrom.png)
    /// </example>
    public static Autodesk.Revit.DB.Connector DisConnectFrom(Autodesk.Revit.DB.Connector connector,
        Autodesk.Revit.DB.Connector connectorFrom)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.ForceCloseTransaction();
        using Autodesk.Revit.DB.Transaction tran = new Autodesk.Revit.DB.Transaction(doc, "disconnect connector");
        tran.Start();
        connector.DisconnectFrom(connectorFrom);
        tran.Commit();
        return connector;
    }

    /// <summary>
    /// Remove connection between two connectors.
    /// </summary>
    /// <param name="connector">connect need connect</param>
    /// <param name="connectorFrom">Indicate the connector, connection will be removed from.</param>
    /// <returns name="connector">connect need connect</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.ConnectTo.png)
    /// </example>
    public static Autodesk.Revit.DB.Connector ConnectTo(Autodesk.Revit.DB.Connector connector,
        Autodesk.Revit.DB.Connector connectorFrom)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.ForceCloseTransaction();
        TransactionManager.Instance.EnsureInTransaction(doc);
        connector.ConnectTo(connectorFrom);
        TransactionManager.Instance.TransactionTaskDone();
        return connector;
    }

    /// <summary>
    /// Shows scalable lines representing the CoordinateSystem of a Connector.
    /// </summary>
    /// <param name="connector">Autodesk.Revit.DB.Connector</param>
    /// <param name="length">double</param>
    /// <returns name="Display">GeometryColor order by x,y,z</returns>
    /// <returns name="Origin">Point</returns>
    /// <returns name="XAxis">Vector(Red color)</returns>
    /// <returns name="YAxis">Vector(Green color)</returns>
    /// <returns name="ZAxis">Vector(Blue color)</returns>
    /// <returns name="XYPlane">Plane(Red-Green color)</returns>
    /// <returns name="YZPlane">Plane(Green-Blue color)</returns>
    /// <returns name="ZXPlane">Plane(Blue-Red color)</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.Display.gif)
    /// </example>
    [MultiReturn(new[] {"Display", "Origin", "XAxis", "YAxis", "ZAxis", "XYPlane", "YZPlane", "ZXPlane"})]
    public static Dictionary<string, object?> Display(Autodesk.Revit.DB.Connector? connector, double length = 1000)
    {
        if (length <= 0)
        {
            length = 1;
        }

        if (connector == null) return new Dictionary<string, object?>();
        var origin = connector.Origin.ToDynamoType();
        if(origin == null) return new Dictionary<string, object?>();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20  
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Length).DisplayUnits;
        double xUnits = UnitUtils.ConvertFromInternalUnits(origin.X, unitTypeId);
        double yUnits = UnitUtils.ConvertFromInternalUnits(origin.Y, unitTypeId);
        double zUnits = UnitUtils.ConvertFromInternalUnits(origin.Z, unitTypeId);
        #else
        ForgeTypeId unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();
        double xUnits = UnitUtils.ConvertFromInternalUnits(origin.X, unitTypeId);
        double yUnits = UnitUtils.ConvertFromInternalUnits(origin.Y, unitTypeId);
        double zUnits = UnitUtils.ConvertFromInternalUnits(origin.Z, unitTypeId);
#endif
        
        Point point = Autodesk.DesignScript.Geometry.Point.ByCoordinates(xUnits, yUnits, zUnits);
        var X = connector.CoordinateSystem.BasisX.ToDynamoVector();
        var Y = connector.CoordinateSystem.BasisY.ToDynamoVector();
        var Z = connector.CoordinateSystem.BasisZ.ToDynamoVector();
        CoordinateSystem coordinateSystem = Autodesk.DesignScript.Geometry.CoordinateSystem.ByOriginVectors(point, X, Y, Z);
        return OpenMEPSandbox.Geometry.CoordinateSystem.Display(coordinateSystem,length);
    }

    /// <summary>
    /// Identifies if the connector is connected to the specified connector.
    /// </summary>
    /// <param name="connector">the connector to check</param>
    /// <param name="connectorOther">the second connector to identifies</param>
    /// <returns name="bool">true if connector is connected to other</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/Connector.IsConnectedTo.png)
    /// </example>
    public static bool IsConnectedTo(Autodesk.Revit.DB.Connector connector,Autodesk.Revit.DB.Connector connectorOther)
    {
        return connector.IsConnectedTo(connectorOther);
    }

    // /// <summary>Gets fabrication connectivity information.</summary>
    // /// <para name="connector">the connector</para>
    // /// <since>2016</since>
    // [MultiReturn("BodyConnectorId", "DoubleWallConnectorId", "FabricationIndex", "IsBodyConnectorLocked",
    //     "IsDoubleWallConnectorLocked", "HasDoubleWallConnector", "IsValid")]
    // public static Dictionary<string, object?> GetFabricationConnectorInfo(Autodesk.Revit.DB.Connector connector)
    // {
    //     if (connector == null) throw new ArgumentException(nameof(connector));
    //     FabricationConnectorInfo fabricationConnectorInfo = connector.GetFabricationConnectorInfo();
    //     if (fabricationConnectorInfo == null)
    //     {
    //         return new Dictionary<string, object?>
    //         {
    //             {"BodyConnectorId", null},
    //             {"DoubleWallConnectorId", null},
    //             {"FabricationIndex", null},
    //             {"IsBodyConnectorLocked", null},
    //             {"IsDoubleWallConnectorLocked", null},
    //             {"HasDoubleWallConnector", null},
    //             {"IsValid", null}
    //         };
    //     }
    //     int? DoubleWallConnectorId = null;
    //     bool? IsDoubleWallConnectorLocked = null;
    //     try
    //     {
    //         DoubleWallConnectorId = fabricationConnectorInfo.DoubleWallConnectorId;
    //         IsDoubleWallConnectorLocked = fabricationConnectorInfo.IsDoubleWallConnectorLocked;
    //     }
    //     catch (Exception e)
    //     {
    //         //TODO: no things
    //     }
    //     return new Dictionary<string, object?>
    //     {
    //         {"BodyConnectorId", fabricationConnectorInfo.BodyConnectorId},
    //         {"DoubleWallConnectorId",DoubleWallConnectorId },
    //         {"FabricationIndex", fabricationConnectorInfo.FabricationIndex},
    //         {"IsBodyConnectorLocked", fabricationConnectorInfo.IsBodyConnectorLocked},
    //         {"IsDoubleWallConnectorLocked",IsDoubleWallConnectorLocked },
    //         {"HasDoubleWallConnector", fabricationConnectorInfo.HasDoubleWallConnector()},
    //         {"IsValid", fabricationConnectorInfo.IsValid()},
    //     };
    // }
}