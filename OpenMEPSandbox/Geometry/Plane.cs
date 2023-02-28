using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Geometry;

public class Plane
{
    private Plane()
    {
    }

    /// <summary>
    /// Shows scalable lines representing the axes and a rectangle for the Plane
    /// </summary>
    /// <param name="plane">Autodesk.DesignScript.Geometry.Plane</param>
    /// <param name="length">double</param>
    /// <returns name="Display">GeometryColor</returns>
    /// <returns name="Origin">Point</returns>
    /// <returns name="XAxis">Vector</returns>
    /// <returns name="YAxis">Vector</returns>
    /// <returns name="Normal">Vector</returns>
    [MultiReturn(new[] {"Display", "Origin", "XAxis", "YAxis", "Normal"})]
    public static Dictionary<string, object?> Display(Autodesk.DesignScript.Geometry.Plane? plane, double length = 1000)
    {
        if (length <= 0)
        {
            length = 1;
        }
        if (plane == null) return new Dictionary<string, object?>();
        var pt = plane.Origin;
        var lineX = Autodesk.DesignScript.Geometry.Line.ByStartPointDirectionLength(pt, plane.XAxis, length);
        var colorX = DSCore.Color.ByARGB(255, 255, 0, 0);
        var lineY = Autodesk.DesignScript.Geometry.Line.ByStartPointDirectionLength(pt, plane.YAxis, length);
        var colorY = DSCore.Color.ByARGB(255, 0, 255, 0);
        var lineN = Autodesk.DesignScript.Geometry.Line.ByStartPointDirectionLength(pt, plane.Normal, length);
        var colorN = DSCore.Color.ByARGB(255, 0, 0, 255);
        var rect = Rectangle.ByWidthLength(plane, length, length);
        var colorR = DSCore.Color.ByARGB(50, 50, 50, 50);
        List<Modifiers.GeometryColor> display = new List<Modifiers.GeometryColor>();
        display.Add(Modifiers.GeometryColor.ByGeometryColor(lineX, colorX));
        display.Add(Modifiers.GeometryColor.ByGeometryColor(lineY, colorY));
        display.Add(Modifiers.GeometryColor.ByGeometryColor(lineN, colorN));
        display.Add(Modifiers.GeometryColor.ByGeometryColor(rect, colorR));
        var d = new Dictionary<string, object?>();
        d.Add("Display", display);
        d.Add("Origin", pt);
        d.Add("XAxis", plane.XAxis);
        d.Add("YAxis", plane.YAxis);
        d.Add("Normal", plane.Normal);
        return d;
    }
}