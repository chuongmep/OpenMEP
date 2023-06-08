namespace OpenMEPRevit.ConnectorManager;

/// <summary>MEP connector information.</summary>
/// <since>2016</since>
public class MEPConnectorInfo
{
    private MEPConnectorInfo()
    {
        
    }
    /// <summary>
    /// True if this is the primary connector.
    /// </summary>
    /// <param name="mepConnectorInfo">mepConnectorInfo</param>
    /// <returns name="bool">result</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/MEPConnectorInfo.IsPrimary.png)
    /// </example>
    public static bool IsPrimary(Autodesk.Revit.DB.MEPConnectorInfo mepConnectorInfo)
    {
        bool isPrimary = mepConnectorInfo.IsPrimary;
        return isPrimary;
    }
    /// <summary>
    /// True if this is the secondary connector.
    /// </summary>
    /// <param name="mepConnectorInfo">mepConnectorInfo</param>
    /// <returns name="bool">result</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/MEPConnectorInfo.IsSecondary.png)
    /// </example>
    public static bool IsSecondary(Autodesk.Revit.DB.MEPConnectorInfo mepConnectorInfo)
    {
        bool isSecondary = mepConnectorInfo.IsSecondary;
        return isSecondary;
    }
    
    /// <summary>
    /// The linked connector or null if there is no linked connector
    /// </summary>
    /// <param name="mepConnectorInfo"></param>
    /// <returns name="Connector">connector</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/MEPConnectorInfo.LinkedConnector.png)
    /// </example>
    public static Autodesk.Revit.DB.Connector LinkedConnector(Autodesk.Revit.DB.MEPConnectorInfo mepConnectorInfo)
    {
        Autodesk.Revit.DB.Connector linkedConnector = mepConnectorInfo.LinkedConnector;
        return linkedConnector;
    }
}