using System.Text;
using System.Text.RegularExpressions;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;

namespace DeployInstaller;

public class RevitSetup : IInstaller
{
    string installationDir = @"%AppDataFolder%\Dynamo\Dynamo Revit\";

    const string projectName = "OpenMEP";

    const string outputName = "OpenMEP";
    string folderPackageName = "OpenMEP";
    const string outputDir = "output";
    static string Version = $"2.0.{Utils.GetLastTwoDigitOfYear()}.{Utils.GetDayInYear()}{Utils.GetDay()}";
    string fileName = new StringBuilder().Append(outputName).Append("-").Append(Version).ToString();

    public void CreateInstaller()
    {
        var projectRevit = new Project
        {
            Name = projectName,
            OutDir = outputDir,
            Platform = Platform.x64,
            UI = WUI.WixUI_InstallDir,
            Version = new Version(Version),
            OutFileName = fileName.ToString(),
            InstallScope = InstallScope.perUser,
            MajorUpgrade = MajorUpgrade.Default,
            UpgradeCode = new Guid("20ED4E57-73E8-4075-9402-EC650D8291D2"),
            GUID = new Guid("9289E103-87BE-433A-8206-1473D7DE0515"),
            BackgroundImage = @"Resources\BackgroundImage.png",
            BannerImage = @"Resources\BannerImage.png",
            ControlPanelInfo =
            {
                Manufacturer = "chuongmep",
                HelpLink = "https://chuongmep.com",
                ProductIcon = @"Resources\ShellIcon.ico",
                Comments = "Project Dynamo Revit open source Automation",
                Contact = "chuongpqvn@gmail.com",
                UrlUpdateInfo = "https://github.com/chuongmep/OpenMEP/releases/latest"
            },
            Dirs = new Dir[]
            {
                new InstallDir(installationDir, GenerateWixEntities()),
            }
        };
        MajorUpgrade.Default.AllowSameVersionUpgrades = true;
        // MajorUpgrade.Default.AllowDowngrades = true;
        projectRevit.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.VerifyReadyDlg);
        string buildMsi = projectRevit.BuildMsi();
        FileInfo fileInfo = new FileInfo(buildMsi);
        if (fileInfo.Directory == null) throw new Exception("Directory not found");

        string zipFileName = Path.Combine(fileInfo.Directory.FullName, fileName + ".zip");
        Utils.CompressFile(buildMsi, zipFileName);
    }
    public WixEntity[] GenerateWixEntities()
    {
        var versionStorages = new Dictionary<string, List<WixEntity>>();
        // Get appdata directory
        var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        DirectoryInfo dir = new DirectoryInfo(Path.Combine(appDataDir, "Dynamo", "Dynamo Revit"));
        Regex regex = new Regex(@"\d+\.\d+");
        List<DirectoryInfo> directoryInfos =
            dir.GetDirectories().Where(x => regex.Match(x.Name).Success).Select(x => x).ToList();
        // order directoryInfos by regex match value *.*
        StringNumberComparer comparer = new StringNumberComparer();
        directoryInfos.Sort((x, y) => comparer.Compare(x.Name, y.Name));
        foreach (var directory in directoryInfos)
        {
            var directoryInfo = new DirectoryInfo(Path.Combine(directory.FullName, "packages", folderPackageName));
            if (directoryInfo.Exists)
            {
                var files = new Files($@"{directoryInfo.FullName}\*.*");
                var dynamoVersion = directoryInfo.Parent.Parent.Name + $"\\packages\\{folderPackageName}";
                if (versionStorages.ContainsKey(dynamoVersion))
                {
                    versionStorages[dynamoVersion].Add(files);
                }
                else
                    Console.WriteLine("Add Files Dynamo Revit Version: " + regex.Match(dynamoVersion).Value);

                versionStorages.Add(dynamoVersion, new List<WixEntity> {files});
                var assemblies = Directory.GetFiles(directoryInfo.FullName, "*", SearchOption.AllDirectories);
                foreach (var assembly in assemblies) Console.WriteLine($"'{assembly}'");
            }
        }

        return versionStorages.Select(storage => new Dir(storage.Key, storage.Value.ToArray())).Cast<WixEntity>().ToArray();
    }
}