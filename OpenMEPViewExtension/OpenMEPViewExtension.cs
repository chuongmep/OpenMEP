using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;
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
        // Create a new menu item under Packages menu item
        var menuItem = new MenuItem {Header = "Open MEP Help"};
        menuItem.Click += (sender, args) =>
        {
            // Get current selected node
            // var vm = viewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;
            // if(vm == null) return;
            // var node = vm?.CurrentSpaceViewModel.Model.CurrentSelection.FirstOrDefault();
            // if(node == null) return;
            // string NodeEndPoint = node.CreationName;
            // string pattern = @"\.\w+@\w+(\.\w+)*,\w+";
            // var MainNameSpace = Regex.Replace(NodeEndPoint, pattern, string.Empty);
            // //  replace all character . @ in NodeEndPoint become _
            // MessageBox.Show(MainNameSpace);
            //
            // return;
            // string pattern2 = @"[.@,]";
            // string direction = Regex.Replace(NodeEndPoint, pattern2, "_");
            //MessageBox.Show(direction);
            // Open a new window
            string url = $"https://chuongmep.github.io/OpenMEP";
            //api/{MainNameSpace}.html#{direction}";
            Process.Start(url);
        };
#if R20 || R21
        viewLoadedParams.AddMenuItem(MenuBarType.Help, menuItem);
        #else
        viewLoadedParams.AddExtensionMenuItem(menuItem);
#endif
        
    }

    public void Shutdown()
    {
    }

    public string UniqueId => "4811E3B2-1B98-4E9E-B34F-BD62AAFFF367";
    public string Name => "Open MEP View Extension";
}