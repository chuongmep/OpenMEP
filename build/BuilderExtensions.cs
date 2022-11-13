using System.Text.RegularExpressions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;

namespace _build;

public static class BuilderExtensions
{
    public static Project GetProject(this Solution solution, string projectName) =>
        solution.GetProject(projectName) ?? throw new NullReferenceException($"Cannon find project \"{projectName}\"");

    public static AbsolutePath GetBinDirectory(this Project project) => project.Directory / "bin";

    private static AbsolutePath GetExePath(this Project project, string configuration) =>
        project.GetBinDirectory() / configuration / $"{project.Name}.exe";

    public static AbsolutePath GetExecutableFile(this Project project, IEnumerable<string> configurations,
        List<DirectoryInfo> directories)
    {
        var directory = directories[0].Name;
        var subConfigRegex = new Regex(@"R\d+$");
        foreach (var subCategory in configurations.Select(configuration =>
                     configuration.Replace(Build.InstallerConfiguration, "")))
            if (string.IsNullOrEmpty(subCategory))
            {
                if (!string.IsNullOrEmpty(subConfigRegex.Match(directory).Value))
                    return project.GetExePath(Build.BuildConfiguration);
            }
            else
            {
                if (directory.EndsWith(subCategory))
                    return project.GetExePath($"{Build.BuildConfiguration}{subCategory}");
            }

        return null;
    }

    public static List<string> GetConfigurations(this Solution solution, params string[] startPatterns)
    {
        var configurations = solution.Configurations
            .Select(pair => pair.Key)
            .Where(s => startPatterns.Any(s.StartsWith))
            .Select(s =>
            {
                var platformIndex = s.LastIndexOf('|');
                return s.Remove(platformIndex);
            })
            .ToList();
        if (configurations.Count == 0)
            throw new Exception(
                $"Can't find configurations in the solution by patterns: {string.Join(" | ", startPatterns)}.");
        return configurations;
    }
    
    public static IEnumerable<IGrouping<string, DirectoryInfo>> GetBuildDirectories(this Solution solution,IEnumerable<string> projectNames)
    {
        var directories = new List<DirectoryInfo>();
        foreach (var projectName in projectNames)
        {
            var project = GetProject(solution, projectName);
            var directoryInfo = new DirectoryInfo(project.GetBinDirectory()).GetDirectories();
            directories.AddRange(directoryInfo);
        }

        if (directories.Count == 0) throw new Exception("There are no packaged assemblies in the project. Try to build the project again.");

        var versionRegex = new Regex(@"^.*R\d+ ?");
        var addInsDirectory = directories
            .Where(dir => dir.Name.StartsWith("Release"))
            .GroupBy(dir => versionRegex.Replace(dir.Name, string.Empty));

        return addInsDirectory;
    }
}