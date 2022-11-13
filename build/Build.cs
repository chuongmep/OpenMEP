using _build;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.VSWhere;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;
internal partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    readonly string[] Projects =
    {
        "OpenMEP"
    };
    private const string ArtifactsFolder = "output";
    public const string InstallerProject = "DeployInstaller";

    public const string BuildConfiguration = "Release";
    public const string InstallerConfiguration = "Installer";
    private const string BinOutputFolder = "bin";
    private static readonly Lazy<string> MsBuildPath = new(() =>
    {
        if (IsServerBuild) return null;
        var (_, output) = VSWhereTasks.VSWhere(settings => settings
            .EnableLatest()
            .AddRequires("Microsoft.Component.MSBuild")
            .SetProperty("installationPath")
        );

        if (output.Count > 0) return null;
        if (!File.Exists(CustomMsBuildPath)) throw new Exception($"Missing file: {CustomMsBuildPath}. Change the path to the build platform or install Visual Studio.");
        return CustomMsBuildPath;
    });

    //Specify the path to the MSBuild.exe file here if you are not using VisualStudio
    private const string CustomMsBuildPath = @"C:\Program Files\JetBrains\JetBrains Rider\tools\MSBuild\Current\Bin\MSBuild.exe";
    private readonly AbsolutePath ArtifactsDirectory = RootDirectory / ArtifactsFolder;
    private readonly AbsolutePath ChangeLogPath = RootDirectory / "CHANGELOG.md";
    [GitRepository] private readonly GitRepository GitRepository;
    [Solution] private readonly Solution Solution;
    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    
    Target Compile => _ => _
        .DependsOn(CreateInstaller)
        .Executes(() =>
        {
            Console.WriteLine("Compile target");
        });
  
    Target RestoreAndBuild => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            var configurations = Solution.GetConfigurations(BuildConfiguration);
            configurations.ForEach(configuration =>
            {
                MSBuild(s => s
                    .SetTargets("Rebuild")
                    .SetProcessToolPath(MsBuildPath.Value)
                    .SetConfiguration(configuration)
                    .SetVerbosity(MSBuildVerbosity.Minimal)
                    .DisableNodeReuse()
                    .EnableRestore());
            });
        });
    Target Clean => _ => _
        .Executes(() =>
        {
            // clear the output directory of msi file
            EnsureCleanDirectory(ArtifactsDirectory);
            if (IsServerBuild) return;
            // clear all old dll release
            foreach (var projectName in Projects)
            {
                var project = BuilderExtensions.GetProject(Solution, projectName);
                var binDirectory = (AbsolutePath)new DirectoryInfo(project.GetBinDirectory()).FullName;
                Console.WriteLine(binDirectory);
                binDirectory.GlobDirectories($"{BinOutputFolder}", $"{BuildConfiguration}*").ForEach(DeleteDirectory);
            }
        });
    
    Target CreateInstaller => _ => _
        .DependsOn(RestoreAndBuild)
        .OnlyWhenStatic(()=>IsLocalBuild || GitRepository.IsOnMainOrMasterBranch())
        .Executes(() =>
        {
            var configurations = Solution.GetConfigurations(InstallerConfiguration);
            configurations.ForEach(configuration =>
            {
                MSBuild(s => s
                    .SetTargets("Rebuild")
                    .SetProcessToolPath(MsBuildPath.Value)
                    .SetConfiguration(configuration)
                    .SetVerbosity(MSBuildVerbosity.Minimal)
                    .DisableNodeReuse()
                    .EnableRestore());
            });
        });



}
