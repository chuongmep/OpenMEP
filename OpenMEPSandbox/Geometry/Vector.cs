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
    /// [Vector.IsSameDirection.dyn](../OpenMEPPage/geometry/dyn/Vector.IsSameDirection.dyn)
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
    /// [Vector.IsOppositeDirection.dyn](../OpenMEPPage/geometry/dyn/Vector.IsOppositeDirection.dyn)
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
    /// [Vector.IsPerpendicular.dyn](../OpenMEPPage/geometry/dyn/Vector.IsPerpendicular.dyn)
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
    /// [Vector.Display.dyn](../OpenMEPPage/geometry/dyn/Vector.Display.dyn)
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

    /// <summary>
    /// Compares this a vector with another vector.
    /// 0: if this is identical to other
    /// 1: if this is greater than other
    /// -1: if this is less than other
    /// </summary>
    /// <param name="v1">the first vector</param>
    /// <param name="v2">the second vector</param>
    /// <param name="tolerance">the tolerance compare two vector</param>
    /// <returns name="double">value compare between two vector</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Vector.CompareTo.png)
    /// [](../OpenMEPPage/geometry/dyn/Vector.CompareTo.dyn)
    /// </example>
    public static double CompareTo(Autodesk.DesignScript.Geometry.Vector v1, Autodesk.DesignScript.Geometry.Vector v2,double tolerance = 0.001)
    {
        if (System.Math.Abs(v1.X - v2.X) > tolerance)
            return v1.X < v2.X ? -1 : 1;
        if (System.Math.Abs(v1.Y - v2.Y) > tolerance)
            return v1.Y < v2.Y ? -1 : 1;
        if (System.Math.Abs(v1.Z - v2.Z) > tolerance)
            return v1.Z < v2.Z ? -1 : 1;
        return 0;
    }
    
    /// <summary>
    /// Multiply two vector
    /// </summary>
    /// <param name="v1">the first vector</param>
    /// <param name="v2">the second vector</param>
    /// <returns name="vector"></returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Vector.Multiply.png)
    /// [Vector.Multiply.dyn](../OpenMEPPage/geometry/dyn/Vector.Multiply.dyn)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Vector Multiply(Autodesk.DesignScript.Geometry.Vector v1, Autodesk.DesignScript.Geometry.Vector v2)
    {
        return Autodesk.DesignScript.Geometry.Vector.ByCoordinates(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
    }
    
    /// <summary>
    ///  Divide two vector
    /// </summary>
    /// <param name="v1">the first vector</param>
    /// <param name="v2">the second vector</param>
    /// <returns name="vector"></returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Vector.Divide.png)
    /// [Vector.Divide.dyn](../OpenMEPPage/geometry/dyn/Vector.Divide.dyn)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Vector Divide(Autodesk.DesignScript.Geometry.Vector v1, Autodesk.DesignScript.Geometry.Vector v2)
    {
        return Autodesk.DesignScript.Geometry.Vector.ByCoordinates(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
    }
    /// <summary>
    /// Returns a new Autodesk.DesignScript.Geometry.Vector object with the largest integer values that are less than or equal to the X, Y, and Z components of the input vector.
    /// </summary>
    /// <param name="vector">The input vector.</param>
    /// <returns>The Autodesk.DesignScript.Geometry.Vector object with the largest integer values that are less than or equal to the X, Y, and Z components of the input vector.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Vector.Ceiling.png)
    /// [Vector.Ceiling.dyn](../OpenMEPPage/geometry/dyn/Vector.Ceiling.dyn)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Vector Ceiling(Autodesk.DesignScript.Geometry.Vector vector)
    {
        int x = (int)System.Math.Ceiling(vector.X);
        int y = (int)System.Math.Ceiling(vector.Y);
        int z = (int)System.Math.Ceiling(vector.Z);
        return Autodesk.DesignScript.Geometry.Vector.ByCoordinates(x, y, z);
    }

    /// <summary>
    /// Returns a new Autodesk.DesignScript.Geometry.Vector object with the smallest integer values that are greater than or equal to the X, Y, and Z components of the input vector.
    /// </summary>
    /// <param name="vector">The input vector.</param>
    /// <returns>The Autodesk.DesignScript.Geometry.Vector object with the smallest integer values that are greater than or equal to the X, Y, and Z components of the input vector.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Vector.Floor.png)
    /// [Vector.Floor.dyn](../OpenMEPPage/geometry/dyn/Vector.Floor.dyn)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Vector Floor(Autodesk.DesignScript.Geometry.Vector vector)
    {
        int x = (int)System.Math.Floor(vector.X);
        int y = (int)System.Math.Floor(vector.Y);
        int z = (int)System.Math.Floor(vector.Z);

        return Autodesk.DesignScript.Geometry.Vector.ByCoordinates(x, y, z);
    }
}