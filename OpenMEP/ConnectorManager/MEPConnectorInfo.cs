namespace ConnectorManager;

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
    public static Autodesk.Revit.DB.Connector LinkedConnector(Autodesk.Revit.DB.MEPConnectorInfo mepConnectorInfo)
    {
        Autodesk.Revit.DB.Connector linkedConnector = mepConnectorInfo.LinkedConnector;
        return linkedConnector;
    }
}