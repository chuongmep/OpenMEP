using Microsoft.Win32;

namespace OpenMEP.Application;

/// <summary>
///  a class to to get information about OpenMEP
/// </summary>
public class OpenMEP
{
    private OpenMEP()
    {
        
    }
    /// <summary>
    /// Return Current Version Installed In Computer
    /// </summary>
    /// <example>
    /// ![](../OpenMEPPage/application/dyn/pic/OpenMEP.Version.png)
    /// </example>
    public static string? Version
    {
        get
        {
            string appName = "OpenMEP";
            string? displayName;
            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registryKey)!;
            foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
            {
                displayName = subkey!.GetValue("DisplayName") as string;
                if (displayName != null && displayName.Contains(appName))
                {
                    return (subkey.GetValue("DisplayVersion") as string)!;
                }
            }
            key.Close();

            registryKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registryKey)!;
            foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
            {
                displayName = subkey!.GetValue("DisplayName") as string;
                if (displayName != null && displayName.Contains(appName))
                {
                    return subkey.GetValue("DisplayVersion") as string;
                }
            }
            key.Close();
            return string.Empty;
        }

    }
}