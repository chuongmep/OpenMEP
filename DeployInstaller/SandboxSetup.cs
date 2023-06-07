using System.Text;
using System.Text.RegularExpressions;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;

namespace DeployInstaller;

public class SandboxSetup : IInstaller
{
    const string outputName = "OpenMEP";
    string PackageName = "OpenMEP";
    const string projectNameSandbox = "OpenMEPSandbox";
    const string outputDir = "output";
    
    string installationDirSandbox = @"%AppDataFolder%\Dynamo\Dynamo Core\";

    public void CreateInstaller(string version)
    {
        string fileNameSandbox =
            new StringBuilder().Append(outputName + "Sandbox").Append("-").Append(version).ToString();
        //Sandbox Project Setup
        var projectSandbox = new Project
        {
            Name = projectNameSandbox,
            OutDir = outputDir,
            Platform = Platform.x64,
            UI = WUI.WixUI_InstallDir,
            Version = new Version(version),
            OutFileName = fileNameSandbox,
            InstallScope = InstallScope.perUser,
            MajorUpgrade = MajorUpgrade.Default,
            UpgradeCode = new Guid("E937CBEB-3675-4CD6-8C71-335F900DC6E2"),
            GUID = new Guid("838E309D-48F2-43F6-BCD4-544FE6E8682A"),
            BackgroundImage = @"Resources\BackgroundImage.png",
            BannerImage = @"Resources\BannerImage.png",
            ControlPanelInfo =
            {
                Manufacturer = "chuongmep",
                HelpLink = "https://chuongmep.com",
                ProductIcon = @"Resources\ShellIcon.ico",
                Comments = "Project Dynamo Sandbox open source Automation",
                Contact = "chuongpqvn@gmail.com",
                UrlUpdateInfo = "https://github.com/chuongmep/OpenMEP/releases/latest"
            },
            Dirs = new Dir[]
            {
                new InstallDir(installationDirSandbox, GenerateWixEntities()),
            }
        };
        MajorUpgrade.Default.AllowSameVersionUpgrades = true;
        // MajorUpgrade.Default.AllowDowngrades = true;
        projectSandbox.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.VerifyReadyDlg);
        string buildSanboxMsi = projectSandbox.BuildMsi();
        FileInfo fileSandboxInfo = new FileInfo(buildSanboxMsi);
        if (fileSandboxInfo.Directory == null) throw new ArgumentNullException(nameof(fileSandboxInfo.Directory));

        string sandboxZip = Path.Combine(fileSandboxInfo.Directory.FullName, fileNameSandbox + ".zip");
        Utils.CompressFile(buildSanboxMsi, sandboxZip);
    }

    public WixEntity[] GenerateWixEntities()
    {
        var versionStorages = new Dictionary<string, List<WixEntity>>();
        // Get appdata directory
        var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        DirectoryInfo dir = new DirectoryInfo(Path.Combine(appDataDir, "Dynamo", "Dynamo Core"));
        Regex regex = new Regex(@"\d+\.\d+");
        List<DirectoryInfo> directoryInfos =
            dir.GetDirectories().Where(x => regex.Match(x.Name).Success).Select(x => x).ToList();
        // order directoryInfos by regex match value *.*
        StringNumberComparer comparer = new StringNumberComparer();
        directoryInfos.Sort((x, y) => comparer.Compare(x.Name, y.Name));
        foreach (var directory in directoryInfos)
        {
            var directoryInfo = new DirectoryInfo(Path.Combine(directory.FullName, "packages", PackageName));
            if (directoryInfo.Exists)
            {
                var files = new Files($@"{directoryInfo.FullName}\*.*");
                var dynamoVersion = directoryInfo.Parent.Parent.Name + $"\\packages\\{PackageName}";
                if (versionStorages.ContainsKey(dynamoVersion))
                {
                    versionStorages[dynamoVersion].Add(files);
                }
                else
                    Console.WriteLine("Add Files Dynamo Sandbox Version: " + regex.Match(dynamoVersion).Value);

                versionStorages.Add(dynamoVersion, new List<WixEntity> {files});
                var assemblies = Directory.GetFiles(directoryInfo.FullName, "*", SearchOption.AllDirectories);
                foreach (var assembly in assemblies) Console.WriteLine($"'{assembly}'");
            }
        }

        return versionStorages.Select(storage => new Dir(storage.Key, storage.Value.ToArray())).Cast<WixEntity>()
            .ToArray();
    }
}