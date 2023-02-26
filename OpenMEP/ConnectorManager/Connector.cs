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
    public static double Radius(Autodesk.Revit.DB.Connector connector)
    {
        double radius = connector.Radius;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_PipeSize).DisplayUnits;
        double value = UnitUtils.ConvertToInternalUnits(radius, unitTypeId);
        return value;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.PipeSize).GetUnitTypeId();
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
    public static Autodesk.Revit.DB.Connector? GetConnectorClosest(Point? point,
        List<Autodesk.Revit.DB.Connector?> connectors)
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
    public static Autodesk.Revit.DB.Connector? GetConnectorClosest(Autodesk.Revit.DB.Connector? c,
        List<Autodesk.Revit.DB.Connector?> connectors)
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

        return null;
    }

    /// <summary>
    /// Get Closet Connector With Element
    /// </summary>
    /// <param name="element">element to check</param>
    /// <param name="connectorSet">an collection connectors</param>
    /// <returns name="connector">closet connector</returns>
    public static Autodesk.Revit.DB.Connector? GetConnectorClosest(Revit.Elements.Element? element,
        ConnectorSet? connectorSet)
    {
        Autodesk.Revit.DB.Connector? closet = null;
        Point? locationCenter = global::OpenMEP.Element.Element.LocationCenter(element);
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
    /// <returns></returns>
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
    /// return list connector from connector manager
    /// </summary>
    /// <param name="connectorManager">connector manager</param>
    /// <returns name="connectors">connectors</returns>
    public static List<Autodesk.Revit.DB.Connector?> GetConnectors(
        Autodesk.Revit.DB.ConnectorManager? connectorManager)
    {
        if (connectorManager == null) throw new ArgumentNullException(nameof(connectorManager));
        List<Autodesk.Revit.DB.Connector?> connectors = new List<Autodesk.Revit.DB.Connector?>();
        foreach (Autodesk.Revit.DB.Connector? c in connectorManager.Connectors)
        {
            connectors.Add(c);
        }

        return connectors;
    }

    /// <summary>
    /// return list connector from element
    /// </summary>
    /// <param name="element">element</param>
    /// <returns name="connectors">connectors</returns>
    public static List<Autodesk.Revit.DB.Connector?> GetConnectors(Revit.Elements.Element? element)
    {
        Autodesk.Revit.DB.ConnectorManager? connectorManager =
            global::OpenMEP.ConnectorManager.ConnectorManager.GetConnectorManager(element);
        if (connectorManager == null) throw new ArgumentNullException(nameof(connectorManager));
        return GetConnectors(connectorManager);
    }

    /// <summary>
    /// return list connector from element
    /// </summary>
    /// <param name="element">element</param>
    /// <returns name="connectors">connectors</returns>
    public static List<Autodesk.Revit.DB.Connector?> GetUnusedConnectors(Revit.Elements.Element? element)
    {
        Autodesk.Revit.DB.ConnectorManager? connectorManager =
            global::OpenMEP.ConnectorManager.ConnectorManager.GetConnectorManager(element);
        if (connectorManager == null) throw new ArgumentNullException(nameof(connectorManager));
        return GetUnusedConnectors(connectorManager);
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
    /// return system type of connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="domain">domain</returns>
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
    public static Point? Direction(Autodesk.Revit.DB.Connector? connector)
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
    public static Point? Origin(Autodesk.Revit.DB.Connector? connector)
    {
        return connector?.Origin?.ToPoint();
    }

    /// <summary>
    /// check connector is connected or not
    /// </summary>
    /// <param name="connector">the connector</param>
    /// <returns name="bool">true if connector is connected</returns>
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
    public static double Id(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Id;
    }

    /// <summary>
    /// return angle of connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="double">radian</returns>
    public static double Angle(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Angle;
    }

    /// <summary>
    /// The coefficient of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">Coefficient</returns>
    public static double Coefficient(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Coefficient;
    }

    /// <summary>
    /// The demand of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">Demand</returns>
    public static double Demand(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Demand;
    }

    /// <summary>
    /// The Flow of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">Flow of connector</returns>
    public static double Flow(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Flow;
    }

    /// <summary>
    /// The height of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">Height</returns>
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
    public static double AssignedFlow(Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedFlow;
    }

    /// <summary>
    /// Connector engagement length
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">Connector engagement length</returns>
    public static double EngagementLength(Autodesk.Revit.DB.Connector connector)
    {
        return connector.EngagementLength;
    }

    /// <summary>
    /// The pressure drop of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">PressureDrop</returns>
    public static double PressureDrop(Autodesk.Revit.DB.Connector connector)
    {
        return connector.PressureDrop;
    }

    /// <summary>
    /// All references of the connector.
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="connectors">A set of connectors that the connectors is connected to, including both physical connection and logical connection.</returns>
    public static List<Autodesk.Revit.DB.Connector> AllRefs(Autodesk.Revit.DB.Connector connector)
    {
        return connector.AllRefs.Cast<Autodesk.Revit.DB.Connector>().ToList();
    }

    /// <summary>
    /// The domain of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="Autodesk.Revit.DB.Domain">Domain</returns>
    public static dynamic Domain(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Domain;
    }

    /// <summary>
    /// The velocity pressure of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">VelocityPressure</returns>
    public static double VelocityPressure(Autodesk.Revit.DB.Connector connector)
    {
        return connector.VelocityPressure;
    }

    /// <summary>
    /// The assigned fixture units of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">AssignedFixtureUnits</returns>
    public static double AssignedFixtureUnits(Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedFixtureUnits;
    }

    /// <summary>
    /// The assigned flow factor of  connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">AssignedFlowFactor</returns>
    public static double AssignedFlowFactor(Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedFlowFactor;
    }

    /// <summary>
    /// The assigned kCoefficient of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">AssignedKCoefficient</returns>
    public static double AssignedKCoefficient(Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedKCoefficient;
    }

    /// <summary>
    /// return element connected with  connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="element">element has connected with connector</returns>
    public static Revit.Elements.Element? GetElementConnectedWith(Autodesk.Revit.DB.Connector connector)
    {
        if (connector.IsConnected)
        {
            return connector.AllRefs.Cast<Autodesk.Revit.DB.Connector>().First().Owner.ToDynamoType();
        }

        return null;
    }

    /// <summary>
    /// return element connected with  connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="element">element has connected with connector</returns>
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
    public static double AssignedLossCoefficient(Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedLossCoefficient;
    }

    /// <summary>
    /// The assigned pressure drop of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="double">AssignedPressureDrop</returns>
    public static double AssignedPressureDrop(Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedPressureDrop;
    }

    /// <summary>
    /// The description of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="string">Description</returns>
    public static string Description(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Description;
    }

    /// <summary>
    /// The shape of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="ConnectorProfileType">ConnectorProfileType</returns>
    public static dynamic Shape(Autodesk.Revit.DB.Connector connector)
    {
        return connector.Shape;
    }


    /// <summary>
    /// Return CoordinateSystem of the connector.
    /// </summary>
    /// <param name="connector"></param>
    /// <returns></returns>
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
    public static Autodesk.Revit.DB.ConnectorManager ConnectorManager(Autodesk.Revit.DB.Connector connector)
    {
        return connector.ConnectorManager;
    }

    /// <summary>
    /// Get area of connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="double">area of connector</returns>
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
}