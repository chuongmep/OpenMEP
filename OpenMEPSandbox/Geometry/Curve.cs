using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

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
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Curve.AsLine.png)
    /// [Curve.AsLine.dyn](../OpenMEPPage/geometry/dyn/Curve.AsLine.dyn)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Line AsLine(Autodesk.DesignScript.Geometry.Curve curve)
    {
        return Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(curve.StartPoint, curve.EndPoint);
    }

    /// <summary>
    /// splits the curve at each point in the input list, creating a new curve between each adjacent pair of points
    /// </summary>
    /// <param name="curve">The input curve to be split.</param>
    /// <param name="points">A list of points that represent the locations at which to split the curve.</param>
    /// <returns name="curves">A list of curves that result from splitting the input curve at the specified points</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Curve.SplitByPoints.png)
    /// [Curve.SplitByPoints.dyn](../OpenMEPPage/geometry/dyn/Curve.SplitByPoints.dyn)
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Curve> SplitByPoints(Autodesk.DesignScript.Geometry.Curve curve,
        List<Autodesk.DesignScript.Geometry.Point> points)
    {
        // split curve by list points, return list of curves
        List<Autodesk.DesignScript.Geometry.Curve> curves = new List<Autodesk.DesignScript.Geometry.Curve>();
        Autodesk.DesignScript.Geometry.Point startPoint = curve.StartPoint;
        Autodesk.DesignScript.Geometry.Point endPoint = curve.EndPoint;
        Autodesk.DesignScript.Geometry.Point lastPoint = startPoint;
        // sort points
        points.Sort((x, y) => x.DistanceTo(startPoint).CompareTo(y.DistanceTo(startPoint)));
        foreach (var point in points)
        {
            Autodesk.DesignScript.Geometry.Curve segment =
                Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(lastPoint, point);
            curves.Add(segment);
            lastPoint = point;
        }

        Autodesk.DesignScript.Geometry.Curve lastSegment =
            Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(lastPoint, endPoint);
        curves.Add(lastSegment);
        return curves;
    }

    /// <summary>
    ///  splits a curve into segments of a specified length
    /// </summary>
    /// <param name="curve">The input curve to be split.</param>
    /// <param name="segmentLength">A number that represents the length of each segment to split the curve into, or a list of numbers that represent the lengths of the segments to split the curve into.</param>
    /// <returns name="curves">A list of curves that result from splitting the input curve at the specified segment lengths.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Curve.SplitBySegmentsLength.png)
    /// [Curve.SplitBySegmentsLength.dyn](../OpenMEPPage/geometry/dyn/Curve.SplitBySegmentsLength.dyn)
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Curve> SplitBySegmentsLength(
        Autodesk.DesignScript.Geometry.Curve curve, double segmentLength)
    {
        var curves = new List<Autodesk.DesignScript.Geometry.Curve>();
        var totalLength = curve.Length;
        var currentPosition = 0.0;
        while (currentPosition < totalLength)
        {
            var currentPoint = curve.PointAtParameter(curve.ParameterAtSegmentLength(currentPosition));
            var endParameter = curve.ParameterAtSegmentLength(currentPosition + segmentLength);
            var endPoint = curve.PointAtParameter(endParameter);
            var line = Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(currentPoint, endPoint);
            curves.Add(line);
            currentPosition += segmentLength;
        }

        return curves;
    }

    /// <summary>
    /// splits a curve into segments of varying lengths specified by a list of segment lengths
    /// </summary>
    /// <param name="curve">The input curve to be split.</param>
    /// <param name="segmentLengths"> A list of numbers that represent the lengths of the segments to split the curve into. </param>
    /// <returns name="curves">A list of curves that result from splitting the input curve at the specified segment lengths.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Curve.SplitBySegmentsLengths.png)
    /// [Curve.SplitBySegmentsLengths.dyn](../OpenMEPPage/geometry/dyn/Curve.SplitBySegmentsLengths.dyn)
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Curve> SplitBySegmentsLengths(
        Autodesk.DesignScript.Geometry.Curve curve, List<double> segmentLengths)
    {
        var curves = new List<Autodesk.DesignScript.Geometry.Curve>();
        var totalLength = curve.Length;
        var currentPosition = 0.0;
        foreach (var segmentLength in segmentLengths)
        {
            var currentPoint = curve.PointAtParameter(curve.ParameterAtSegmentLength(currentPosition));
            var endParameter = curve.ParameterAtSegmentLength(currentPosition + segmentLength);
            var endPoint = curve.PointAtParameter(endParameter);
            var line = Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(currentPoint, endPoint);
            curves.Add(line);
            currentPosition += segmentLength;
        }

        if (currentPosition < totalLength)
        {
            var currentPoint = curve.PointAtParameter(curve.ParameterAtSegmentLength(currentPosition));
            var endPoint = curve.EndPoint;
            var line = Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(currentPoint, endPoint);
            curves.Add(line);
        }

        return curves;
    }

    /// <summary>
    /// Generates a Bézier curve segment based on four control points.
    /// </summary>
    /// <param name="start">The starting point of the segment.</param>
    /// <param name="startHandler">The control point that determines the tangent of the curve at the starting point.</param>
    /// <param name="endHandler">The control point that determines the tangent of the curve at the ending point.</param>
    /// <param name="end">The ending point of the segment.</param>
    /// <param name="numPoints">The number of points to generate on the curve.</param>
    /// <returns>A list of Point3D objects representing the points on the Bézier curve segment.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Curve.BezierSegment.png)
    /// [Curve.BezierSegment.dyn](../OpenMEPPage/geometry/dyn/Curve.BezierSegment.dyn)
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Point> BezierSegment(Autodesk.DesignScript.Geometry.Point start,
        Autodesk.DesignScript.Geometry.Point startHandler, Autodesk.DesignScript.Geometry.Point endHandler,
        Autodesk.DesignScript.Geometry.Point end, int numPoints = 20)
    {
        // Generate a set of t values to use in the Bézier curve calculation
        var t = new double[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            t[i] = (double) i / (numPoints - 1);
        }

        // Calculate the x, y, and z coordinates of each point on the Bézier curve
        var xPoints = new double[numPoints];
        var yPoints = new double[numPoints];
        var zPoints = new double[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            double u = 1 - t[i];
            double tt = t[i] * t[i];
            double uu = u * u;
            double uuu = uu * u;
            double ttt = tt * t[i];

            xPoints[i] = uuu * start.X + 3 * uu * t[i] * startHandler.X + 3 * u * tt * endHandler.X + ttt * end.X;
            yPoints[i] = uuu * start.Y + 3 * uu * t[i] * startHandler.Y + 3 * u * tt * endHandler.Y + ttt * end.Y;
            zPoints[i] = uuu * start.Z + 3 * uu * t[i] * startHandler.Z + 3 * u * tt * endHandler.Z + ttt * end.Z;
        }

        // Create a list of Point3D objects to represent the points on the curve
        var points = new List<Autodesk.DesignScript.Geometry.Point>();
        for (int i = 0; i < numPoints; i++)
        {
            points.Add(Autodesk.DesignScript.Geometry.Point.ByCoordinates(xPoints[i], yPoints[i], zPoints[i]));
        }

        return points;
    }

    /// <summary>
    /// Converts a curve to a list of points based on the specified number of points.
    /// </summary>
    /// <param name="curve">The curve to convert to a list of points.</param>
    /// <param name="numPoints">The number of points to generate on the curve.</param>
    /// <returns>A list of Point3D objects representing the points on the curve.</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Curve.ToPoints.png)
    /// [Curve.ToPoints.dyn](../OpenMEPPage/geometry/dyn/Curve.ToPoints.dyn)
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Point> ToPoints(Autodesk.DesignScript.Geometry.Curve curve,
        int numPoints)
    {
        var points = new List<Autodesk.DesignScript.Geometry.Point>();
        double tInterval = 1.0 / (numPoints - 1);

        for (int i = 0; i < numPoints; i++)
        {
            double t = tInterval * i;
            Autodesk.DesignScript.Geometry.Point pt = curve.PointAtParameter(t);
            points.Add(Autodesk.DesignScript.Geometry.Point.ByCoordinates(pt.X, pt.Y, pt.Z));
        }

        return points;
    }

    /// <summary>
    /// Gets start and end points and parameter values
    /// Use Open Source From lunchbox package : https://bitbucket.org/provingground-io/lunchbox-for-dynamo/src/master/
    /// </summary>
    /// <param name="curve">Curve</param>
    /// <returns name="StartPoint">Start point.</returns>
    /// <returns name="EndPoint">Start point.</returns>
    /// <returns name="StartParameter">Start parameter.</returns>
    /// <returns name="EndParameter">End parameter.</returns>
    /// <search>lunchbox,curve,points,start,end,parameter</search>
    [MultiReturn(new[] {"StartPoint", "EndPoint", "StartParameter", "EndParameter"})]
    public static Dictionary<string, object> EndPoints(Autodesk.DesignScript.Geometry.Curve curve)
    {
        Autodesk.DesignScript.Geometry.Point m_start = curve.StartPoint;
        Autodesk.DesignScript.Geometry.Point m_end = curve.EndPoint;

        double m_startparam = curve.StartParameter();
        double m_endparam = curve.EndParameter();

        return new Dictionary<string, object>
        {
            {"StartPoint", m_start},
            {"EndPoint", m_end},
            {"StarParameter", m_startparam},
            {"EndParameter", m_endparam},
        };
    }

    /// <summary>
    /// Divides a curve using a distance between segments
    /// Use Open Source From lunchbox package : https://bitbucket.org/provingground-io/lunchbox-for-dynamo/src/master/
    /// </summary>
    /// <param name="curve">Curve to divide</param>
    /// <param name="Distance">Distance between curve segments</param>
    /// <returns name="Points">A list of points</returns>
    /// <returns name="Planes">A list of planes</returns>
    /// <returns name="Tangents">A list of tangent vectors</returns>
    /// <returns name="Distances">A list of distances</returns>
    /// <returns name="Parameters">A list of parameter values</returns>
    /// <search>lunchbox,curve,point,plane,divide,tangent</search>
    [MultiReturn(new[] {"Points", "Planes", "Tangents", "Distances", "Parameters"})]
    public static Dictionary<string, object> DivideCurveByDistance(Autodesk.DesignScript.Geometry.Curve curve,
        double Distance)
    {
        List<Autodesk.DesignScript.Geometry.Point> m_points = new List<Autodesk.DesignScript.Geometry.Point>();
        List<Autodesk.DesignScript.Geometry.Plane> m_planes = new List<Autodesk.DesignScript.Geometry.Plane>();
        List<double> m_distances = new List<double>();
        List<double> m_parameters = new List<double>();
        List<Autodesk.DesignScript.Geometry.Vector> m_tangents = new List<Autodesk.DesignScript.Geometry.Vector>();

        int Number = Convert.ToInt16(curve.Length / Distance);

        for (int i = 0; i <= Number; i++)
        {
            double m_dist = Distance * i;
            if (m_dist <= curve.Length)
            {
                Autodesk.DesignScript.Geometry.Point m_pt = curve.PointAtChordLength(m_dist);
                Autodesk.DesignScript.Geometry.Plane m_pln = curve.PlaneAtSegmentLength(m_dist);
                Autodesk.DesignScript.Geometry.Vector m_tan = curve.TangentAtParameter(curve.ParameterAtPoint(m_pt));
                double m_param = curve.ParameterAtChordLength(m_dist);

                m_points.Add(m_pt);
                m_planes.Add(m_pln);
                m_tangents.Add(m_tan);
                m_distances.Add(m_dist);
                m_parameters.Add(m_param);
            }
        }

        return new Dictionary<string, object>
        {
            {"Points", m_points},
            {"Planes", m_planes},
            {"Tangents", m_tangents},
            {"Distances", m_distances},
            {"Parameters", m_parameters}
        };
    }

    /// <summary>
    /// Divides a curve evenly along the parameter space
    /// Use Open Source From lunchbox package : https://bitbucket.org/provingground-io/lunchbox-for-dynamo/src/master/
    /// </summary>
    /// <param name="curve">Curve to divide</param>
    /// <param name="Number">Number of divisions</param>
    /// <returns name="Points">A list of points</returns>
    /// <returns name="Planes">A list of planes</returns>
    /// <returns name="Tangents">A list of tangent vectors</returns>
    /// <returns name="Parameters">A list of parameter values</returns>
    /// <search>lunchbox,curve,point,plane,divide,tangent</search>
    [MultiReturn(new[] {"Points", "Planes", "Tangents", "Parameters"})]
    public static Dictionary<string, object> DivideCurve(Autodesk.DesignScript.Geometry.Curve curve, double Number)
    {
        List<Autodesk.DesignScript.Geometry.Point> m_points = new List<Autodesk.DesignScript.Geometry.Point>();
        List<Autodesk.DesignScript.Geometry.Plane> m_planes = new List<Autodesk.DesignScript.Geometry.Plane>();
        List<double> m_parameters = new List<double>();
        List<Autodesk.DesignScript.Geometry.Vector> m_tangents = new List<Autodesk.DesignScript.Geometry.Vector>();

        double m_p1 = curve.StartParameter();
        double m_p2 = curve.EndParameter();
        double m_range = m_p2 - m_p1;
        double m_step = m_range / Number;

        for (int i = 0; i <= Number; i++)
        {
            double m_parm = m_step * i;
            Autodesk.DesignScript.Geometry.Point m_pt = curve.PointAtParameter(m_parm);
            Autodesk.DesignScript.Geometry.Plane m_pln = curve.PlaneAtParameter(m_parm);
            Autodesk.DesignScript.Geometry.Vector m_tan = curve.TangentAtParameter(m_parm);

            m_points.Add(m_pt);
            m_planes.Add(m_pln);
            m_tangents.Add(m_tan);
            m_parameters.Add(m_parm);
        }

        return new Dictionary<string, object>
        {
            {"Points", m_points},
            {"Planes", m_planes},
            {"Tangents", m_tangents},
            {"Parameters", m_parameters}
        };
    }

    /// <summary>
    /// Get segment and vertices of a polycurve
    /// Use Open Source From lunchbox package : https://bitbucket.org/provingground-io/lunchbox-for-dynamo/src/master/
    /// </summary>
    /// <param name="polycurve">PolyCurve</param>
    /// <returns name="Segments">PolyCurve segments.</returns>
    /// <returns name="Points">Points at PolyCurve discontinuities.</returns>
    /// <search>deconstruct,polycurve</search>
    [MultiReturn(new[] {"Segments", "Points"})]
    public static Dictionary<string, object> DeconstructPolyCurve(PolyCurve polycurve)
    {
        Autodesk.DesignScript.Geometry.Curve[] curves = polycurve.Curves();
        List<Autodesk.DesignScript.Geometry.Point> points = new List<Autodesk.DesignScript.Geometry.Point>();
        foreach (Autodesk.DesignScript.Geometry.Curve c in curves)
        {
            points.Add(c.StartPoint);
        }

        return new Dictionary<string, object>
        {
            {"Segments", curves},
            {"Points", points}
        };
    }
}