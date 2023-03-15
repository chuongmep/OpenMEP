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
    /// <param name="digits">The number of fractional digits in the return value.</param>
    /// <returns name="slope">value slope of line</returns>
    public static double Slope(Autodesk.DesignScript.Geometry.Line line,double digits=0)
    {
        Autodesk.DesignScript.Geometry.Point bottomPoint;
        Autodesk.DesignScript.Geometry.Point topPoint;
        Autodesk.DesignScript.Geometry.Point startPoint = line.StartPoint;
        Autodesk.DesignScript.Geometry.Point endPoint = line.EndPoint;
        if (Math.Abs(endPoint.Z - startPoint.Z) < 0.0001) return 0;
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
        if(project!.DistanceTo(bottomPoint)<0.0001) return 0;
        double slope = topPoint.DistanceTo(project) / bottomPoint.DistanceTo(project);
        return Math.Round(slope * 100,(int)digits);
    }
    
    /// <summary>
    /// get top point of line
    /// </summary>
    /// <param name="line">the line to get</param>
    /// <returns name="point">top point</returns>
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
    public static Autodesk.DesignScript.Geometry.Point BottomPoint(Autodesk.DesignScript.Geometry.Line line)
    {
        Autodesk.DesignScript.Geometry.Point startPoint = line.StartPoint;
        Autodesk.DesignScript.Geometry.Point endPoint = line.EndPoint;
        if (Math.Abs(startPoint.Z - endPoint.Z) < 0.001) throw new ArgumentException("The line is horizontal");
        if (startPoint.Z < endPoint.Z) return startPoint;
        return endPoint;
    }
}

