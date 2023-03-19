using Autodesk.DesignScript.Runtime;
using OpenMEPSandbox.Helpers;

namespace OpenMEPSandbox.Geometry;

public class Line
{
    private Line()
    {
        
    }
    
    /// <summary>
    /// Return slope of line
    /// </summary>
    /// <param name="line">line to get slope</param>
    /// <param name="digits">Number of fractional digits in the return value</param>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Line.Slope.png)
    /// </example>
    [MultiReturn("Percent", "Degrees", "Ratio")]
    public static Dictionary<string, object?> Slope(Autodesk.DesignScript.Geometry.Curve? line,double digits=0)
    {
        Autodesk.DesignScript.Geometry.Point bottomPoint;
        Autodesk.DesignScript.Geometry.Point topPoint;
        Autodesk.DesignScript.Geometry.Point startPoint = line.StartPoint;
        Autodesk.DesignScript.Geometry.Point endPoint = line.EndPoint;
        if(startPoint.Z<endPoint.Z)
        {
            bottomPoint = startPoint;
            topPoint = endPoint;
        }
        else
        {
            bottomPoint = endPoint;
            topPoint = startPoint;
        }
        Autodesk.DesignScript.Geometry.Plane plane =
            Autodesk.DesignScript.Geometry.Plane.ByOriginNormal(bottomPoint,
                Autodesk.DesignScript.Geometry.Vector.ZAxis());
        var project = topPoint.ToGSharkType().ProjectToPlan(plane.ToGSharkType()).ToDynamoType();
        double slope = topPoint.DistanceTo(project) / bottomPoint.DistanceTo(project);
        double per = Math.Round(slope * 100, (int) digits);
        double deg = Math.Round(Math.Atan(slope) * 180 / Math.PI, (int) digits);
        return new Dictionary<string, object?>()
        {
            {"Percent", per},
            {"Degrees", deg},
            {"Ratio", slope}
        };
    }
    
    /// <summary>
    /// get top point of line
    /// </summary>
    /// <param name="line">the line to get</param>
    /// <returns name="point">top point</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Line.TopPoint.png)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Point TopPoint(Autodesk.DesignScript.Geometry.Line line)
    {
        Autodesk.DesignScript.Geometry.Point startPoint = line.StartPoint;
        Autodesk.DesignScript.Geometry.Point endPoint = line.EndPoint;
        if(Math.Abs(startPoint.Z - endPoint.Z) < 0.001) throw new ArgumentException("The line is horizontal");
        if (startPoint.Z > endPoint.Z) return startPoint;
        return endPoint;
    }
    /// <summary>
    /// get bottom point of line
    /// </summary>
    /// <param name="line">the line to get</param>
    /// <returns name="point">bottom point</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Line.BottomPoint.png)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Point BottomPoint(Autodesk.DesignScript.Geometry.Line line)
    {
        Autodesk.DesignScript.Geometry.Point startPoint = line.StartPoint;
        Autodesk.DesignScript.Geometry.Point endPoint = line.EndPoint;
        if (Math.Abs(startPoint.Z - endPoint.Z) < 0.001) throw new ArgumentException("The line is horizontal");
        if (startPoint.Z < endPoint.Z) return startPoint;
        return endPoint;
    }
    
}

