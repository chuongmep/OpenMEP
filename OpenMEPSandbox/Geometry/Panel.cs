using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using OpenMEPSandbox.Helpers;
using OpenMEPSandbox.Helpers.CLASSES;

namespace OpenMEPSandbox.Geometry;

public class Panel
{
    /// <summary>
    /// Paneling Systems
    /// Use Open Source From lunchbox package : https://bitbucket.org/provingground-io/lunchbox-for-dynamo/src/master/
    /// </summary>
    private Panel()
    {
    }

    /// <summary>
    /// Create quad panels
    /// </summary>
    /// <param name="srf">Surface</param>
    /// <param name="u">U Divisions</param>
    /// <param name="v">V Divisions</param>
    /// <returns name = "Panels">Panel surfaces</returns>
    /// <returns name = "Points">Panel points</returns>
    [MultiReturn(new[] {"Panels", "Points"})]
    public static Dictionary<string, object> PanelQuad(Autodesk.DesignScript.Geometry.Surface srf, int u, int v)
    {
        List<List<Autodesk.DesignScript.Geometry.Point>> mPoints =
            new List<List<Autodesk.DesignScript.Geometry.Point>>();
        List<Autodesk.DesignScript.Geometry.Surface> mPanels = new List<Autodesk.DesignScript.Geometry.Surface>();

        TesselationUtils tess = new TesselationUtils();
        List<clsQuad>? mQuads = tess.PanelQuad(u, v);
        foreach (clsQuad q in mQuads!)
        {
            List<Autodesk.DesignScript.Geometry.Point> mPts = new List<Autodesk.DesignScript.Geometry.Point>();
            mPts.Add(srf.PointAtParameter(q.A.X, q.A.Y));
            mPts.Add(srf.PointAtParameter(q.B.X, q.B.Y));
            mPts.Add(srf.PointAtParameter(q.C.X, q.C.Y));
            mPts.Add(srf.PointAtParameter(q.D.X, q.D.Y));

            Autodesk.DesignScript.Geometry.Surface panel =
                Autodesk.DesignScript.Geometry.Surface.ByPerimeterPoints(mPts);

            mPoints.Add(mPts);
            mPanels.Add(panel);
        }

        return new Dictionary<string, object>
        {
            {"Panels", mPanels},
            {"Points", mPoints},
        };
    }

    /// <summary>
    /// Create staggered quad panels
    /// </summary>
    /// <param name="srf">Surface</param>
    /// <param name="u">U Divisions</param>
    /// <param name="v">V Divisions</param>
    /// <returns name = "Panels">Panel surfaces</returns>
    /// <returns name = "Points">Panel points</returns>
    [MultiReturn(new[] {"Panels", "Points"})]
    public static Dictionary<string, object> PanelQuadStaggered(Autodesk.DesignScript.Geometry.Surface srf, int u,
        int v)
    {
        List<List<Autodesk.DesignScript.Geometry.Point>> mPoints =
            new List<List<Autodesk.DesignScript.Geometry.Point>>();
        List<Autodesk.DesignScript.Geometry.Surface> mPanels = new List<Autodesk.DesignScript.Geometry.Surface>();

        TesselationUtils tess = new TesselationUtils();
        List<clsQuad>? mQuads = tess.PanelQuadStaggered(u, v);
        foreach (clsQuad q in mQuads!)
        {
            List<Autodesk.DesignScript.Geometry.Point> mPts = new List<Autodesk.DesignScript.Geometry.Point>();
            mPts.Add(srf.PointAtParameter(q.A.X, q.A.Y));
            mPts.Add(srf.PointAtParameter(q.B.X, q.B.Y));
            mPts.Add(srf.PointAtParameter(q.C.X, q.C.Y));
            mPts.Add(srf.PointAtParameter(q.D.X, q.D.Y));

            Autodesk.DesignScript.Geometry.Surface panel =
                Autodesk.DesignScript.Geometry.Surface.ByPerimeterPoints(mPts);

            mPoints.Add(mPts);
            mPanels.Add(panel);
        }

        return new Dictionary<string, object>
        {
            {"Panels", mPanels},
            {"Points", mPoints},
        };
    }

    /// <summary>
    /// Create randomized quad panels
    /// </summary>
    /// <param name="srf">Surface</param>
    /// <param name="u">U Divisions</param>
    /// <param name="v">V Divisions</param>
    /// <param name="s">Random Seed</param>
    /// <returns name = "Panels">Panel surfaces</returns>
    /// <returns name = "Points">Panel points</returns>
    [MultiReturn(new[] {"Panels", "Points"})]
    public static Dictionary<string, object> PanelQuadRandom(Autodesk.DesignScript.Geometry.Surface srf, int u, int v,
        int s)
    {
        List<List<Autodesk.DesignScript.Geometry.Point>> m_points =
            new List<List<Autodesk.DesignScript.Geometry.Point>>();
        List<Surface?> m_panels = new List<Surface?>();

        TesselationUtils tess = new TesselationUtils();
        List<clsQuadVar>? mQuads = tess.RandomQuad(u, v, s);
        foreach (clsQuadVar q in mQuads!)
        {
            if (q.RowCount > 1)
            {
                List<NurbsCurve> crvs = new List<NurbsCurve>();
                List<Autodesk.DesignScript.Geometry.Point> dsPts = new List<Autodesk.DesignScript.Geometry.Point>();
                List<clsPoint> pts = q.Points;
                int rowCount = q.RowCount;
                IEnumerable<clsPoint> ptsA = pts.Take(rowCount);
                IEnumerable<clsPoint> ptsB = pts.Skip(rowCount).Take(rowCount);

                List<Autodesk.DesignScript.Geometry.Point> dsPtsA = new List<Autodesk.DesignScript.Geometry.Point>();
                foreach (clsPoint p in ptsA)
                {
                    Autodesk.DesignScript.Geometry.Point dsPt = srf.PointAtParameter(p.X, p.Y);
                    dsPtsA.Add(dsPt);
                    dsPts.Add(dsPt);
                }

                List<Autodesk.DesignScript.Geometry.Point> dsPtsB = new List<Autodesk.DesignScript.Geometry.Point>();
                foreach (clsPoint p in ptsB)
                {
                    Autodesk.DesignScript.Geometry.Point dsPt = srf.PointAtParameter(p.X, p.Y);
                    dsPtsB.Add(dsPt);
                    dsPts.Add(dsPt);
                }

                crvs.Add(NurbsCurve.ByPoints(dsPtsA));
                crvs.Add(NurbsCurve.ByPoints(dsPtsB));

                Surface? panel = null;
                try
                {
                    panel = Autodesk.DesignScript.Geometry.Surface.ByLoft(crvs);
                }
                catch (Exception)
                {
                }


                m_points.Add(dsPts);
                m_panels.Add(panel);
            }
        }

        return new Dictionary<string, object>
        {
            {"Panels", m_panels},
            {"Points", m_points},
        };
    }

    /// <summary>
    /// Create scewed quad panels
    /// </summary>
    /// <param name="srf">Surface</param>
    /// <param name="u">U Divisions</param>
    /// <param name="v">V Divisions</param>
    /// <param name="t">Parameter for adjusted scewing amount (0-1)</param>
    /// <returns name = "QuadPanels">Quad Panel surfaces</returns>
    /// <returns name = "QuadPoints">Quad Panel points</returns>
    /// <returns name = "TriPanels">Tri Panel surfaces</returns>
    /// <returns name = "TriPoints">Tri Panel points</returns>
    [MultiReturn(new[] {"QuadPanels", "QuadPoints", "TriPanels", "TriPoints"})]
    public static Dictionary<string, object> PanelQuadScewed(Autodesk.DesignScript.Geometry.Surface srf, int u, int v,
        double t)
    {
        List<List<Autodesk.DesignScript.Geometry.Point>> m_quadpoints =
            new List<List<Autodesk.DesignScript.Geometry.Point>>();
        List<Autodesk.DesignScript.Geometry.Surface> m_quadpanels = new List<Autodesk.DesignScript.Geometry.Surface>();

        List<List<Autodesk.DesignScript.Geometry.Point>> m_tripoints =
            new List<List<Autodesk.DesignScript.Geometry.Point>>();
        List<Autodesk.DesignScript.Geometry.Surface> m_tripanels = new List<Autodesk.DesignScript.Geometry.Surface>();

        TesselationUtils tess = new TesselationUtils();
        clsPanelCollection? mPanelcollection = tess.PanelQuadScewed(u, v, t);
        foreach (clsQuad q in mPanelcollection!.Quads)
        {
            List<Autodesk.DesignScript.Geometry.Point> m_pts = new List<Autodesk.DesignScript.Geometry.Point>();
            m_pts.Add(srf.PointAtParameter(q.A.X, q.A.Y));
            m_pts.Add(srf.PointAtParameter(q.B.X, q.B.Y));
            m_pts.Add(srf.PointAtParameter(q.C.X, q.C.Y));
            m_pts.Add(srf.PointAtParameter(q.D.X, q.D.Y));

            Autodesk.DesignScript.Geometry.Surface panel =
                Autodesk.DesignScript.Geometry.Surface.ByPerimeterPoints(m_pts);

            m_quadpoints.Add(m_pts);
            m_quadpanels.Add(panel);
        }

        foreach (clsTriangle q in mPanelcollection.Triangles!)
        {
            List<Autodesk.DesignScript.Geometry.Point> mPts = new List<Autodesk.DesignScript.Geometry.Point>();
            mPts.Add(srf.PointAtParameter(q.A.X, q.A.Y));
            mPts.Add(srf.PointAtParameter(q.B.X, q.B.Y));
            mPts.Add(srf.PointAtParameter(q.C.X, q.C.Y));

            Autodesk.DesignScript.Geometry.Surface panel =
                Autodesk.DesignScript.Geometry.Surface.ByPerimeterPoints(mPts);

            m_tripoints.Add(mPts);
            m_tripanels.Add(panel);
        }

        return new Dictionary<string, object>
        {
            {"QuadPanels", m_quadpanels},
            {"QuadPoints", m_quadpoints},
            {"TriPanels", m_quadpanels},
            {"TriPoints", m_quadpoints},
        };
    }

    /// <summary>
    /// Create diamond panels
    /// </summary>
    /// <param name="srf">Surface</param>
    /// <param name="u">U Divisions</param>
    /// <param name="v">V Divisions</param>
    /// <returns name = "QuadPanels">Quad Panel surfaces</returns>
    /// <returns name = "QuadPoints">Quad Panel points</returns>
    /// <returns name = "TriPanels">Tri Panel surfaces</returns>
    /// <returns name = "TriPoints">Tri Panel points</returns>
    [MultiReturn(new[] {"QuadPanels", "QuadPoints", "TriPanels", "TriPoints"})]
    public static Dictionary<string, object> PanelDiamond(Autodesk.DesignScript.Geometry.Surface srf, int u, int v)
    {
        List<List<Autodesk.DesignScript.Geometry.Point>> m_quadpoints =
            new List<List<Autodesk.DesignScript.Geometry.Point>>();
        List<Autodesk.DesignScript.Geometry.Surface> m_quadpanels = new List<Autodesk.DesignScript.Geometry.Surface>();

        List<List<Autodesk.DesignScript.Geometry.Point>> m_tripoints =
            new List<List<Autodesk.DesignScript.Geometry.Point>>();
        List<Autodesk.DesignScript.Geometry.Surface> m_tripanels = new List<Autodesk.DesignScript.Geometry.Surface>();

        TesselationUtils tess = new TesselationUtils();
        clsPanelCollection? mPanelcollection = tess.PanelDiamond(u, v);
        foreach (clsQuad q in mPanelcollection!.Quads)
        {
            try
            {
                List<Autodesk.DesignScript.Geometry.Point> mPts = new List<Autodesk.DesignScript.Geometry.Point>();
                mPts.Add(srf.PointAtParameter(q.A.X, q.A.Y));
                mPts.Add(srf.PointAtParameter(q.B.X, q.B.Y));
                mPts.Add(srf.PointAtParameter(q.C.X, q.C.Y));
                mPts.Add(srf.PointAtParameter(q.D.X, q.D.Y));

                Autodesk.DesignScript.Geometry.Surface panel =
                    Autodesk.DesignScript.Geometry.Surface.ByPerimeterPoints(mPts);

                m_quadpoints.Add(mPts);
                m_quadpanels.Add(panel);
            }
            catch
            {
            }
        }

        foreach (clsTriangle q in mPanelcollection.Triangles!)
        {
            try
            {
                List<Autodesk.DesignScript.Geometry.Point> m_pts = new List<Autodesk.DesignScript.Geometry.Point>();
                m_pts.Add(srf.PointAtParameter(q.A.X, q.A.Y));
                m_pts.Add(srf.PointAtParameter(q.B.X, q.B.Y));
                m_pts.Add(srf.PointAtParameter(q.C.X, q.C.Y));

                Autodesk.DesignScript.Geometry.Surface panel =
                    Autodesk.DesignScript.Geometry.Surface.ByPerimeterPoints(m_pts);

                m_tripoints.Add(m_pts);
                m_tripanels.Add(panel);
            }
            catch
            {
            }
        }

        return new Dictionary<string, object>
        {
            {"QuadPanels", m_quadpanels},
            {"QuadPoints", m_quadpoints},
            {"TriPanels", m_tripanels},
            {"TriPoints", m_tripoints},
        };
    }

    /// <summary>
    /// Create hexagonal panels
    /// </summary>
    /// <param name="srf">Surface</param>
    /// <param name="u">U Divisions</param>
    /// <param name="v">V Divisions</param>
    /// <param name="t">Parameter for adjusted hexagons (0-1)</param>
    /// <returns name = "HexPanels">Hexagon Panel surfaces</returns>
    /// <returns name = "HexPoints">Hexagon Panel points</returns>
    /// <returns name = "PentPanels">Pentagon Panel surfaces</returns>
    /// <returns name = "PentPoints">Pentagon Panel points</returns>
    /// <returns name = "QuadPanels">Quad Panel surfaces</returns>
    /// <returns name = "QuadPoints">Quad Panel points</returns>
    [MultiReturn(new[] {"HexPanels", "HexPoints", "PentPanels", "PentPoints", "QuadPanels", "QuadPoints"})]
    public static Dictionary<string, object> PanelHexagon(Autodesk.DesignScript.Geometry.Surface srf, int u, int v,
        double t)
    {
        List<List<Autodesk.DesignScript.Geometry.Point>> m_quadpoints =
            new List<List<Autodesk.DesignScript.Geometry.Point>>();
        List<Autodesk.DesignScript.Geometry.Surface> m_quadpanels = new List<Autodesk.DesignScript.Geometry.Surface>();

        List<List<Autodesk.DesignScript.Geometry.Point>> m_pentpoints =
            new List<List<Autodesk.DesignScript.Geometry.Point>>();
        List<Autodesk.DesignScript.Geometry.Surface> m_pentpanels = new List<Autodesk.DesignScript.Geometry.Surface>();

        List<List<Autodesk.DesignScript.Geometry.Point>> m_hexpoints =
            new List<List<Autodesk.DesignScript.Geometry.Point>>();
        List<Autodesk.DesignScript.Geometry.Surface> m_hexpanels = new List<Autodesk.DesignScript.Geometry.Surface>();

        TesselationUtils tess = new TesselationUtils();
        clsPanelCollection? mPanelcollection = tess.PanelHexagonal(u, v, t);
        foreach (clsQuad q in mPanelcollection!.Quads)
        {
            try
            {
                List<Autodesk.DesignScript.Geometry.Point> m_pts = new List<Autodesk.DesignScript.Geometry.Point>();
                m_pts.Add(srf.PointAtParameter(q.A.X, q.A.Y));
                m_pts.Add(srf.PointAtParameter(q.B.X, q.B.Y));
                m_pts.Add(srf.PointAtParameter(q.C.X, q.C.Y));
                m_pts.Add(srf.PointAtParameter(q.D.X, q.D.Y));

                Autodesk.DesignScript.Geometry.Surface panel =
                    Autodesk.DesignScript.Geometry.Surface.ByPerimeterPoints(m_pts);

                m_quadpoints.Add(m_pts);
                m_quadpanels.Add(panel);
            }
            catch
            {
            }
        }

        foreach (clsPentagon p in mPanelcollection.Pentagons!)
        {
            try
            {
                List<Autodesk.DesignScript.Geometry.Point> m_pts = new List<Autodesk.DesignScript.Geometry.Point>();
                m_pts.Add(srf.PointAtParameter(p.A.X, p.A.Y));
                m_pts.Add(srf.PointAtParameter(p.B.X, p.B.Y));
                m_pts.Add(srf.PointAtParameter(p.C.X, p.C.Y));
                m_pts.Add(srf.PointAtParameter(p.D.X, p.D.Y));
                m_pts.Add(srf.PointAtParameter(p.E.X, p.E.Y));

                Autodesk.DesignScript.Geometry.Surface panel =
                    Autodesk.DesignScript.Geometry.Surface.ByPerimeterPoints(m_pts);

                m_pentpoints.Add(m_pts);
                m_pentpanels.Add(panel);
            }
            catch
            {
            }
        }

        foreach (clsHexagon p in mPanelcollection.Hexagons!)
        {
            try
            {
                List<Autodesk.DesignScript.Geometry.Point> mPts = new List<Autodesk.DesignScript.Geometry.Point>();
                mPts.Add(srf.PointAtParameter(p.A.X, p.A.Y));
                mPts.Add(srf.PointAtParameter(p.B.X, p.B.Y));
                mPts.Add(srf.PointAtParameter(p.C.X, p.C.Y));
                mPts.Add(srf.PointAtParameter(p.D.X, p.D.Y));
                mPts.Add(srf.PointAtParameter(p.E.X, p.E.Y));
                mPts.Add(srf.PointAtParameter(p.F.X, p.F.Y));

                Autodesk.DesignScript.Geometry.Surface panel =
                    Autodesk.DesignScript.Geometry.Surface.ByPerimeterPoints(mPts);

                m_hexpoints.Add(mPts);
                m_hexpanels.Add(panel);
            }
            catch
            {
            }
        }

        return new Dictionary<string, object>
        {
            {"QuadPanels", m_quadpanels},
            {"QuadPoints", m_quadpoints},
            {"PentPanels", m_pentpanels},
            {"PentPoints", m_pentpoints},
            {"HexPanels", m_hexpanels},
            {"HexPoints", m_hexpoints},
        };
    }

    /// <summary>
    /// Create Triangle A panels
    /// </summary>
    /// <param name="srf">Surface</param>
    /// <param name="u">U Divisions</param>
    /// <param name="v">V Divisions</param>
    /// <returns name = "Panels">Panel surfaces</returns>
    /// <returns name = "Points">Panel points</returns>
    [MultiReturn(new[] {"Panels", "Points"})]
    public static Dictionary<string, object> PanelTriangleA(Autodesk.DesignScript.Geometry.Surface srf, int u, int v)
    {
        List<List<Autodesk.DesignScript.Geometry.Point>> mPoints =
            new List<List<Autodesk.DesignScript.Geometry.Point>>();
        List<Autodesk.DesignScript.Geometry.Surface> mPanels = new List<Autodesk.DesignScript.Geometry.Surface>();

        TesselationUtils tess = new TesselationUtils();
        List<clsTriangle>? mTris = tess.PanelTriangleA(u, v);
        foreach (clsTriangle q in mTris!)
        {
            List<Autodesk.DesignScript.Geometry.Point> mPts = new List<Autodesk.DesignScript.Geometry.Point>();
            mPts.Add(srf.PointAtParameter(q.A.X, q.A.Y));
            mPts.Add(srf.PointAtParameter(q.B.X, q.B.Y));
            mPts.Add(srf.PointAtParameter(q.C.X, q.C.Y));

            Autodesk.DesignScript.Geometry.Surface panel =
                Autodesk.DesignScript.Geometry.Surface.ByPerimeterPoints(mPts);

            mPoints.Add(mPts);
            mPanels.Add(panel);
        }

        return new Dictionary<string, object>
        {
            {"Panels", mPanels},
            {"Points", mPoints},
        };
    }

    /// <summary>
    /// Create Triangle B panels
    /// </summary>
    /// <param name="srf">Surface</param>
    /// <param name="u">U Divisions</param>
    /// <param name="v">V Divisions</param>
    /// <returns name = "Panels">Panel surfaces</returns>
    /// <returns name = "Points">Panel points</returns>
    [MultiReturn(new[] {"Panels", "Points"})]
    public static Dictionary<string, object> PanelTriangleB(Autodesk.DesignScript.Geometry.Surface srf, int u, int v)
    {
        List<List<Autodesk.DesignScript.Geometry.Point>> mPoints =
            new List<List<Autodesk.DesignScript.Geometry.Point>>();
        List<Autodesk.DesignScript.Geometry.Surface> mPanels = new List<Autodesk.DesignScript.Geometry.Surface>();

        TesselationUtils tess = new TesselationUtils();
        List<clsTriangle>? mTris = tess.PanelTriangleB(u, v);
        foreach (clsTriangle q in mTris!)
        {
            List<Autodesk.DesignScript.Geometry.Point> mPts = new List<Autodesk.DesignScript.Geometry.Point>();
            mPts.Add(srf.PointAtParameter(q.A.X, q.A.Y));
            mPts.Add(srf.PointAtParameter(q.B.X, q.B.Y));
            mPts.Add(srf.PointAtParameter(q.C.X, q.C.Y));

            Autodesk.DesignScript.Geometry.Surface panel =
                Autodesk.DesignScript.Geometry.Surface.ByPerimeterPoints(mPts);

            mPoints.Add(mPts);
            mPanels.Add(panel);
        }

        return new Dictionary<string, object>
        {
            {"Panels", mPanels},
            {"Points", mPoints},
        };
    }

    /// <summary>
    /// Create Triangle C panels
    /// </summary>
    /// <param name="srf">Surface</param>
    /// <param name="u">U Divisions</param>
    /// <param name="v">V Divisions</param>
    /// <returns name = "Panels">Panel surfaces</returns>
    /// <returns name = "Points">Panel points</returns>
    [MultiReturn(new[] {"Panels", "Points"})]
    public static Dictionary<string, object> PanelTriangleC(Autodesk.DesignScript.Geometry.Surface srf, int u, int v)
    {
        List<List<Autodesk.DesignScript.Geometry.Point>> mPoints =
            new List<List<Autodesk.DesignScript.Geometry.Point>>();
        List<Autodesk.DesignScript.Geometry.Surface> mPanels = new List<Autodesk.DesignScript.Geometry.Surface>();

        TesselationUtils tess = new TesselationUtils();
        List<clsTriangle>? mTris = tess.PanelTriangleC(u, v);
        foreach (clsTriangle q in mTris!)
        {
            List<Autodesk.DesignScript.Geometry.Point> mPts = new List<Autodesk.DesignScript.Geometry.Point>();
            mPts.Add(srf.PointAtParameter(q.A.X, q.A.Y));
            mPts.Add(srf.PointAtParameter(q.B.X, q.B.Y));
            mPts.Add(srf.PointAtParameter(q.C.X, q.C.Y));

            Autodesk.DesignScript.Geometry.Surface panel =
                Autodesk.DesignScript.Geometry.Surface.ByPerimeterPoints(mPts);

            mPoints.Add(mPts);
            mPanels.Add(panel);
        }

        return new Dictionary<string, object>
        {
            {"Panels", mPanels},
            {"Points", mPoints},
        };
    }
}