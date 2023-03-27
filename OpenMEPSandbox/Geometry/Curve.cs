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
    /// <returns name="curves">A list of curves that result from splitting the input curve at the specified segment lengths. The list includes the original curve and the newly created segments.</returns>
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
    /// <returns name="curves">A list of curves that result from splitting the input curve at the specified segment lengths. The list includes the original curve and the newly created segments.</returns>
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
}