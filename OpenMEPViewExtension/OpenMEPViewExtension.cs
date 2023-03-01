using System.Reflection;
using Dynamo.Wpf.Extensions;

namespace OpenMEPViewExtension;

public class OpenMEPViewExtension : IViewExtension
{
    public void Dispose()
    {
    }

    public void Startup(ViewStartupParams viewStartupParams)
    {
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
    }

    private Assembly? CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
    {
        // Get assembly name
        var assemblyName = new AssemblyName(args.Name).Name + ".dll";

        // Get resource name
        var resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(x => x.EndsWith(".dll"))
            .ToArray().FirstOrDefault(x => x.EndsWith(assemblyName));
        if (resourceName == null)
        {
            return null;
        }

        // Load assembly from resource
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
        {
            var bytes = new byte[stream!.Length];
            stream.Read(bytes, 0, bytes.Length);
            return Assembly.Load(bytes);
        }
    }

    public void Loaded(ViewLoadedParams viewLoadedParams)
    {
    }

    public void Shutdown()
    {
    }

    public string UniqueId => "4811E3B2-1B98-4E9E-B34F-BD62AAFFF367";
    public string Name => "Open MEP View Extension";
}