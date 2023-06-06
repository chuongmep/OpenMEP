using Autodesk.DesignScript.Geometry;

namespace OpenMEPSandbox.Autocad;

/// <summary>
/// Class for Text object
/// </summary>
public class Text
{
    private Text()
    {
    }
    
    /// <summary>
    /// Return the text string of the text object
    /// </summary>
    /// <param name="AcadText"></param>
    /// <returns></returns>
    public static string TextString(dynamic AcadText)
    {
        return AcadText.CadEntity.TextString;
    }

    /// <summary>
    /// Return Layer name of the text object
    /// </summary>
    /// <param name="AcadText"></param>
    /// <returns></returns>
    public static string Layer(dynamic AcadText)
    {
        return AcadText.CadEntity.Layer;
    }

    /// <summary>
    /// Return Rotation of the block reference
    /// </summary>
    /// <param name="AcadText">AcadText</param>
    /// <returns name="double">angle(degrees)</returns>
    public static double Rotation(dynamic AcadText)
    {
        double radRotate = AcadText.CadEntity.Rotation;
        // convert to degrees
        return radRotate * 180 / System.Math.PI;
    }
    
    /// <summary>
    /// Return Location Insert of the text object
    /// </summary>
    /// <param name="AcadText"></param>
    /// <returns name="point">location inserted </returns>
    /// <exception cref="Exception"></exception>
    public static Point InsertionPoint(dynamic AcadText)
    {
        double[] point = AcadText.CadEntity.InsertionPoint;
        if(point.Length != 3) throw new Exception("Insertion point is not 3D");
        return Point.ByCoordinates(point[0], point[1], point[2]);
    }
    /// <summary>
    /// Get Bounding box of the text object
    /// </summary>
    /// <param name="AcadText"></param>
    /// <returns name="BoundingBox"></returns>
    public static BoundingBox BoundingBox(dynamic AcadText)
    {
        AcadText.CadEntity.GetBoundingBox(out object minpoint,out object maxpoint);
        double[]? minPoints = minpoint as double[];
        double[]? maxPoints = maxpoint as double[];
        Point min = Point.ByCoordinates(minPoints[0], minPoints[1], minPoints[2]);
        Point max = Point.ByCoordinates(maxPoints[0], maxPoints[1], maxPoints[2]);
        return Autodesk.DesignScript.Geometry.BoundingBox.ByCorners(min, max);
    }
}