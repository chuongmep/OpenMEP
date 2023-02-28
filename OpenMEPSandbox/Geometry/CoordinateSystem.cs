using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Geometry;

public class CoordinateSystem
{
    private CoordinateSystem()
    {
    }

    /// <summary>
    /// Shows scalable lines representing the CoordinateSystem axes and rectangles for the planes
    /// </summary>
    /// <param name="coordinateSystem">Autodesk.DesignScript.Geometry.CoordinateSystem</param>
    /// <param name="length">double</param>
    /// <returns name="Display">GeometryColor</returns>
    /// <returns name="Origin">Point</returns>
    /// <returns name="XAxis">Vector</returns>
    /// <returns name="YAxis">Vector</returns>
    /// <returns name="ZAxis">Vector</returns>
    /// <returns name="XYPlane">Plane</returns>
    /// <returns name="YZPlane">Plane</returns>
    /// <returns name="ZXPlane">Plane</returns>
    [MultiReturn(new[] {"Display", "Origin", "XAxis", "YAxis", "ZAxis", "XYPlane", "YZPlane", "ZXPlane"})]
    public static Dictionary<string, object?> Display(Autodesk.DesignScript.Geometry.CoordinateSystem coordinateSystem,
        double length = 1000)
    {
        if (length <= 0)
        {
            length = 1;
        }
        var pt = coordinateSystem.Origin;
        var lineX = Autodesk.DesignScript.Geometry.Line.ByStartPointDirectionLength(pt, coordinateSystem.XAxis, length);
        var colorX = DSCore.Color.ByARGB(255, 255, 0, 0);
        var lineY = Autodesk.DesignScript.Geometry.Line.ByStartPointDirectionLength(pt, coordinateSystem.YAxis, length);
        var colorY = DSCore.Color.ByARGB(255, 0, 255, 0);
        var lineZ = Autodesk.DesignScript.Geometry.Line.ByStartPointDirectionLength(pt, coordinateSystem.ZAxis, length);
        var colorZ = DSCore.Color.ByARGB(255, 0, 0, 255);
        List<Modifiers.GeometryColor> display = new List<Modifiers.GeometryColor>();
        display.Add(Modifiers.GeometryColor.ByGeometryColor(lineX, colorX));
        display.Add(Modifiers.GeometryColor.ByGeometryColor(lineY, colorY));
        display.Add(Modifiers.GeometryColor.ByGeometryColor(lineZ, colorZ));
        var d = new Dictionary<string, object?>();
        d.Add("Display", display);
        d.Add("Origin", pt);
        d.Add("XAxis", coordinateSystem.XAxis);
        d.Add("YAxis", coordinateSystem.YAxis);
        d.Add("ZAxis", coordinateSystem.ZAxis);
        d.Add("XYPlane", coordinateSystem.XYPlane);
        d.Add("YZPlane", coordinateSystem.YZPlane);
        d.Add("ZXPlane", coordinateSystem.ZXPlane);
        return d;
    }
}