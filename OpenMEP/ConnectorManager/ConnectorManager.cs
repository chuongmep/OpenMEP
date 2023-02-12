using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Revit.Elements;

namespace ConnectorManager;

public static class ConnectorManager
{
    /// <summary>
    /// return connector manager of element
    /// </summary>
    /// <param name="element">element</param>
    /// <returns name="ConnectorManager">ConnectorManager</returns>
    public static Autodesk.Revit.DB.ConnectorManager? GetConnectorManager(this Revit.Elements.Element? element)
    {
        Autodesk.Revit.DB.Element internalElement = element.InternalElement;
        if (internalElement is Autodesk.Revit.DB.FamilyInstance familyInstance)
        {
            return familyInstance.MEPModel.ConnectorManager;
        }

        if (internalElement is Wire wire)
        {
            return wire.ConnectorManager;
        }

        if (internalElement is MEPCurve mepCurve)
        {
            return mepCurve.ConnectorManager;
        }

        return null;
    }

    /// <summary>
    /// return UnusedConnectors from connector manager
    /// </summary>
    /// <param name="connectorManager">connector manager</param>
    /// <returns name="connectors">connectors</returns>
    public static List<Autodesk.Revit.DB.Connector> UnusedConnectors( this 
        Autodesk.Revit.DB.ConnectorManager connectorManager)
    {
        List<Autodesk.Revit.DB.Connector> connectors = new List<Autodesk.Revit.DB.Connector>();
        ConnectorSet connectorSet = connectorManager.UnusedConnectors;
        foreach (Autodesk.Revit.DB.Connector o in connectorSet)
        {
            connectors.Add(o);
        }

        return connectors;
    }

    /// <summary>
    /// This property is used to retrieve the owner of the Connector Manager. 
    /// </summary>
    /// <param name="connectorManager"></param>
    /// <returns name="element">element owner</returns>
    public static Revit.Elements.Element Owner(this Autodesk.Revit.DB.ConnectorManager connectorManager)
    {
        Autodesk.Revit.DB.Element element = connectorManager.Owner;
        Revit.Elements.Element dsType = element.ToDSType(true);
        return dsType;
    }

    /// <summary>
    /// Return all the Connectors of the Connector Manager. 
    /// </summary>
    /// <param name="connectorManager">connector manager</param>
    /// <returns name="connectors">a collections of connector manager</returns>
    public static List<Autodesk.Revit.DB.Connector> Connectors(this Autodesk.Revit.DB.ConnectorManager connectorManager)
    {
        List<Autodesk.Revit.DB.Connector> connectors = new List<Autodesk.Revit.DB.Connector>();
        ConnectorSet connectorSet = connectorManager.Connectors;
        foreach (Autodesk.Revit.DB.Connector connector in connectorSet)
        {
            connectors.Add(connector);
        }

        return connectors;
    }

    /// <summary>
    /// return connector by index 
    /// </summary>
    /// <param name="connectorManager">connector manager</param>
    /// <param name="index">index of connector</param>
    /// <returns name="connector">connector</returns>
    public static Autodesk.Revit.DB.Connector LookUp(this Autodesk.Revit.DB.ConnectorManager connectorManager, int index)
    {
        Autodesk.Revit.DB.Connector connector = connectorManager.Lookup(index);
        return connector;
    }
}