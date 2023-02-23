using Autodesk.DesignScript.Runtime;

namespace OpenMEP.Application;

public class Windows
{
    private Windows()
    {
        
    }

    /// <summary>
    /// return version number of windows
    /// </summary>
    /// <returns></returns>
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
}