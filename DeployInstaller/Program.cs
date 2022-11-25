// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.RegularExpressions;
using DeployInstaller;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;
using ICSharpCode.SharpZipLib.Zip;
using File = System.IO.File;

string installationDir = @"%AppDataFolder%\Dynamo\Dynamo Revit\";
const string projectName = "OpenMEP";
const string outputName = "OpenMEP";
string folderPackageName = "OpenMEP";
const string outputDir = "output";
string Version = $"1.0.{GetLastTwoDigitOfYear()}.{GetDayInYear()}";
var fileName = new StringBuilder().Append(outputName).Append("-").Append(Version);

var project = new Project
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
        Comments = "Project open source help mep engineer happy",
    },
    Dirs = new Dir[]
    {
        new InstallDir(installationDir, GenerateWixEntities()),
    }
};
MajorUpgrade.Default.AllowSameVersionUpgrades = true;
// MajorUpgrade.Default.AllowDowngrades = true;
project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.InstallDirDlg);
string buildMsi = project.BuildMsi();
FileInfo fileInfo = new FileInfo(buildMsi);
// get version info 

string zipfileName = Path.Combine(fileInfo.Directory.FullName, fileName + ".zip");
CompressFile(buildMsi, zipfileName);


WixEntity[] GenerateWixEntities()
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
                Console.WriteLine("Add Files Dynamo Version: " + regex.Match(dynamoVersion).Value);
                versionStorages.Add(dynamoVersion, new List<WixEntity> {files});
            var assemblies = Directory.GetFiles(directoryInfo.FullName, "*", SearchOption.AllDirectories);
            foreach (var assembly in assemblies) Console.WriteLine($"'{assembly}'");
        }
    }

    return versionStorages.Select(storage => new Dir(storage.Key, storage.Value.ToArray())).Cast<WixEntity>().ToArray();
}

void CompressFile(string filePath, string OutputFilePath, int compressLevel = 9)
{
    try
    {
        using (ZipOutputStream OutputStream = new ZipOutputStream(File.Create(OutputFilePath)))
        {
            // Define the compression level
            // 0 - store only to 9 - means best compression
            OutputStream.SetLevel(compressLevel);
            byte[] buffer = new byte[4096];
            ZipEntry entry = new ZipEntry(Path.GetFileName(filePath));
            entry.DateTime = DateTime.Now;
            OutputStream.PutNextEntry(entry);

            using (FileStream fs = File.OpenRead(filePath))
            {
                // Using a fixed size buffer here makes no noticeable difference for output
                // but keeps a lid on memory usage.
                int sourceBytes;
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    OutputStream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }

            OutputStream.Finish();
            OutputStream.Close();
            Console.WriteLine("Zip file has been built: " + OutputFilePath);
        }
    }
    catch (Exception ex)
    {
        // No need to rethrow the exception as for our purposes its handled.
        Console.WriteLine("Exception during processing {0}", ex);
    }
}

string GetDayInYear()
{
    // Get Day Current Of Year
    DateTime now = DateTime.Now;
    DateTime startOfYear = new DateTime(now.Year, 1, 1);
    return (now - startOfYear).Days.ToString();
}

string GetLastTwoDigitOfYear()
{
    DateTime now = DateTime.Now;
    return now.Year.ToString().Substring(2, 2);
}


