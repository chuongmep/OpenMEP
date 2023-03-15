namespace OpenMEPSandbox.Geometry;

public class Curve
{
    private Curve()
    {
        
    }
    
    /// <summary>
    /// Convert a curve simple to line
    /// </summary>
    /// <param name="curve">curve need to convert</param>
    /// <returns name="line">Line</returns>
    public static Autodesk.DesignScript.Geometry.Line AsLine(Autodesk.DesignScript.Geometry.Curve curve)
    {
        return Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(curve.StartPoint, curve.EndPoint);
    }
}