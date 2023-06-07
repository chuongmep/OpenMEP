using WixSharp;

namespace DeployInstaller;

public interface IInstaller
{
    /// <summary>
    /// Execute the installer
    /// </summary>
    public void CreateInstaller(string Version);
    
    /// <summary>
    /// Generate Wix entities
    /// </summary>
    /// <returns></returns>
    public WixEntity[] GenerateWixEntities();
}