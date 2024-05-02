using Autodesk.DesignScript.Runtime;
using OpenMEPSandbox.Helpers.CLASSES;

namespace OpenMEPSandbox.Helpers
{
    /// <summary>
    /// Tesselation Utilities
    /// Use Open Source From lunchbox package : https://bitbucket.org/provingground-io/lunchbox-for-dynamo/src/master/
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class TesselationUtils
    {
        /// <summary>
        /// New Tessleation Utils
        /// </summary>
        public TesselationUtils()
        {
            //widen scope
        }

        #region Panels
        /// <summary>
        /// Even Quad Grid Division
        /// </summary>
        /// <param name="u">Number of divisions in the U direction</param>
        /// <param name="v">Number of divisions in the V direction</param>
        /// <returns>List of Quads</returns>
        public List<clsQuad>? PanelQuad(int u, int v)
        {
            try
            {
                List<clsQuad>? m_quads = new List<clsQuad>();

                double ustep = 1.0 / u;
                double vstep = 1.0 / v;

                for (int i = 0; i <= u - 1; i++)
                {
                    for (int j = 0; j <= v - 1; j++)
                    {
                        clsPoint ptA = new clsPoint(i * ustep, j * vstep, 0);
                        clsPoint ptB = new clsPoint((i + 1) * ustep, j * vstep, 0);
                        clsPoint ptC = new clsPoint((i + 1) * ustep, (j + 1) * vstep, 0);
                        clsPoint ptD = new clsPoint(i * ustep, (j + 1) * vstep, 0);

                        clsQuad quad = new clsQuad(ptA, ptB, ptC, ptD);
                        m_quads.Add(quad);
                    }
                }
                return m_quads;
            }
            catch { return null; }
        }

        /// <summary>
        /// Even Staggerd Quad Grid Division
        /// </summary>
        /// <param name="u">Number of divisions in the U direction</param>
        /// <param name="v">Number of divisions in the V direction</param>
        /// <returns>List of Quads</returns>
        public List<clsQuad>? PanelQuadStaggered(int u, int v)
        {
            try
            {
                List<clsQuad>? m_quads = new List<clsQuad>();

                double ustep = 1.0 / u;
                double vstep = 1.0 / v;

                clsPoint ptA;
                clsPoint ptB;
                clsPoint ptC;
                clsPoint ptD;

                for (int i = 0; i <= u - 1; i++)
                {
                    for (int j = 0; j <= v - 1; j++)
                    {
                        if ((j % 2) == 0)
                        {
                            ptA = new clsPoint(i * ustep, j * vstep,0);
                            ptB = new clsPoint((i + 1) * ustep, j * vstep,0);
                            ptC = new clsPoint((i + 1) * ustep, (j + 1) * vstep,0);
                            ptD = new clsPoint(i * ustep, (j + 1) * vstep,0);

                            clsQuad mysurface = new clsQuad(ptA, ptB, ptC, ptD);
                            m_quads.Add(mysurface);
                        }

                        if ((j % 2) == 1)
                        {
                            if ((i < u - 1))
                            {
                                ptA = new clsPoint((i + 0.5) * ustep, j * vstep,0);
                                ptB = new clsPoint((i + 1.5) * ustep, j * vstep,0);
                                ptC = new clsPoint((i + 1.5) * ustep, (j + 1) * vstep,0);
                                ptD = new clsPoint((i + 0.5) * ustep, (j + 1) * vstep,0);

                                clsQuad mysurface = new clsQuad(ptA, ptB, ptC, ptD);
                                m_quads.Add(mysurface);
                            }

                            if (i == 0)
                            {
                                ptA = new clsPoint(i * ustep, j * vstep,0);
                                ptB = new clsPoint((i + 0.5) * ustep, j * vstep,0);
                                ptC = new clsPoint((i + 0.5) * ustep, (j + 1) * vstep,0);
                                ptD = new clsPoint(i * ustep, (j + 1) * vstep,0);

                                clsQuad mysurface = new clsQuad(ptA, ptB, ptC, ptD);
                                m_quads.Add(mysurface);
                            }

                            if (i == v - 1)
                            {
                                ptA = new clsPoint((i + 0.5) * ustep, j * vstep,0);
                                ptB = new clsPoint((i + 1) * ustep, j * vstep,0);
                                ptC = new clsPoint((i + 1) * ustep, (j + 1) * vstep,0);
                                ptD = new clsPoint((i + 0.5) * ustep, (j + 1) * vstep,0);

                                clsQuad mysurface = new clsQuad(ptA, ptB, ptC, ptD);
                                m_quads.Add(mysurface);
                            }
                        }
                    }
                }
                return m_quads;
            }
            catch { return null; }
        }

        /// <summary>
        /// Scewed Quad Division
        /// </summary>
        /// <param name="u">Number of divisions in the U direction</param>
        /// <param name="v">Number of divisions in the V direction</param>
        /// <param name="t">Scew parameter (0-1)</param>
        /// <returns>List of Quads</returns>
        public clsPanelCollection? PanelQuadScewed(int u, int v, double t)
        {
            try
            {
                List<clsQuad> m_quads = new List<clsQuad>();
                List<clsTriangle>? m_tris = new List<clsTriangle>();

                double ustep = 1.0 / u;
                double vstep = 1.0 / v;

                for (int i = 0; i <= u - 1; i++)
                {

                    for (int j = 0; j <= v - 1; j++)
                    {
                        if (j > 0)
                        {
                            clsPoint ptA = new clsPoint(i * ustep, (j - (1 - t)) * vstep, 0);
                            clsPoint ptB = new clsPoint((i + 1) * ustep, j * vstep, 0);
                            clsPoint ptC = new clsPoint((i + 1) * ustep, (j + 1) * vstep, 0);
                            clsPoint ptD = new clsPoint(i * ustep, (j + t) * vstep, 0);

                            clsQuad mysurface = new clsQuad(ptA, ptB, ptC, ptD);
                            m_quads.Add(mysurface);
                        }
                        else if (j == 0)
                        {
                            clsPoint ptA = new clsPoint(i * ustep, j * vstep,0);
                            clsPoint ptB = new clsPoint((i + 1) * ustep, j * vstep,0);
                            clsPoint ptC = new clsPoint((i + 1) * ustep, (j + 1) * vstep,0);
                            clsPoint ptD = new clsPoint(i * ustep, (j + t) * vstep,0);

                            clsQuad mysurface = new clsQuad(ptA, ptB, ptC, ptD);
                            m_quads.Add(mysurface);
                        }

                        if (j == v - 1 & t < 1)
                        {
                            clsPoint ptA = new clsPoint(i * ustep, (j + t) * vstep,0);
                            clsPoint ptC = new clsPoint((i + 1) * ustep, (j + 1) * vstep,0);
                            clsPoint ptD = new clsPoint(i * ustep, (j + 1) * vstep,0);

                            clsTriangle mysurfaceB = new clsTriangle(ptC, ptD, ptA);
                            m_tris.Add(mysurfaceB);
                        }
                    }
                }
                return new clsPanelCollection(m_tris, m_quads, null, null);
            }
            catch { return null; }
        }

        /// <summary>
        /// Random Quad Division
        /// </summary>
        /// <param name="u">Number of divisions in the U direction</param>
        /// <param name="v">Number of divisions in the V direction</param>
        /// <param name="s">Random seed</param>
        /// <returns>List of Quads</returns>
        public List<clsQuadVar>? RandomQuad(int u, int v, int s)
        {
            try
            {
                List<clsQuadVar>? m_quads = new List<clsQuadVar>();

                double ustep = 1.0 / u;
                double vstep = 1.0 / v;

                Random rand = new Random(s);

                for (int i = 0; i <= u-1; i++)
                {
                    int j = 0;
                    while (j <= v-1)
                    {
                        int varLength = rand.Next(1, 10);
                        if ((j+varLength) > v)
                        {
                            varLength = v - j;
                        }
                        List<clsPoint> panelPts = new List<clsPoint>();
                        for (int k = 0; k <= varLength; k++)
                        {
                            clsPoint ptA = new clsPoint(i * ustep, (j + k) * vstep,0);
                            panelPts.Add(ptA);
                        }
                        for (int k = 0; k <= varLength; k++)
                        {
                            clsPoint ptB = new clsPoint((i + 1) * ustep, (j + k) * vstep, 0);
                            panelPts.Add(ptB);
                        }
                        clsQuadVar srfPanel = new clsQuadVar(panelPts, varLength+1);
                        m_quads.Add(srfPanel);

                        j = j + varLength;
                    }
                }
                return m_quads;
            }
            catch (Exception){ return null; }
        }

        /// <summary>
        /// Even Diamond Grid Division
        /// </summary>
        /// <param name="u">Number of divisions in the U direction</param>
        /// <param name="v">Number of divisions in the V direction</param>
        /// <returns>List of Diamonds</returns>
        public clsPanelCollection? PanelDiamond(int u, int v)
        {
            try
            {
                List<clsQuad> m_quads = new List<clsQuad>();
                List<clsTriangle>? m_tris = new List<clsTriangle>();

                double ustep = 1.0 / u;
                double vstep = 1.0 / v;

                clsPoint ptA;
                clsPoint ptB;
                clsPoint ptC;
                clsPoint ptD;

                for (int i = 0; i <= u; i++)
                {
                    for (int j = 0; j <= v; j++)
                    {
                        if (((i + j) % 2) == 0)
                        {
                            clsQuad m_quad;
                            clsTriangle m_tri;

                            if ((i > 0))
                            {
                                ptA = new clsPoint((i - 1) * ustep, j * vstep,0);
                            }
                            else
                            {
                                ptA = new clsPoint(i * ustep, j * vstep, 0);
                            }

                            if ((j > 0))
                            {
                                ptB = new clsPoint(i * ustep, (j - 1) * vstep, 0);
                            }
                            else
                            {
                                ptB = new clsPoint(i * ustep, j * vstep, 0);
                            }

                            if ((i < u))
                            {
                                ptC = new clsPoint((i + 1) * ustep, j * vstep, 0);
                            }
                            else
                            {
                                ptC = new clsPoint(i * ustep, j * vstep, 0);
                            }

                            if ((j <= v - 1))
                            {
                                ptD = new clsPoint(i * ustep, (j + 1) * vstep, 0);
                            }
                            else
                            {
                                ptD = new clsPoint(i * ustep, j * vstep, 0);
                            }


                            if ((i > 0) & (j > 0) & (i < u) & (j <= v - 1))
                            {
                                m_quad = new clsQuad(ptA, ptB, ptC, ptD);
                                m_quads.Add(m_quad);
                            }
                            else
                            {
                                if (i == 0 & j == 0)
                                {
                                    m_tri = new clsTriangle(ptB, ptC, ptD);
                                    m_tris.Add(m_tri);
                                }
                                if (i == 0 & j == v)
                                {
                                    m_tri = new clsTriangle(ptB, ptC, ptD);
                                    m_tris.Add(m_tri);
                                }
                                if (i == u & j == 0)
                                {
                                    m_tri = new clsTriangle(ptC, ptD, ptA);
                                    m_tris.Add(m_tri);
                                }
                                if (i == u & j == v)
                                {
                                    m_tri = new clsTriangle(ptA, ptB, ptC);
                                    m_tris.Add(m_tri);
                                }

                                if (i == 0 & j > 0 & j < v)
                                {
                                    m_tri = new clsTriangle(ptB, ptC, ptD);
                                    m_tris.Add(m_tri);
                                }

                                if (i == u & j > 0 & j < v)
                                {
                                    m_tri = new clsTriangle(ptA, ptB, ptD);
                                    m_tris.Add(m_tri);
                                }

                                if (j == 0 & i > 0 & i < u)
                                {
                                    m_tri = new clsTriangle(ptA, ptC, ptD);
                                    m_tris.Add(m_tri);
                                }

                                if (j == v & i > 0 & i < u)
                                {
                                    m_tri = new clsTriangle(ptA, ptB, ptC);
                                    m_tris.Add(m_tri);
                                }
                            }
                        }
                    }
                }
                return new clsPanelCollection(m_tris, m_quads, null, null);
            }
            catch { return null; }
        }

        /// <summary>
        /// Even Hexagonal Division
        /// </summary>
        /// <param name="u">Number of divisions in the U direction</param>
        /// <param name="v">Number of divisions in the V direction</param>
        /// <param name="t">Hex parameter (0-1)</param>
        /// <returns>List of Hexagons</returns>
        public clsPanelCollection? PanelHexagonal(int u, int v, double t)
        {
            try
            {
                List<clsQuad> m_quads = new List<clsQuad>();
                List<clsPentagon>? m_pents = new List<clsPentagon>();
                List<clsHexagon>? m_hexs = new List<clsHexagon>();

                double ustep = 1.0 / u;
                double vstep = 1.0 / v;
                double r = t;

                clsPoint ptA;
                clsPoint ptB;
                clsPoint ptC;
                clsPoint ptD;
                clsPoint ptE;
                clsPoint ptF;
                clsPoint ptCent;

                List<clsPoint> ptsA = new List<clsPoint>();
                List<clsPoint> ptsB = new List<clsPoint>();
                List<clsPoint> ptsC = new List<clsPoint>();
                List<clsPoint> ptsD = new List<clsPoint>();
                List<clsPoint> ptsE = new List<clsPoint>();
                List<clsPoint> ptsF = new List<clsPoint>();

                List<clsPoint> ptsCent = new List<clsPoint>();


                for (int i = 0; i <= u; i++)
                {

                    for (int j = 0; j <= v; j++)
                    {

                        if (((i + j) % 2) == 0)
                        {

                            if ((i > 0))
                            {
                                if ((j + r) > v)
                                {
                                    ptA = new clsPoint((i - 1) * ustep, j * vstep,0);
                                    ptsA.Add(ptA);
                                }
                                else
                                {
                                    ptA = new clsPoint((i - 1) * ustep, (j + r) * vstep, 0);
                                    ptsA.Add(ptA);
                                }

                                if ((j - r) < 0)
                                {
                                    ptB = new clsPoint((i - 1) * ustep, j * vstep, 0);
                                    ptsB.Add(ptB);
                                }
                                else
                                {
                                    ptB = new clsPoint((i - 1) * ustep, (j - r) * vstep, 0);
                                    ptsB.Add(ptB);
                                }

                                ptCent = new clsPoint(i * ustep, j * vstep, 0);
                                ptsCent.Add(ptCent);


                            }
                            else
                            {
                                ptA = new clsPoint(i * ustep, j * vstep, 0);
                                ptsA.Add(ptA);

                                ptB = new clsPoint(i * ustep, j * vstep, 0);
                                ptsB.Add(ptB);

                                ptCent = new clsPoint(i * ustep, j * vstep, 0);
                                ptsCent.Add(ptCent);

                            }

                            if ((j > 0))
                            {
                                if ((j - (1 - r)) < 0)
                                {
                                    ptC = new clsPoint(i * ustep, (j - 1) * vstep, 0);
                                    ptsC.Add(ptC);
                                }
                                else
                                {
                                    ptC = new clsPoint(i * ustep, (j - (1 - r)) * vstep, 0);
                                    ptsC.Add(ptC);
                                }

                            }
                            else
                            {
                                ptC = new clsPoint(i * ustep, j * vstep, 0);
                                ptsC.Add(ptC);
                            }


                            if ((i < u))
                            {
                                if ((j + r) > v)
                                {
                                    ptE = new clsPoint((i + 1) * ustep, j * vstep, 0);
                                    ptsE.Add(ptE);
                                }
                                else
                                {
                                    ptE = new clsPoint((i + 1) * ustep, (j + r) * vstep, 0);
                                    ptsE.Add(ptE);
                                }

                                if ((j - r) < 0)
                                {
                                    ptD = new clsPoint((i + 1) * ustep, j * vstep, 0);
                                    ptsD.Add(ptD);
                                }
                                else
                                {
                                    ptD = new clsPoint((i + 1) * ustep, (j - r) * vstep, 0);
                                    ptsD.Add(ptD);
                                }
                            }
                            else
                            {
                                ptE = new clsPoint(i * ustep, j * vstep, 0);
                                ptsE.Add(ptE);
                                ptD = new clsPoint(i * ustep, j * vstep, 0);
                                ptsD.Add(ptD);
                            }

                            if ((j <= v - 1))
                            {
                                ptF = new clsPoint(i * ustep, (j + (1 - r)) * vstep, 0);
                                ptsF.Add(ptF);
                            }
                            else
                            {
                                ptF = new clsPoint(i * ustep, j * vstep, 0);
                                ptsF.Add(ptF);
                            }

                            clsQuad m_quad;
                            clsHexagon m_hex;
                            clsPentagon m_pent;
                            if ((i > 0) & (j > 0) & (i < u) & (j <= v - 1))
                            {
                                m_hex = new clsHexagon(ptA, ptB, ptC, ptD, ptE, ptF);
                                m_hexs.Add(m_hex);
                            }
                            else
                            {
                                if (i == 0 & j == 0)
                                {
                                    List<clsPoint> plPts = new List<clsPoint>();
                                    plPts.Add(ptA);
                                    plPts.Add(ptD);
                                    plPts.Add(ptE);
                                    plPts.Add(ptF);
                                    plPts.Add(ptA);
                                    m_quad = new clsQuad(ptA, ptD, ptE, ptF);
                                    m_quads.Add(m_quad);
                                }
                                if (i == 0 & j == v)
                                {
                                    if (v % 2 == 0)
                                    {
                                        List<clsPoint> plPts = new List<clsPoint>();
                                        plPts.Add(ptA);
                                        plPts.Add(ptC);
                                        plPts.Add(ptD);
                                        plPts.Add(ptE);
                                        plPts.Add(ptA);
                                        m_quad = new clsQuad(ptA, ptC, ptD, ptE);
                                        m_quads.Add(m_quad);
                                    }
                                }

                                if (i == u & j == 0)
                                {
                                    if (u % 2 == 0)
                                    {
                                        List<clsPoint> plPts = new List<clsPoint>();
                                        plPts.Add(ptA);
                                        plPts.Add(ptB);
                                        plPts.Add(ptD);
                                        plPts.Add(ptF);
                                        plPts.Add(ptA);
                                        m_quad = new clsQuad(ptA, ptB, ptD, ptF);
                                        m_quads.Add(m_quad);
                                    }
                                }
                                if (i == u & j == v)
                                {
                                    if (u % 2 == 0 & v % 2 == 0)
                                    {
                                        List<clsPoint> plPts = new List<clsPoint>();
                                        plPts.Add(ptA);
                                        plPts.Add(ptB);
                                        plPts.Add(ptC);
                                        plPts.Add(ptF);
                                        plPts.Add(ptA);
                                        m_quad = new clsQuad(ptA, ptB, ptC, ptF);
                                        m_quads.Add(m_quad);
                                    }
                                    else
                                    {
                                        List<clsPoint> plPts = new List<clsPoint>();
                                        plPts.Add(ptA);
                                        plPts.Add(ptB);
                                        plPts.Add(ptC);
                                        plPts.Add(ptF);
                                        plPts.Add(ptA);
                                        m_quad = new clsQuad(ptA, ptB, ptC, ptF);
                                        m_quads.Add(m_quad);
                                    }

                                }

                                if (i == 0 & j > 0 & j < v)
                                {
                                    List<clsPoint> plPts = new List<clsPoint>();
                                    plPts.Add(ptA);
                                    plPts.Add(ptC);
                                    plPts.Add(ptD);
                                    plPts.Add(ptE);
                                    plPts.Add(ptF);
                                    plPts.Add(ptA);
                                    m_pent = new clsPentagon(ptA, ptC, ptD, ptE, ptF);
                                    m_pents.Add(m_pent);
                                }

                                if (i == u & j > 0 & j < v)
                                {
                                    List<clsPoint> plPts = new List<clsPoint>();
                                    plPts.Add(ptA);
                                    plPts.Add(ptB);
                                    plPts.Add(ptC);
                                    plPts.Add(ptD);
                                    plPts.Add(ptF);
                                    plPts.Add(ptA);
                                    m_pent = new clsPentagon(ptA, ptB, ptC, ptD, ptF);
                                    m_pents.Add(m_pent);
                                }

                                if (j == 0 & i > 0 & i < u)
                                {
                                    List<clsPoint> plPts = new List<clsPoint>();
                                    plPts.Add(ptA);
                                    plPts.Add(ptB);
                                    plPts.Add(ptD);
                                    plPts.Add(ptE);
                                    plPts.Add(ptF);
                                    plPts.Add(ptA);
                                    m_pent = new clsPentagon(ptA, ptB, ptD, ptE, ptF);
                                    m_pents.Add(m_pent);
                                }

                                if (j == v & i > 0 & i < u)
                                {
                                    List<clsPoint> plPts = new List<clsPoint>();
                                    plPts.Add(ptA);
                                    plPts.Add(ptB);
                                    plPts.Add(ptC);
                                    plPts.Add(ptD);
                                    plPts.Add(ptE);
                                    plPts.Add(ptA);
                                    m_pent = new clsPentagon(ptA, ptB, ptC, ptD, ptE);
                                    m_pents.Add(m_pent);
                                }
                            }
                        }
                    }
                }
                return new clsPanelCollection(null, m_quads, m_pents, m_hexs);
            }
            catch { return null; }
        }

        /// <summary>
        /// Even Triangle Grid Division (Method A)
        /// </summary>
        /// <param name="u">Number of divisions in the U direction</param>
        /// <param name="v">Number of divisions in the V direction</param>
        /// <returns>List of Triangles</returns>
        public List<clsTriangle>? PanelTriangleA(int u, int v)
        {
            try
            {
                List<clsTriangle>? m_triangles = new List<clsTriangle>();
                double ustep = 1.0 / u;
                double vstep = 1.0 / v;

                for (int i = 0; i <= u - 1; i++)
                {
                    for (int j = 0; j <= v - 1; j++)
                    {
                        clsPoint ptA = new clsPoint(i * ustep, j * vstep, 0);
                        clsPoint ptB = new clsPoint((i + 1) * ustep, j * vstep, 0);
                        clsPoint ptC = new clsPoint((i + 1) * ustep, (j + 1) * vstep, 0);
                        clsPoint ptD = new clsPoint(i * ustep, (j + 1) * vstep, 0);

                        clsTriangle triA = new clsTriangle(ptA, ptB, ptC);
                        clsTriangle triB = new clsTriangle(ptC, ptD, ptA);
                        m_triangles.Add(triA);
                        m_triangles.Add(triB);
                    }
                }
                return m_triangles;
            }
            catch { return null; }
        }

        /// <summary>
        /// Even Triangle Grid Division (Method B)
        /// </summary>
        /// <param name="u">Number of divisions in the U direction</param>
        /// <param name="v">Number of divisions in the V direction</param>
        /// <returns>List of Triangles</returns>
        public List<clsTriangle>? PanelTriangleB(int u, int v)
        {
            try
            {
                List<clsTriangle>? m_triangles = new List<clsTriangle>();

                double ustep = 1.0 / u;
                double vstep = 1.0 / v;

                for (int i = 0; i <= u - 1; i++)
                {
                    for (int j = 0; j <= v - 1; j++)
                    {
                        clsPoint ptA = new clsPoint(i * ustep, j * vstep, 0);
                        clsPoint ptB = new clsPoint((i + 1) * ustep, j * vstep, 0);
                        clsPoint ptC = new clsPoint((i + 1) * ustep, (j + 1) * vstep, 0);
                        clsPoint ptD = new clsPoint(i * ustep, (j + 1) * vstep, 0);
                        clsPoint ptE = new clsPoint((i + 0.5) * ustep, (j + 0.5) * vstep, 0);

                        clsTriangle triA = new clsTriangle(ptA, ptB, ptE);
                        clsTriangle triB = new clsTriangle(ptB, ptC, ptE);
                        clsTriangle triC = new clsTriangle(ptC, ptD, ptE);
                        clsTriangle triD = new clsTriangle(ptD, ptA, ptE);

                        m_triangles.Add(triA);
                        m_triangles.Add(triB);
                        m_triangles.Add(triC);
                        m_triangles.Add(triD);
                    }
                }
                return m_triangles;
            }
            catch { return null; }
        }

        /// <summary>
        /// Even Triangle Grid Division (Method C)
        /// </summary>
        /// <param name="u">Number of divisions in the U direction</param>
        /// <param name="v">Number of divisions in the V direction</param>
        /// <returns>List of Triangles</returns>
        public List<clsTriangle>? PanelTriangleC(int u, int v)
        {
            try
            {
                List<clsTriangle>? m_triangles = new List<clsTriangle>();

                double ustep = 1.0 / u;
                double vstep = 1.0 / v;

                clsPoint ptA;
                clsPoint ptB;
                clsPoint ptC;

                for (int i = 0; i <= u - 1; i++)
                {

                    for (int j = 0; j <= v; j++)
                    {
                        if (((i + j) % 2) == 0)
                        {
                            ptA = new clsPoint(i * ustep, j * vstep, 0);
                            if ((j > 0))
                            {
                                ptB = new clsPoint((i + 1) * ustep, (j - 1) * vstep, 0);
                            }
                            else
                            {
                                ptB = new clsPoint((i + 1) * ustep, j * vstep, 0);
                            }

                            if ((j < v))
                            {
                                ptC = new clsPoint((i + 1) * ustep, (j + 1) * vstep, 0);
                            }
                            else
                            {
                                ptC = new clsPoint((i + 1) * ustep, j * vstep, 0);
                            }
                            clsTriangle mysurfaceA = new clsTriangle(ptA, ptB, ptC);
                            m_triangles.Add(mysurfaceA);

                        }
                        else
                        {
                            ptA = new clsPoint((i + 1) * ustep, j * vstep, 0);
                            if ((j < v))
                            {
                                ptB = new clsPoint(i * ustep, (j + 1) * vstep, 0);
                            }
                            else
                            {
                                ptB = new clsPoint(i * ustep, j * vstep, 0);
                            }
                            if ((j > 0))
                            {
                                ptC = new clsPoint(i * ustep, (j - 1) * vstep, 0);
                            }
                            else
                            {
                                ptC = new clsPoint(i * ustep, j * vstep, 0);
                            }
                            clsTriangle mysurfaceB = new clsTriangle(ptA, ptB, ptC);
                            m_triangles.Add(mysurfaceB);
                        }
                    }
                }
                return m_triangles;
            }
            catch { return null; }
        }
        #endregion

        #region Wireframes
        /// <summary>
        /// Even Basic Grid Wireframe
        /// </summary>
        /// <param name="u">Number of divisions in the U direction</param>
        /// <param name="v">Number of divisions in the V direction</param>
        /// <returns>List of Wires</returns>
        public List<clsLine>? WireGrid(int u, int v)
        {
            try
            {
                List<clsLine>? lines = new List<clsLine>();
                double ustep = 1.0 / u;
                double vstep = 1.0 / v;

                for (int i = 0; i <= u; i++)
                {

                    for (int j = 0; j <= v; j++)
                    {
                        clsPoint ptA = new clsPoint(i * ustep, j * vstep,0);

                        if (i < u)
                        {
                            clsPoint ptBu = new clsPoint((i + 1) * ustep, j * vstep,0);
                            clsLine myline = new clsLine(ptA, ptBu);
                            lines.Add(myline);
                        }

                        if (j < v)
                        {
                            clsPoint ptBv = new clsPoint(i * ustep, (j + 1) * vstep,0);
                            clsLine myline = new clsLine(ptA, ptBv);
                            lines.Add(myline);
                        }
                    }
                }
                return lines;
            }
            catch { return null; }
        }

        /// <summary>
        /// Even 1D Braced Grid Wireframe
        /// </summary>
        /// <param name="u">Number of divisions in the U direction</param>
        /// <param name="v">Number of divisions in the V direction</param>
        /// <returns>List of Wires</returns>
        public List<clsLine>? WireBraced1DGrid(int u, int v)
        {
            try
            {
                List<clsLine>? lines = new List<clsLine>();
                double ustep = 1.0 / u;
                double vstep = 1.0 / v;

                for (int i = 0; i <= u; i++)
                {

                    for (int j = 0; j <= v; j++)
                    {
                        clsPoint ptA = new clsPoint(i * ustep, j * vstep, 0);

                        if (i < u)
                        {
                            clsPoint ptBu = new clsPoint((i + 1) * ustep, j * vstep, 0);
                            clsLine myline = new clsLine(ptA, ptBu);
                            lines.Add(myline);
                        }

                        if (j < v)
                        {
                            clsPoint ptBv = new clsPoint(i * ustep, (j + 1) * vstep, 0);
                            clsLine myline = new clsLine(ptA, ptBv);
                            lines.Add(myline);
                        }

                        if (i < u & j < v)
                        {
                            clsPoint braceptA = new clsPoint(i * ustep, j * vstep, 0);
                            clsPoint braceptC = new clsPoint((i + 1) * ustep, (j + 1) * vstep, 0);
                            clsLine myline = new clsLine(braceptA, braceptC);
                            lines.Add(myline);
                        }
                    }
                }
                return lines;
            }
            catch { return null; }
        }

        /// <summary>
        /// Even 2D Braced Grid Wireframe
        /// </summary>
        /// <param name="u">Number of divisions in the U direction</param>
        /// <param name="v">Number of divisions in the V direction</param>
        /// <returns>List of Wires</returns>
        public List<clsLine>? WireBraced2DGrid(int u, int v)
        {
            try
            {
                List<clsLine>? lines = new List<clsLine>();
                double ustep = 1.0 / u;
                double vstep = 1.0 / v;

                for (int i = 0; i <= u; i++)
                {

                    for (int j = 0; j <= v; j++)
                    {
                        clsPoint ptA = new clsPoint(i * ustep, j * vstep, 0);

                        if (i < u)
                        {
                            clsPoint ptBu = new clsPoint((i + 1) * ustep, j * vstep, 0);
                            clsLine myline = new clsLine(ptA, ptBu);
                            lines.Add(myline);
                        }

                        if (j < v)
                        {
                            clsPoint ptBv = new clsPoint(i * ustep, (j + 1) * vstep, 0);
                            clsLine myline = new clsLine(ptA, ptBv);
                            lines.Add(myline);
                        }

                        if (i < u & j < v)
                        {
                            clsPoint braceptA = new clsPoint(i * ustep, j * vstep,0);
                            clsPoint braceptB = new clsPoint((i + 1) * ustep, j * vstep,0);
                            clsPoint braceptC = new clsPoint((i + 1) * ustep, (j + 1) * vstep,0);
                            clsPoint braceptD = new clsPoint(i * ustep, (j + 1) * vstep,0);
                            clsPoint mpt = new clsPoint((i * ustep) + (0.5 * ustep), (j * vstep) + (0.5 * vstep),0);

                            clsLine brace1 = new clsLine(braceptA, mpt);
                            clsLine brace2 = new clsLine(braceptB, mpt);
                            clsLine brace3 = new clsLine(braceptC, mpt);
                            clsLine brace4 = new clsLine(braceptD, mpt);

                            lines.Add(brace1);
                            lines.Add(brace2);
                            lines.Add(brace3);
                            lines.Add(brace4);
                        }
                    }
                }
                return lines;
            }
            catch { return null; }
        }

        /// <summary>
        /// Even Diagrid Wireframe
        /// </summary>
        /// <param name="u">Number of divisions in the U direction</param>
        /// <param name="v">Number of divisions in the V direction</param>
        /// <param name="t">Boolean toggle to change diagrid division</param>
        /// <returns>List of Wires</returns>
        public List<clsLine>? WireDiagrid(int u, int v, bool t)
        {
            try
            {
                List<clsLine>? lines = new List<clsLine>();
                double ustep = 1.0 / u;
                double vstep = 1.0 / v;

                int diaswitch = 0;
                if (t == true)
                {
                    diaswitch = 1;
                }
                else
                {
                    diaswitch = 0;
                }

                for (int i = 0; i <= u; i++)
                {
                    for (int j = 0; j <= v; j++)
                    {
                        if (((i + j) % 2) == diaswitch)
                        {
                            clsPoint pt = new clsPoint(i * ustep, j * vstep,0);
                        }

                        if (((i + j) % 2) == diaswitch & i < u)
                        {
                            clsPoint ptA = new clsPoint(i * ustep, j * vstep,0);
                            if (j > 0)
                            {
                                clsPoint ptBu = new clsPoint((i + 1) * ustep, (j - 1) * vstep,0);
                                clsLine myline = new clsLine(ptA, ptBu);
                                lines.Add(myline);
                            }

                            if (j < v)
                            {
                                clsPoint ptBv = new clsPoint((i + 1) * ustep, (j + 1) * vstep,0);
                                clsLine myline = new clsLine(ptA, ptBv);
                                lines.Add(myline);
                            }
                        }
                    }
                }
                return lines;
            }
            catch { return null; }
        }

        /// <summary>
        /// Even Hexagon Wireframe
        /// </summary>
        /// <param name="u">Number of divisions in the U direction</param>
        /// <param name="v">Number of divisions in the V direction</param>
        /// <param name="a">Hexagon shape adjust</param>
        /// <param name="t">Boolean toggle to change diagrid division</param>
        /// <returns>List of Wires</returns>
        public List<clsLine>? WireHexagon(int u, int v, double a, bool t)
        {
            try
            {
                List<clsLine>? lines = new List<clsLine>();
                double ustep = 1.0 / u;
                double vstep = 1.0 / v;

                double r = a;

                clsLine crvA;
                clsLine crvB;
                clsLine crvC;

                clsPoint ptA;
                clsPoint ptB;
                clsPoint ptC;
                clsPoint ptD;

                for (int i = 0; i <= u; i++)
                {

                    for (int j = 0; j <= v; j++)
                    {

                        if (t == false)
                        {
                            if (((i + j) % 2) == 0)
                            {
                                if (j == 0)
                                {
                                    ptA = new clsPoint(i * ustep, j * vstep,0);
                                    ptB = new clsPoint(i * ustep, (j + r) * vstep, 0);
                                    crvA = new clsLine(ptA, ptB);
                                    lines.Add(crvA);
                                }
                                else if (j == v)
                                {
                                    ptA = new clsPoint(i * ustep, (j - r) * vstep, 0);
                                    ptB = new clsPoint(i * ustep, j * vstep, 0);
                                    crvA = new clsLine(ptA, ptB);
                                    lines.Add(crvA);
                                }
                                else
                                {
                                    ptA = new clsPoint(i * ustep, (j - r) * vstep, 0);
                                    ptB = new clsPoint(i * ustep, (j + r) * vstep, 0);
                                    crvA = new clsLine(ptA, ptB);
                                    lines.Add(crvA);
                                }

                                if (i < u)
                                {
                                    if (j > 0)
                                    {
                                        ptC = new clsPoint((i + 1) * ustep, (j - 1 + r) * vstep, 0);
                                        crvB = new clsLine(ptA, ptC);
                                        lines.Add(crvB);

                                    }
                                    if (j < v)
                                    {
                                        ptD = new clsPoint((i + 1) * ustep, (j + 1 - r) * vstep, 0);
                                        crvC = new clsLine(ptB, ptD);
                                        lines.Add(crvC);
                                    }
                                }
                            }


                        }
                        else
                        {
                            ptA = new clsPoint(i * ustep, j * vstep, 0);
                            if (((i + j) % 3) == 0)
                            {
                                if (i < u)
                                {
                                    ptB = new clsPoint((i + 1) * ustep, j * vstep, 0);
                                    crvA = new clsLine(ptA, ptB);
                                    lines.Add(crvA);
                                }

                                if (j < v)
                                {
                                    ptC = new clsPoint(i * ustep, (j + 1) * vstep, 0);
                                    crvB = new clsLine(ptA, ptC);
                                    lines.Add(crvB);
                                }
                            }

                            if ((i + j) % 3 == 1)
                            {
                                if ((i < u & j < v))
                                {
                                    ptD = new clsPoint((i + 1) * ustep, (j + 1) * vstep, 0);
                                    crvC = new clsLine(ptA, ptD);
                                    lines.Add(crvC);
                                }
                            }
                        }
                    }
                }
                return lines;
            }
            catch { return null; }
        }
        #endregion
    }
}
