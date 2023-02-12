using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Core;
using Revit.Elements;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;
using Point = Autodesk.DesignScript.Geometry.Point;

namespace ConnectorManager;

public static class Connector
{
    /// <summary>
    /// return the radius of connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="radius">radius</returns>
    public static double Radius( this Autodesk.Revit.DB.Connector connector)
    {
        double radius = connector.Radius;
        return radius;
    }

    /// <summary>
    ///  Return Connector Closet With Connector Current
    /// </summary>
    /// <param name="point">origin</param>
    /// <param name="connectors">list connector to check</param>
    /// <returns name="connector">connector</returns>
    public static Autodesk.Revit.DB.Connector? GetConnectorCloset(this Point? point,
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
    public static Autodesk.Revit.DB.Connector? GetConnectorCloset(this Autodesk.Revit.DB.Connector? c,
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
    /// return connector farthest with point current
    /// </summary>
    /// <param name="point"></param>
    /// <param name="connectors"></param>
    /// <returns></returns>
    public static Autodesk.Revit.DB.Connector? GetConnectorFarthest(this Point? point,
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
    public static Autodesk.Revit.DB.Connector? GetConnectorFarthest(this Autodesk.Revit.DB.Connector? c,
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
    public static List<Autodesk.Revit.DB.Connector?> GetConnectors( this 
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
    public static List<Autodesk.Revit.DB.Connector?> GetConnectors(this Revit.Elements.Element? element)
    {
        Autodesk.Revit.DB.ConnectorManager? connectorManager =
            element.GetConnectorManager();
        if (connectorManager == null) throw new ArgumentNullException(nameof(connectorManager));
        return GetConnectors(connectorManager);
    }

    /// <summary>
    /// return list connector from element
    /// </summary>
    /// <param name="element">element</param>
    /// <returns name="connectors">connectors</returns>
    public static List<Autodesk.Revit.DB.Connector?> GetUnusedConnectors(this Revit.Elements.Element? element)
    {
        Autodesk.Revit.DB.ConnectorManager? connectorManager =
            element.GetConnectorManager();
        if (connectorManager == null) throw new ArgumentNullException(nameof(connectorManager));
        return GetUnusedConnectors(connectorManager);
    }

    /// <summary>
    /// return list connector from element
    /// </summary>
    /// <param name="connectorManager">connector manager</param>
    /// <returns name="connectors">connectors</returns>
    public static List<Autodesk.Revit.DB.Connector?> GetUnusedConnectors( this 
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
    public static dynamic? SystemType(this Autodesk.Revit.DB.Connector connector)
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
    public static Point? Direction(this Autodesk.Revit.DB.Connector? connector)
    {
        Transform? t = connector?.CoordinateSystem;
        if (t == null) return null;
        return Autodesk.DesignScript.Geometry.Point.ByCoordinates(t.BasisX.X, t.BasisX.Y, t.BasisX.Z);
    }

    /// <summary>
    /// Get direction of connector
    /// </summary>
    /// <param name="connector"></param>
    /// <returns></returns>
    public static Point? Origin(this Autodesk.Revit.DB.Connector? connector)
    {
        return connector?.Origin?.ToPoint();
    }

    /// <summary>
    /// check connector is connected or not
    /// </summary>
    /// <param name="connector"></param>
    /// <returns></returns>
    public static bool? IsConnected(this Autodesk.Revit.DB.Connector? connector)
    {
        return connector?.IsConnected;
    }


    /// <summary>
    /// return distance between one connector with another point
    /// </summary>
    /// <param name="connector"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static double? DistanceTo(this Autodesk.Revit.DB.Connector? connector,
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
    public static double? DistanceTo(this Autodesk.Revit.DB.Connector? connector1,
        Autodesk.Revit.DB.Connector? connector2)
    {
        XYZ? origin = connector1?.Origin;
        XYZ? target = connector2?.Origin;
        double? distanceTo = origin?.DistanceTo(target);
        return distanceTo;
    }

    /// <summary>
    /// The host of the connector.
    /// The element that contains this connector. It may also contain other connectors.
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="element">element</returns>
    public static Revit.Elements.Element Owner(this Autodesk.Revit.DB.Connector connector)
    {
        Revit.Elements.Element dsType = connector.Owner.ToDSType(true);
        return dsType;
    }

    /// <summary>
    /// return id of connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="Id">Id</returns>
    public static double Id(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.Id;
    }

    /// <summary>
    /// return angle of connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="angle">angle</returns>
    public static double Angle(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.Angle;
    }

    /// <summary>
    /// The coefficient of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="Coefficient">Coefficient</returns>
    public static double Coefficient(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.Coefficient;
    }

    /// <summary>
    /// The demand of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="Demand">Demand</returns>
    public static double Demand(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.Demand;
    }

    /// <summary>
    /// The Flow of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="Flow">Flow</returns>
    public static double Flow(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.Flow;
    }

    /// <summary>
    /// The height of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="Height">Height</returns>
    public static double Height(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.Height;
    }

    /// <summary>
    /// The Width of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="Width">Width</returns>
    public static double Width(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.Width;
    }

    /// <summary>
    /// The assigned flow of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="AssignedFlow">AssignedFlow</returns>
    public static double AssignedFlow(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedFlow;
    }

    /// <summary>
    /// Connector engagement length
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="Connector engagement length">Connector engagement length</returns>
    public static double EngagementLength(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.EngagementLength;
    }

    /// <summary>
    /// The pressure drop of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="PressureDrop">PressureDrop</returns>
    public static double PressureDrop(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.PressureDrop;
    }

    /// <summary>
    /// All references of the connector.
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="connectors">A set of connectors that the connectors is connected to, including both physical connection and logical connection.</returns>
    public static List<Autodesk.Revit.DB.Connector> AllRefs(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.AllRefs.Cast<Autodesk.Revit.DB.Connector>().ToList();
    }

    /// <summary>
    /// The domain of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="Domain">Domain</returns>
    public static dynamic Domain(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.Domain;
    }

    /// <summary>
    /// The velocity pressure of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="VelocityPressure">VelocityPressure</returns>
    public static double VelocityPressure(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.VelocityPressure;
    }

    /// <summary>
    /// The assigned fixture units of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="AssignedFixtureUnits">AssignedFixtureUnits</returns>
    public static double AssignedFixtureUnits(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedFixtureUnits;
    }

    /// <summary>
    /// The assigned flow factor of this connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="AssignedFlowFactor">AssignedFlowFactor</returns>
    public static double AssignedFlowFactor(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedFlowFactor;
    }

    /// <summary>
    /// The assigned kCoefficient of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="AssignedKCoefficient">AssignedKCoefficient</returns>
    public static double AssignedKCoefficient(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedKCoefficient;
    }

    /// <summary>
    /// return element connected with this connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="element">element has connected with connecter</returns>
    public static Revit.Elements.Element? GetElementConnectedWith(this Autodesk.Revit.DB.Connector connector)
    {
        if (connector.IsConnected)
        {
            return connector.AllRefs.Cast<Autodesk.Revit.DB.Connector>().First().Owner.ToDynamoType();
        }

        return null;
    }

    /// <summary>
    /// return element connected with this connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="element">element has connected with connector</returns>
    public static Autodesk.Revit.DB.Connector? GetConnectorConnectedWith(this Autodesk.Revit.DB.Connector connector)
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
    /// <returns name="AssignedLossCoefficient">AssignedLossCoefficient</returns>
    public static double AssignedLossCoefficient(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedLossCoefficient;
    }

    /// <summary>
    /// The assigned pressure drop of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="AssignedPressureDrop">AssignedPressureDrop</returns>
    public static double AssignedPressureDrop(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.AssignedPressureDrop;
    }

    /// <summary>
    /// The description of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="Description">Description</returns>
    public static string Description(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.Description;
    }

    /// <summary>
    /// The shape of the connector.
    /// </summary>
    /// <param name="connector">Connector</param>
    /// <returns name="ConnectorProfileType">ConnectorProfileType</returns>
    public static dynamic Shape(this Autodesk.Revit.DB.Connector connector)
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
    public static Dictionary<string, object?> CoordinateSystem(this Autodesk.Revit.DB.Connector connector)
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
    public static Autodesk.Revit.DB.ConnectorManager ConnectorManager(this Autodesk.Revit.DB.Connector connector)
    {
        return connector.ConnectorManager;
    }

    /// <summary>
    /// Get area of connector
    /// </summary>
    /// <param name="connector">connector</param>
    /// <returns name="area">area of connector</returns>
    public static double GetArea(this Autodesk.Revit.DB.Connector connector)
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
    public static Autodesk.Revit.DB.MEPConnectorInfo GetMEPConnectorInfo(this Autodesk.Revit.DB.Connector connector)
    {
        Autodesk.Revit.DB.MEPConnectorInfo mepConnectorInfo = connector.GetMEPConnectorInfo();
        return mepConnectorInfo;
    }

    /// <summary>
    /// set new angle for connector
    /// </summary>
    /// <param name="connector"></param>
    /// <param name="angle"></param>
    public static void SetAngle(this Autodesk.Revit.DB.Connector connector, double angle)
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
    public static void SetOrigin(this Autodesk.Revit.DB.Connector connector, Autodesk.DesignScript.Geometry.Point origin)
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
    public static Autodesk.Revit.DB.Connector DisConnectFrom(this Autodesk.Revit.DB.Connector connector,
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
    public static Autodesk.Revit.DB.Connector ConnectTo(this Autodesk.Revit.DB.Connector connector,
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