using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Autocad;

public class Layer
{
    private Layer()
    {
        
    }
    
    /// <summary>
    /// Gets the name of the layer.
    /// </summary>
    /// <param name="AcadLayer">AcadLayer</param>
    /// <returns name="string">the name of layer</returns>
    public static string Name (dynamic AcadLayer)
    {
        return AcadLayer.Name;
    }
    public static bool Used(dynamic AcadLayer)
    {
        return AcadLayer.Used;
    }
    public static bool Lock(dynamic AcadLayer)
    {
        return AcadLayer.Lock;
    }
    public static bool Freeze(dynamic AcadLayer)
    {
        return AcadLayer.Freeze;
    }
    public static bool LayerOn(dynamic AcadLayer)
    {
        return AcadLayer.LayerOn;
    }
    public static double Lineweight(dynamic AcadLayer)
    {
        return AcadLayer.Lineweight;
    }
    public static string Linetype(dynamic AcadLayer)
    {
        return AcadLayer.Linetype;
    }
    public static string PlotStyleName(dynamic AcadLayer)
    {
        return AcadLayer.PlotStyleName;
    }
    public static string Material(dynamic AcadLayer)
    {
        return AcadLayer.Material;
    }
    public static dynamic Color(dynamic AcadLayer)
    {
        return AcadLayer.color;
    }
    
    [MultiReturn("Name","EntityColor","ColorIndex","Red","Green","Blue","TrueColor")]
    public static Dictionary<string,object> TrueColor(dynamic AcadLayer)
    {
        return new Dictionary<string, object>()
        {
            {"Name",AcadLayer.TrueColor.ColorName},
            {"EntityColor",AcadLayer.TrueColor.EntityColor},
            {"ColorIndex",AcadLayer.TrueColor.ColorIndex},
            {"Red",AcadLayer.TrueColor.Red},
            {"Green",AcadLayer.TrueColor.Green},
            {"Blue",AcadLayer.TrueColor.Blue},
            {"TrueColor",AcadLayer.TrueColor}
        };
    }
    public static bool ViewportDefault(dynamic AcadLayer)
    {
        return AcadLayer.ViewportDefault;
    }
    public static bool HasExtensionDictionary(dynamic AcadLayer)
    {
        return AcadLayer.HasExtensionDictionary;
    }
    public static string Description(dynamic AcadLayer)
    {
        return AcadLayer.Description;
    }
}