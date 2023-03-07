using RevitServices.Persistence;

namespace OpenMEP.Application;

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
    /// </example>
    public static string Version()
    {
        return DocumentManager.Instance.CurrentUIDocument.Application.Application.VersionNumber;
    }
}