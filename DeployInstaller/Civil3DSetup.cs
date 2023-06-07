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

    public Dictionary<string, double> DynamoCivil3DVersions = new Dictionary<string, double>()
    {
        {"2.12", 2022},
        {"2.13", 2023},
        {"2.16", 2023},
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
            string dynamoVersionPath = Path.Combine($"C3D {civil3DVersion}",
            "Dynamo", dynamoVersion, "packages", PackageName);
            Console.WriteLine("Folder Wix: " + dynamoVersionPath);
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

        // DirectoryInfo dir = new DirectoryInfo(Path.Combine(appDataDir, "Autodesk", "Dynamo Revit"));
        // Regex regex = new Regex(@"\d+\.\d+");
        // List<DirectoryInfo> directoryInfos =
        //     dir.GetDirectories().Where(x => regex.Match(x.Name).Success).Select(x => x).ToList();
        // // order directoryInfos by regex match value *.*
        // StringNumberComparer comparer = new StringNumberComparer();
        // directoryInfos.Sort((x, y) => comparer.Compare(x.Name, y.Name));
        // foreach (var directory in directoryInfos)
        // {
        //     var directoryInfo = new DirectoryInfo(Path.Combine(directory.FullName, "packages", PackageName));
        //     if (directoryInfo.Exists)
        //     {
        //         var files = new Files($@"{directoryInfo.FullName}\*.*");
        //         var dynamoVersion = directoryInfo.Parent.Parent.Name + $"\\packages\\{PackageName}";
        //         // check DynamoVersion match with DynamoCivil3DVersions
        //         var version = regex.Match(dynamoVersion).Value;
        //         if (DynamoCivil3DVersions.TryGetValue(version, out var civil3DVersion))
        //         {
        //             Console.WriteLine("Add Files Dynamo Civil3D Version: " + civil3DVersion);
        //             if (versionStorages.ContainsKey(dynamoVersion))
        //             {
        //                 versionStorages[dynamoVersion].Add(files);
        //             }
        //             else
        //             {
        //                 Console.WriteLine("Add Files Dynamo Revit Version: " + regex.Match(dynamoVersion).Value);
        //             }
        //
        //             versionStorages.Add(dynamoVersion, new List<WixEntity> {files});
        //             var assemblies = Directory.GetFiles(directoryInfo.FullName, "*", SearchOption.AllDirectories);
        //             foreach (var assembly in assemblies) Console.WriteLine($"'{assembly}'");
        //         }
        //     }
        // }

        return versionStorages.Select(storage => new Dir(storage.Key, storage.Value.ToArray())).Cast<WixEntity>()
            .ToArray();
    }
}