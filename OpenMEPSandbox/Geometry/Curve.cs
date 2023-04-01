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

    /// <summary>
    /// splits the curve at each point in the input list, creating a new curve between each adjacent pair of points
    /// </summary>
    /// <param name="curve">The input curve to be split.</param>
    /// <param name="points">A list of points that represent the locations at which to split the curve.</param>
    /// <returns name="curves">A list of curves that result from splitting the input curve at the specified points</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/Curve.SplitByPoints.png)
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
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Point> BezierSegment(Autodesk.DesignScript.Geometry.Point start,
        Autodesk.DesignScript.Geometry.Point startHandler, Autodesk.DesignScript.Geometry.Point endHandler,
        Autodesk.DesignScript.Geometry.Point end, int numPoints = 20)
    {
        // Generate a set of t values to use in the Bézier curve calculation
        var t = new double[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            t[i] = (double)i / (numPoints - 1);
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
    /// </example>
    public static List<Autodesk.DesignScript.Geometry.Point> ToPoints(Autodesk.DesignScript.Geometry.Curve curve, int numPoints)
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
}