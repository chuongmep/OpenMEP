using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Geometry;

/// <summary>
    /// Surface Manipulation
    /// </summary>
    public class Surfaces
    {
        private Surfaces()
        {

        }
        /// <summary>
        /// Divides a surface using UV divisions
        /// </summary>
        /// <param name="surface">Surface</param>
        /// <param name="UDivision">U division</param>
        /// <param name="VDivision">V division</param>
        /// <returns name="Points">A list of points</returns>
        /// <returns name="Planes">A list of planes</returns>
        /// <returns name="Normals">A list of normal vectors.</returns>
        /// <returns name="Gaussian">A list of Gaussian curvature values</returns>
        /// <returns name="UParams">A list of U parameter values</returns>
        /// <returns name="VParams">A list of V parameter values</returns>
        /// <search>lunchbox,surface,point,plane,divide,normal,curvature</search>
        [MultiReturn(new[] {"Points", "Planes", "Normals", "Gaussian", "UParams", "VParams"})]
        public static Dictionary<string, object> DivideSurfaceUV(Surface surface, int UDivision, int VDivision)
        {
            double m_uStep = 1.0 / UDivision;
            double m_vStep = 1.0 / VDivision;

            List<List<Autodesk.DesignScript.Geometry.Point>> m_points = new List<List<Autodesk.DesignScript.Geometry.Point>>();
            List<List<Autodesk.DesignScript.Geometry.Plane>> m_planes = new List<List<Autodesk.DesignScript.Geometry.Plane>>();
            List<List<Autodesk.DesignScript.Geometry.Vector>> m_normals = new List<List<Autodesk.DesignScript.Geometry.Vector>>();
            List<List<double>> m_curvature = new List<List<double>>();
            List<List<double>> m_uparams = new List<List<double>>();
            List<List<double>> m_vparams = new List<List<double>>();

            for (int i = 0; i <= UDivision; i++)
            {
                List<Autodesk.DesignScript.Geometry.Point> m_ptrow = new List<Autodesk.DesignScript.Geometry.Point>();
                List<Autodesk.DesignScript.Geometry.Plane> m_plnrow = new List<Autodesk.DesignScript.Geometry.Plane>();
                List<Autodesk.DesignScript.Geometry.Vector> m_normrow = new List<Autodesk.DesignScript.Geometry.Vector>();
                List<double> m_curvrow = new List<double>();
                List<double> m_urow = new List<double>();
                List<double> m_vrow = new List<double>();
                for (int j = 0; j <= VDivision; j++)
                {
                    double u = m_uStep * i;
                    double v = m_vStep * j;

                    Autodesk.DesignScript.Geometry.Point m_pt = surface.PointAtParameter(u, v);
                    Autodesk.DesignScript.Geometry.Plane m_pl = surface.CoordinateSystemAtParameter(u, v).XYPlane;
                    Autodesk.DesignScript.Geometry.Vector m_norm = surface.NormalAtParameter(u, v);
                    double m_crv = surface.GaussianCurvatureAtParameter(u, v);

                    m_ptrow.Add(m_pt);
                    m_plnrow.Add(m_pl);
                    m_normrow.Add(m_norm);
                    m_curvrow.Add(m_crv);
                    m_urow.Add(u);
                    m_vrow.Add(v);
                }

                m_points.Add(m_ptrow);
                m_planes.Add(m_plnrow);
                m_normals.Add(m_normrow);
                m_curvature.Add(m_curvrow);
                m_uparams.Add(m_urow);
                m_vparams.Add(m_vrow);
            }

            return new Dictionary<string, object>
            {
                {"Points", m_points},
                {"Planes", m_planes},
                {"Normals", m_normals},
                {"Gaussian", m_curvature},
                {"UParams", m_uparams},
                {"VParams", m_vparams}
            };
        }

        /// <summary>
        /// Deconstructs a surface into edge curves and points
        /// </summary>
        /// <param name="surface">Surface</param>
        /// <returns name="Edges">Edge curves.</returns>
        /// <returns name="Points">Corner points.</returns>
        [MultiReturn(new[] {"Edges", "Points"})]
        public static Dictionary<string, object> DeconstructSurface(Surface surface)
        {
            Autodesk.DesignScript.Geometry.Curve[] m_perimeter = surface.PerimeterCurves();
            List<Autodesk.DesignScript.Geometry.Point> m_points = new List<Autodesk.DesignScript.Geometry.Point>();
            foreach (Autodesk.DesignScript.Geometry.Curve c in m_perimeter)
            {
                m_points.Add(c.StartPoint);
            }

            return new Dictionary<string, object>
            {
                {"Edges", m_perimeter},
                {"Points", m_points}
            };
        }


        /// <summary>
        /// Rebuild the Surface into desired U and V space
        /// </summary>
        /// <param name="surface">Surface</param>
        /// <param name="uDiv">Number of U spans after rebuilding</param>
        /// <param name="vDiv">Number of V spans after rebuilding</param>
        /// <returns name="Rebuilt">Rebuilt surface</returns>
        [MultiReturn(new[] {"Rebuilt"})]
        public static Dictionary<string, object?> RebuildSurface(Surface surface, int uDiv, int vDiv)
        {
            double mUStep = 1.0 / uDiv;
            double mVStep = 1.0 / vDiv;
            List<NurbsCurve> crvs = new List<NurbsCurve>();
            Surface? s = null;
            for (int i = 0; i <= uDiv; i++)
            {
                List<Autodesk.DesignScript.Geometry.Point> mPtrow = new List<Autodesk.DesignScript.Geometry.Point>();

                for (int j = 0; j <= vDiv; j++)
                {
                    double u = mUStep * i;
                    double v = mVStep * j;

                    Autodesk.DesignScript.Geometry.Point mPt = surface.PointAtParameter(u, v);

                    mPtrow.Add(mPt);
                }

                NurbsCurve crv = NurbsCurve.ByPoints(mPtrow);
                crvs.Add(crv);
            }

            s = Surface.ByLoft(crvs);

            return new Dictionary<string, object?>
            {
                {"Rebuilt", s},
            };
        }

        /// <summary>
        /// Transpose the UV space of the surface
        /// </summary>
        /// <param name="surface">Surface</param>
        /// <returns name="Transposed">Transposed Surface</returns>
        [MultiReturn(new[] {"Transposed"})]
        public static Dictionary<string, object> TransposeSurfaceUV(Surface surface)
        {
            NurbsSurface n = surface.ToNurbsSurface();
            Autodesk.DesignScript.Geometry.Point[][] ctrlPts = n.ControlPoints();
            Double[][] weights = n.Weights();
            Double[] knotsU = n.UKnots();
            Double[] knotsV = n.VKnots();
            int degreeU = n.DegreeU;
            int degreeV = n.DegreeV;
            int uNumber = n.NumControlPointsU;
            int vNumber = n.NumControlPointsV;

            Autodesk.DesignScript.Geometry.Point[][] transPts = new Autodesk.DesignScript.Geometry.Point[vNumber][];
            double[][] transWeights = new double[vNumber][];
            for (int v = 0; v < vNumber; v++)
            {
                transPts[v] = new Autodesk.DesignScript.Geometry.Point[uNumber];
                transWeights[v] = new double[uNumber];
                for (int u = 0; u < uNumber; u++)
                {
                    transPts[v][u] = ctrlPts[u][v];
                    transWeights[v][u] = weights[u][v];
                }
            }

            Surface s = NurbsSurface.ByControlPointsWeightsKnots(transPts, transWeights, knotsV, knotsU, degreeV,
                degreeU);

            return new Dictionary<string, object>
            {
                {"Transposed", s},
            };
        }

        /// <summary>
        /// Deconstruct a Surface as a NurbsSurface
        /// </summary>
        /// <param name="surface">Surface</param>
        /// <returns name="CtrlPoints">NurbsSurface control points</returns>
        /// <returns name="Weights">Control point weights</returns>
        /// <returns name="KnotsU">Knots in the U direction</returns>
        /// <returns name="KnotsV">Knots in the V direction</returns>
        /// <returns name="DegreeU">Degree in the U direction</returns>
        /// <returns name="DegreeU">Degree in the V direction</returns>
        /// <returns name="NumberU">Number of control points in the U direction</returns>
        /// <returns name="NumberV">Number of control points in the V direction</returns>
        [MultiReturn(new[] {"CtrlPoints", "Weights", "KnotsU", "KnotsV", "DegreeU", "DegreeV", "NumberU", "NumberV"})]
        public static Dictionary<string, object> DeconstructNurbsSurface(Surface surface)
        {
            NurbsSurface n = surface.ToNurbsSurface();
            Autodesk.DesignScript.Geometry.Point[][] ctrlPts = n.ControlPoints();
            Double[][] weights = n.Weights();
            Double[] knotsU = n.UKnots();
            Double[] knotsV = n.VKnots();
            int degreeU = n.DegreeU;
            int degreeV = n.DegreeV;
            int uNumber = n.NumControlPointsU;
            int vNumber = n.NumControlPointsV;

            return new Dictionary<string, object>
            {
                {"CtrlPoints", ctrlPts},
                {"Weights", weights},
                {"KnotsU", knotsU},
                {"KnotsV", knotsV},
                {"DegreeU", degreeU},
                {"DegreeV", degreeV},
                {"NumberU", uNumber},
                {"NumberV", vNumber}
            };
        }


        /// <summary>
        /// Deconstructs a polysurface into faces, edge curves, and points
        /// </summary>
        /// <param name="polysurface">PolySurface</param>
        /// <returns name="Faces">Face surfaces.</returns>
        /// <returns name="Points">Corner points.</returns>
        [MultiReturn(new[] {"Faces", "Points"})]
        public static Dictionary<string, object> DeconstructPolySurface(PolySurface polysurface)
        {
            Surface[] m_faces = polysurface.Surfaces();
            List<List<Curve>> m_perimeter = new List<List<Curve>>();
            List<Autodesk.DesignScript.Geometry.Point> m_points = new List<Autodesk.DesignScript.Geometry.Point>();

            foreach (Surface s in polysurface.Surfaces())
            {
                foreach (Autodesk.DesignScript.Geometry.Curve c in s.PerimeterCurves())
                {
                    m_points.Add(c.StartPoint);
                }
            }

            Autodesk.DesignScript.Geometry.Point[] m_nodups = Autodesk.DesignScript.Geometry.Point.PruneDuplicates(m_points);

            return new Dictionary<string, object>
            {
                {"Faces", m_faces},
                {"Points", m_nodups}
            };
        }
    }