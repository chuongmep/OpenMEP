﻿namespace OpenMEPSandbox.Geometry;

public class Geometry
{
    private Geometry()
    {
    }

    /// <summary>
    /// Returns the top surface of the provided geometry.
    /// </summary>
    /// <param name="geometry">The geometry to extract the top surface from.</param>
    /// <returns name="surfaces">A list of the top surface(s) of the geometry.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Geometry.TopSurfaces.png)
    /// [Geometry.TopSurfaces.dyn](../OpenMEPPage/geometry/dyn/Geometry.TopSurfaces.dyn)
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Surface> TopSurfaces(
        Autodesk.DesignScript.Geometry.Geometry geometry)
    {
        List<Autodesk.DesignScript.Geometry.Surface> TopSurface = new List<Autodesk.DesignScript.Geometry.Surface>();
        Autodesk.DesignScript.Geometry.Geometry[] surfaces = geometry.Explode();
        foreach (var surface in surfaces)
        {
            var g = (Autodesk.DesignScript.Geometry.Surface) surface;
            double d = g.NormalAtParameter(0.5, 0.5).Z;
            if (d > 0.1)
            {
                TopSurface.Add(g);
            }
        }

        return TopSurface;
    }

    /// <summary>
    /// Returns the bottom surface of the provided geometry.
    /// </summary>
    /// <param name="geometry">The geometry to extract the bottom surface from.</param>
    /// <returns name="surfaces">A list of the bottom surface(s) of the geometry.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Geometry.BottomSurfaces.png)
    /// [Geometry.BottomSurfaces.dyn](../OpenMEPPage/geometry/dyn/Geometry.BottomSurfaces.dyn)
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Surface> BottomSurfaces(
        Autodesk.DesignScript.Geometry.Geometry geometry)
    {
        List<Autodesk.DesignScript.Geometry.Surface> BottomSurface = new List<Autodesk.DesignScript.Geometry.Surface>();
        Autodesk.DesignScript.Geometry.Geometry[] surfaces = geometry.Explode();
        foreach (var surface in surfaces)
        {
            var g = (Autodesk.DesignScript.Geometry.Surface) surface;
            double d = g.NormalAtParameter(0.5, 0.5).Z;
            if (d < 0)
            {
                BottomSurface.Add(g);
            }
        }

        return BottomSurface;
    }

    /// <summary>
    /// Returns a list of side surfaces that are vertical to the provided geometry.
    /// </summary>
    /// <param name="geometry">The geometry to extract side surfaces from.</param>
    /// <returns name="surface">A list of side surfaces of the geometry.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Geometry.SideSurface.png)
    /// [Geometry.SideSurface.dyn](../OpenMEPPage/geometry/dyn/Geometry.SideSurface.dyn)
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Surface> SideSurface(
        Autodesk.DesignScript.Geometry.Geometry geometry)
    {
        List<Autodesk.DesignScript.Geometry.Surface> SideSurface = new List<Autodesk.DesignScript.Geometry.Surface>();
        Autodesk.DesignScript.Geometry.Geometry[] surfaces = geometry.Explode();
        // get all surface parallel to Z axis
        foreach (var surface in surfaces)
        {
            var g = (Autodesk.DesignScript.Geometry.Surface) surface;
            double d = g.NormalAtParameter(0.5, 0.5).Z;
            if (d > -0.1 && d < 0.1)
            {
                SideSurface.Add(g);
            }
        }
        return SideSurface;
    }
}