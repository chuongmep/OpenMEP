using System.Reflection;
using Dynamo.Applications;

namespace OpenMEP.Application;

public class Dynamo
{
    private Dynamo()
    {
    }

    /// <summary>
    /// return current version of dynamo
    /// </summary>
    /// <returns name="version">version</returns>
    /// <example>
    /// ![](../OpenMEPPage/application/dyn/pic/Dynamo.Version.png)
    /// </example>
    public static string Version()
    {
        Assembly assembly = Assembly.Load("DynamoCore");
        Version version = assembly.GetName().Version;
        return version.ToString();
    }

#if R20 || R21
#else
    /// <summary>
    /// return current file name of script opening
    /// </summary>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/application/dyn/pic/Dynamo.CurrentFileName.png)
    /// </example>
    public static string CurrentFileName()
    {
        try
        {
            string fileName = DynamoRevit.RevitDynamoModel.CurrentWorkspace.FileName;
            if (string.IsNullOrEmpty(fileName)) return "Home";
            return fileName;
        }
        catch (Exception e)
        {
            return string.Empty;
        }
    }
#endif
}