using RevitServices.Persistence;

namespace OpenMEPRevit.Application;

/// <summary>
/// A class to get information about Revit
/// </summary>
public class Revit
{
    private Revit()
    {
        
    }

    /// <summary>
    /// return version number of revit
    /// </summary>
    /// <returns name="string">version</returns>
    /// <example>
    /// ![](../OpenMEPPage/application/dyn/pic/Revit.Version.png)
    /// [Revit.Version.dyn](../OpenMEPPage/application/dyn/Revit.Version.dyn)
    /// </example>
    public static string Version()
    {
        return DocumentManager.Instance.CurrentUIDocument.Application.Application.VersionNumber;
    }
}