using Autodesk.DesignScript.Runtime;
using Dynamo.Graph.Nodes;

namespace OpenMEPSandbox.Geometry;

public class Vector
{
    private Vector()
    {
        
    }
    
    /// <summary>
    /// Check whether two vector is same direction or not
    /// </summary>
    /// <param name="v1">the first vector</param>
    /// <param name="v2">the second vector</param>
    /// <returns name="bool">true if two vector is same direction</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Vector.IsSameDirection.gif)
    /// </example>
    [NodeCategory("Query")]
    public static bool IsSameDirection(Autodesk.DesignScript.Geometry.Vector v1,Autodesk.DesignScript.Geometry.Vector v2)
    {
        return v1.Normalized().Dot(v2.Normalized()).Equals(1);
    }
    /// <summary>
    /// Check two Vector is opposite direction or not
    /// </summary>
    /// <param name="v1">the first vector</param>
    /// <param name="v2">the second vector</param>
    /// <returns name="bool">true if two vector is opposite</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Vector.IsOppositeDirection.gif)
    /// </example>
    [NodeCategory("Query")]
    public static bool IsOppositeDirection(Autodesk.DesignScript.Geometry.Vector v1, Autodesk.DesignScript.Geometry.Vector v2)
    {
        return (v1.Normalized().Dot(v2.Normalized())).Equals(-1);
    }
    
    /// <summary>
    /// Evaluate whether two Vector is perpendicular or not
    /// </summary>
    /// <param name="v1">first vector</param>
    /// <param name="v2">second vector</param>
    /// <returns name="bool">true if two vector is perpendicular</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Vector.IsPerpendicular.png)
    /// </example>
    public static bool IsPerpendicular(Autodesk.DesignScript.Geometry.Vector v1, Autodesk.DesignScript.Geometry.Vector v2)
    {
        return v1.Normalized().Dot(v2.Normalized()).Equals(0);
    }
    
    /// <summary>
    /// Shows a scalable line representing a Vector from a chosen starting point
    /// </summary>
    /// <param name="vector">Autodesk.DesignScript.Geometry.Vector</param>
    /// <param name="startPoint">Autodesk.DesignScript.Geometry.Point</param>
    /// <param name="scale">value scale start from 1</param>
    /// <returns name="Display">GeometryColor</returns>
    /// <returns name="X">double</returns>
    /// <returns name="Y">double</returns>
    /// <returns name="Z">double</returns>
    /// <returns name="Length">double</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Vector.Display.gif)
    /// </example>
    [MultiReturn(new[] { "Display", "X", "Y", "Z", "Length" })]
    public static Dictionary<string, object?> Display(Autodesk.DesignScript.Geometry.Vector vector, Autodesk.DesignScript.Geometry.Point startPoint, double scale = 1000)
    {
        if (scale <= 0)
        {
            scale = 1;
        }
        var line = Autodesk.DesignScript.Geometry.Line.ByStartPointDirectionLength(startPoint, vector, vector.Length * scale);
        var color = DSCore.Color.ByARGB(255, 255, 0, 0);
        List<Modifiers.GeometryColor> display = new List<Modifiers.GeometryColor>();
        display.Add(Modifiers.GeometryColor.ByGeometryColor(line, color));
        var d = new Dictionary<string, object?>();
        d.Add("Display", display);
        d.Add("X", vector.X);
        d.Add("Y", vector.Y);
        d.Add("Z", vector.Z);
        d.Add("Length", vector.Length);
        return d;
    }
    
    
}