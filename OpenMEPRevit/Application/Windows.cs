using System.IO;
using Autodesk.DesignScript.Runtime;
using Dynamo.Applications;

namespace OpenMEPRevit.Application;

/// <summary>
/// A class to get information about Windows
/// </summary>
public class Windows
{
    private Windows()
    {
        
    }

    /// <summary>
    /// return version number of windows
    /// </summary>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/application/dyn/pic/Windows.Version.png)
    /// [Windows.Version.dyn)](../OpenMEPPage/application/dyn/Windows.Version.dyn)
    /// </example>
    [MultiReturn("build", "major", "minor", "revision", "version")]
    public static Dictionary<string, object?> Version()
    {
        // Get version System 
        int build = System.Environment.OSVersion.Version.Build;
        int major = System.Environment.OSVersion.Version.Major;
        int minor = System.Environment.OSVersion.Version.Minor;
        int revision = System.Environment.OSVersion.Version.Revision;
        string version = System.Environment.OSVersion.Version.ToString();
        return new Dictionary<string, object?>()
        {
            {"build", build},
            {"major", major},
            {"minor", minor},
            {"revision", revision},
            {"version", version}
        };
    }

    /// <summary>
    /// Return the full path of relative path same folder with current script.
    /// e.g. FileFromRelativePath("test.txt") will return the full path of test.txt in the same folder with current script.
    /// </summary>
    /// <param name="relativeFileName">The file name.extension</param>
    /// <returns name="filePath">path</returns>
    public static string FileFromRelativePath(string relativeFileName)
    {
        try
        {
            string fileName = DynamoRevit.RevitDynamoModel.CurrentWorkspace.FileName;
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(relativeFileName));
            var directory = Path.GetDirectoryName(fileName);
            if (directory == null) throw new ArgumentNullException(nameof(relativeFileName));
            string filePath = Path.Combine(directory, relativeFileName);
            if (!File.Exists(filePath)) throw new FileNotFoundException("File not found");
            return filePath;
        }
        catch (Exception e)
        {
            return string.Empty;
        }
    }
}