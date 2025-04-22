using System.Text;
using System.Text.RegularExpressions;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;
using File = WixSharp.File;

namespace DeployInstaller;

public class Civil3DSetup : IInstaller
{
    string installationDir = @"%AppDataFolder%\Autodesk\";

    const string projectName = "OpenMEPCivil3D";

    const string outputName = "OpenMEPCivil3D";
    string PackageName = "OpenMEP";
    const string outputDir = "output";


    public void CreateInstaller(string version)
    {
        string fileName = new StringBuilder().Append(outputName).Append("-").Append(version).ToString();
        var projectRevit = new Project
        {
            Name = projectName,
            OutDir = outputDir,
            Platform = Platform.x64,
            UI = WUI.WixUI_InstallDir,
            Version = new Version(version),
            OutFileName = fileName,
            InstallScope = InstallScope.perUser,
            MajorUpgrade = MajorUpgrade.Default,
            UpgradeCode = new Guid("7944C304-060C-44CD-AC00-3A42005B9A42"),
            GUID = new Guid("67F86ABD-EDDF-4CBF-99B6-7A0C10C921D1"),
            BackgroundImage = @"Resources\BackgroundImage.png",
            BannerImage = @"Resources\BannerImage.png",
            ControlPanelInfo =
            {
                Manufacturer = "chuongmep",
                HelpLink = "https://chuongmep.com",
                ProductIcon = @"Resources\ShellIcon.ico",
                Comments = "Project Dynamo Civil3D open source Automation",
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

    public Dictionary<string, double> DynamoCivil3DVersions = new Dictionary<string, double>()
    {
        {"2.12", 2022},
        {"2.13", 2023},
        // {"2.16", 2023},
        {"2.17", 2024},
    };

    public WixEntity[] GenerateWixEntities()
    {
        var versionStorages = new Dictionary<string, List<WixEntity>>();
        // Get appdata directory
        var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        foreach (KeyValuePair<string, double> keyValuePair in DynamoCivil3DVersions)
        {
            string dynamoVersion = keyValuePair.Key;
            double civil3DVersion = keyValuePair.Value;
            Console.WriteLine("Civil3D Version: " + civil3DVersion);
            var directoryInfo = new DirectoryInfo(Path.Combine(appDataDir, "Autodesk", $"C3D {civil3DVersion}",
                "Dynamo", dynamoVersion, "packages", PackageName));
            string dynamoVersionPath = $"C3D {civil3DVersion}\\Dynamo\\{dynamoVersion}\\packages\\{PackageName}";
            if (directoryInfo.Exists)
            {
                var files = new Files($@"{directoryInfo.FullName}\*.*");
                if (versionStorages.ContainsKey(dynamoVersionPath))
                {
                    versionStorages[dynamoVersionPath].Add(files);
                }
                else
                {
                    Console.WriteLine("Add Files Dynamo Civil Version: " + dynamoVersion);
                    versionStorages.Add(dynamoVersionPath, new List<WixEntity> {files});
                    var assemblies = Directory.GetFiles(directoryInfo.FullName, "*", SearchOption.AllDirectories);
                    foreach (string assembly in assemblies)
                    {
                        Console.WriteLine(assembly);
                    }
                }
            }
            else
            {
                Console.WriteLine("Directory not found: " + directoryInfo.FullName);
            }
        }
        
        return versionStorages.Select(storage => new Dir(storage.Key, storage.Value.ToArray())).Cast<WixEntity>()
            .ToArray();
    }
}