namespace OpenMEPSandbox.Geometry;

public class Geometry
{
    private Geometry()
    {
    }

    /// <summary>
    /// Get Top Surface From Geometry
    /// </summary>
    /// <param name="geometry">the geometry to get top of surfaces</param>
    /// <returns name="surfaces">list top surface of geometry</returns>
    public static List<Autodesk.DesignScript.Geometry.Surface> TopSurfaces(
        Autodesk.DesignScript.Geometry.Geometry geometry)
    {
        List<Autodesk.DesignScript.Geometry.Surface> TopSurface = new List<Autodesk.DesignScript.Geometry.Surface>();
        Autodesk.DesignScript.Geometry.Geometry[] surfaces = geometry.Explode();
        foreach (var geometry1 in surfaces)
        {
            var g = (Autodesk.DesignScript.Geometry.Surface) geometry1;
            double d = g.NormalAtParameter(0.5, 0.5).Z;
            if (d > 0.1)
            {
                TopSurface.Add(g);
            }
        }

        return TopSurface;
    }

    /// <summary>
    /// Return Bottom Surface from Geometry
    /// </summary>
    /// <param name="geometry">the geometry get bottom surface</param>
    /// <returns name="surfaces">list surface bottom of geometry</returns>
    public static List<Autodesk.DesignScript.Geometry.Surface> BottomSurfaces(
        Autodesk.DesignScript.Geometry.Geometry geometry)
    {
        List<Autodesk.DesignScript.Geometry.Surface> BottomSurface = new List<Autodesk.DesignScript.Geometry.Surface>();
        Autodesk.DesignScript.Geometry.Geometry[] surfaces = geometry.Explode();
        foreach (var geometry1 in surfaces)
        {
            var g = (Autodesk.DesignScript.Geometry.Surface) geometry1;
            double d = g.NormalAtParameter(0.5, 0.5).Z;
            if (d < 0)
            {
                BottomSurface.Add(g);
            }
        }

        return BottomSurface;
    }

    /// <summary>
    /// Return Side Surface Vertical from Geometry
    /// </summary>
    /// <param name="geometry">the geometry to get side surface</param>
    /// <returns name="surface">list surface from size</returns>
    public static List<Autodesk.DesignScript.Geometry.Surface> SideSurface(
        Autodesk.DesignScript.Geometry.Geometry geometry)
    {
        List<Autodesk.DesignScript.Geometry.Surface> SideSurface = new List<Autodesk.DesignScript.Geometry.Surface>();
        Autodesk.DesignScript.Geometry.Geometry[] surfaces = geometry.Explode();
        // get all surface parallel to Z axis
        foreach (var geometry1 in surfaces)
        {
            var g = (Autodesk.DesignScript.Geometry.Surface) geometry1;
            double d = g.NormalAtParameter(0.5, 0.5).Z;
            if (d > -0.1 && d < 0.1)
            {
                SideSurface.Add(g);
            }
        }
        return SideSurface;
    }
}