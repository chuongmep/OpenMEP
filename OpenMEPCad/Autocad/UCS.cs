using Autodesk.DesignScript.Geometry;

namespace OpenMEPCad.Autocad;

/// <summary>
/// UserCoordinateSystems class
/// </summary>
public class UCS
{
    private UCS()
    {
        
    }

    public static string Name( dynamic AcadUCS)
    {
        return AcadUCS.Name;
    }
    public static Point Origin( dynamic AcadUCS)
    {
        double[] origin = AcadUCS.Origin;
        return  Point.ByCoordinates(origin[0], origin[1], origin[2]);
    }
    public static dynamic GetUCSMatrix( dynamic AcadUCS)
    {
        return AcadUCS.GetUCSMatrix();
    }
}