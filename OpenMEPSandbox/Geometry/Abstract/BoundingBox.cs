using Autodesk.DesignScript.Geometry;
using Dynamo.Graph.Nodes;

namespace OpenMEPSandbox.Geometry.Abstract;

public class BoundingBox
{
    private BoundingBox()
    {
    }

    /// <summary>
    /// Create a bounding box from a list of points
    /// </summary>
    /// <param name="points">the list point to create new bounding box</param>
    /// <returns name="boundingBox">new boundingBox from list point</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/BoundingBox.ByPoints.png)
    /// [BoundingBox.ByPoints.dyn](../OpenMEPPage/geometry/dyn/BoundingBox.ByPoints.dyn)
    ///</example>
    [NodeCategory("Create")]
    public static Autodesk.DesignScript.Geometry.BoundingBox ByPoints(List<Autodesk.DesignScript.Geometry.Point> points)
    {
        if (points.Count == 0)
        {
            throw new ArgumentNullException($"Points is empty");
        }

        if (points.Count == 1)
        {
            throw new ArgumentNullException($"Points require more than 1 point");
        }

        Autodesk.DesignScript.Geometry.Point minPoint = points[0];
        Autodesk.DesignScript.Geometry.Point maxPoint = points[0];
        foreach (var point in points)
        {
            if (point.X < minPoint.X)
            {
                minPoint = Autodesk.DesignScript.Geometry.Point.ByCoordinates(point.X, minPoint.Y, minPoint.Z);
            }

            if (point.Y < minPoint.Y)
            {
                minPoint = Autodesk.DesignScript.Geometry.Point.ByCoordinates(minPoint.X, point.Y, minPoint.Z);
            }

            if (point.Z < minPoint.Z)
            {
                minPoint = Autodesk.DesignScript.Geometry.Point.ByCoordinates(minPoint.X, minPoint.Y, point.Z);
            }

            if (point.X > maxPoint.X)
            {
                maxPoint = Autodesk.DesignScript.Geometry.Point.ByCoordinates(point.X, maxPoint.Y, maxPoint.Z);
            }

            if (point.Y > maxPoint.Y)
            {
                maxPoint = Autodesk.DesignScript.Geometry.Point.ByCoordinates(maxPoint.X, point.Y, maxPoint.Z);
            }

            if (point.Z > maxPoint.Z)
            {
                maxPoint = Autodesk.DesignScript.Geometry.Point.ByCoordinates(maxPoint.X, maxPoint.Y, point.Z);
            }
        }

        return Autodesk.DesignScript.Geometry.BoundingBox.ByCorners(minPoint, maxPoint);
    }

    /// <summary>
    /// Get center point of bounding box
    /// </summary>
    /// <param name="boundingBox">the boundingBox need get point center</param>
    /// <returns name="point">center point of boundingBox</returns>
    /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/BoundingBox.Center.png)
    /// [BoundingBox.Center.dyn](../OpenMEPPage/geometry/dyn/BoundingBox.Center.dyn)
    ///</example>
    [NodeCategory("Query")]
    public static Autodesk.DesignScript.Geometry.Point Center(Autodesk.DesignScript.Geometry.BoundingBox boundingBox)
    {
        Autodesk.DesignScript.Geometry.Point maxPoint = boundingBox.MaxPoint;
        Autodesk.DesignScript.Geometry.Point minPoint = boundingBox.MinPoint;
        return Autodesk.DesignScript.Geometry.Point.ByCoordinates((maxPoint.X + minPoint.X) / 2,
            (maxPoint.Y + minPoint.Y) / 2, (maxPoint.Z + minPoint.Z) / 2);
    }

    /// <summary>
    /// Get 8 points corners of bounding box
    /// </summary>
    /// <param name="boundingBox"></param>
    /// <returns name="points">list point corner of the boundingBox</returns>
    /// /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/BoundingBox.Corners.png)
    /// [BoundingBox.Corners.dyn](../OpenMEPPage/geometry/dyn/BoundingBox.Corners.dyn)
    ///</example>
    [NodeCategory("Query")]
    public static List<Autodesk.DesignScript.Geometry.Point> Corners(
        Autodesk.DesignScript.Geometry.BoundingBox boundingBox)
    {
        if (boundingBox == null)
        {
            throw new ArgumentNullException($"BoundingBox is null");
        }
        var minPoint = boundingBox.MinPoint;
        var maxPoint = boundingBox.MaxPoint;
        List<Autodesk.DesignScript.Geometry.Point> corners = new List<Autodesk.DesignScript.Geometry.Point>();
        corners.Add(minPoint);
        corners.Add(Autodesk.DesignScript.Geometry.Point.ByCoordinates(minPoint.X, minPoint.Y, maxPoint.Z));
        corners.Add(Autodesk.DesignScript.Geometry.Point.ByCoordinates(minPoint.X, maxPoint.Y, minPoint.Z));
        corners.Add(Autodesk.DesignScript.Geometry.Point.ByCoordinates(minPoint.X, maxPoint.Y, maxPoint.Z));
        corners.Add(Autodesk.DesignScript.Geometry.Point.ByCoordinates(maxPoint.X, minPoint.Y, minPoint.Z));
        corners.Add(Autodesk.DesignScript.Geometry.Point.ByCoordinates(maxPoint.X, minPoint.Y, maxPoint.Z));
        corners.Add(Autodesk.DesignScript.Geometry.Point.ByCoordinates(maxPoint.X, maxPoint.Y, minPoint.Z));
        corners.Add(maxPoint);
        return corners;
    }

    /// <summary>
    /// Create a bounding box from a list of bounding boxes
    /// </summary>
    /// <param name="boundingBoxes">the list of boundingBox</param>
    /// <returns name="boundingBox">new boundingBox crete by collection boundingBox</returns>
    /// /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/BoundingBox.ByBoundingBoxs.gif)
    /// [BoundingBox.ByBoundingBoxs.dyn](../OpenMEPPage/geometry/dyn/BoundingBox.ByBoundingBoxs.dyn)
    ///</example>
    [NodeCategory("Create")]
    public static Autodesk.DesignScript.Geometry.BoundingBox ByBoundingBoxs(
        List<Autodesk.DesignScript.Geometry.BoundingBox> boundingBoxes)
    {
        if (boundingBoxes.Count == 0)
        {
            throw new ArgumentNullException($"BoundingBoxs is empty");
        }

        if (boundingBoxes.Count == 1)
        {
            return boundingBoxes[0];
        }

        Autodesk.DesignScript.Geometry.Point minPoint = boundingBoxes[0].MinPoint;
        Autodesk.DesignScript.Geometry.Point maxPoint = boundingBoxes[0].MaxPoint;
        foreach (var boundingBox in boundingBoxes)
        {
            if (boundingBox.MinPoint.X < minPoint.X)
            {
                minPoint = Autodesk.DesignScript.Geometry.Point.ByCoordinates(boundingBox.MinPoint.X, minPoint.Y,
                    minPoint.Z);
            }

            if (boundingBox.MinPoint.Y < minPoint.Y)
            {
                minPoint = Autodesk.DesignScript.Geometry.Point.ByCoordinates(minPoint.X, boundingBox.MinPoint.Y,
                    minPoint.Z);
            }

            if (boundingBox.MinPoint.Z < minPoint.Z)
            {
                minPoint = Autodesk.DesignScript.Geometry.Point.ByCoordinates(minPoint.X, minPoint.Y,
                    boundingBox.MinPoint.Z);
            }

            if (boundingBox.MaxPoint.X > maxPoint.X)
            {
                maxPoint = Autodesk.DesignScript.Geometry.Point.ByCoordinates(boundingBox.MaxPoint.X, maxPoint.Y,
                    maxPoint.Z);
            }

            if (boundingBox.MaxPoint.Y > maxPoint.Y)
            {
                maxPoint = Autodesk.DesignScript.Geometry.Point.ByCoordinates(maxPoint.X, boundingBox.MaxPoint.Y,
                    maxPoint.Z);
            }

            if (boundingBox.MaxPoint.Z > maxPoint.Z)
            {
                maxPoint = Autodesk.DesignScript.Geometry.Point.ByCoordinates(maxPoint.X, maxPoint.Y,
                    boundingBox.MaxPoint.Z);
            }
        }

        return Autodesk.DesignScript.Geometry.BoundingBox.ByCorners(minPoint, maxPoint);
    }

    /// <summary>
    /// Returns the volume of the bounding box.
    /// </summary>
    /// <param name="boundingBox">the boundingBox</param>
    /// <returns name="double">volume of the boundingBox</returns>
    /// /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/BoundingBox.Volume.png)
    /// [BoundingBox.Volume.dyn](../OpenMEPPage/geometry/dyn/BoundingBox.Volume.dyn)
    ///</example>
    [NodeCategory("Query")]
    public static double Volume(Autodesk.DesignScript.Geometry.BoundingBox boundingBox)
    {
        var minPoint = boundingBox.MinPoint;
        var maxPoint = boundingBox.MaxPoint;
        return (maxPoint.X - minPoint.X) * (maxPoint.Y - minPoint.Y) * (maxPoint.Z - minPoint.Z);
    }

    /// <summary>
    /// Get area of bounding box
    /// </summary>
    /// <param name="boundingBox">the boundingBox</param>
    /// <returns name="double">the area value of boundingBox</returns>
    /// /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/BoundingBox.Area.png)
    /// [BoundingBox.Area.dyn](../OpenMEPPage/geometry/dyn/BoundingBox.Area.dyn)
    ///</example>
    [NodeCategory("Query")]
    public static double Area(Autodesk.DesignScript.Geometry.BoundingBox boundingBox)
    {
        var minPoint = boundingBox.MinPoint;
        var maxPoint = boundingBox.MaxPoint;
        return 2 * ((maxPoint.X - minPoint.X) * (maxPoint.Y - minPoint.Y) +
                    (maxPoint.X - minPoint.X) * (maxPoint.Z - minPoint.Z) +
                    (maxPoint.Y - minPoint.Y) * (maxPoint.Z - minPoint.Z));
    }

    /// <summary>
    /// Visualize the bounding box by corner points
    /// </summary>
    /// <param name="boundingBox">the boundingBox</param>
    /// <returns name="lines">the list line corner of the boundingBox</returns>
    /// /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/BoundingBox.Visualize.png)
    /// [BoundingBox.Visualize.dyn](../OpenMEPPage/geometry/dyn/BoundingBox.Visualize.dyn)
    ///</example>
    public static List<Autodesk.DesignScript.Geometry.Line> Visualize(Autodesk.DesignScript.Geometry.BoundingBox boundingBox)
    {
        // get 8 corner points
        var corners = Corners(boundingBox);
        // get 12 lines
        List<Autodesk.DesignScript.Geometry.Line> lines = new List<Autodesk.DesignScript.Geometry.Line>();
        lines.Add(Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(corners[0], corners[1]));
        lines.Add(Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(corners[0], corners[2]));
        lines.Add(Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(corners[0], corners[4]));
        lines.Add(Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(corners[1], corners[3]));
        lines.Add(Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(corners[1], corners[5]));
        lines.Add(Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(corners[2], corners[3]));
        lines.Add(Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(corners[2], corners[6]));
        lines.Add(Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(corners[3], corners[7]));
        lines.Add(Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(corners[4], corners[5]));
        lines.Add(Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(corners[4], corners[6]));
        lines.Add(Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(corners[5], corners[7]));
        lines.Add(Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(corners[6], corners[7]));
        return lines;
    }

    /// <summary>
    /// Divide the bounding box by value
    /// </summary>
    /// <param name="boundingBox">the boundingBox</param>
    /// <param name="value">how many part boundingBox will divide for each edge</param>
    /// <returns name="boundingBoxs">the collection boundingBoxs divide</returns>
    /// /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/BoundingBox.Divide.gif)
    /// [BoundingBox.Divide.dyn](../OpenMEPPage/geometry/dyn/BoundingBox.Divide.dyn)
    ///</example>
    public static List<Autodesk.DesignScript.Geometry.BoundingBox> Divide(
        Autodesk.DesignScript.Geometry.BoundingBox boundingBox, double value)
    {
        var minPoint = boundingBox.MinPoint;
        var maxPoint = boundingBox.MaxPoint;
        var x = (maxPoint.X - minPoint.X) / value;
        var y = (maxPoint.Y - minPoint.Y) / value;
        var z = (maxPoint.Z - minPoint.Z) / value;
        List<Autodesk.DesignScript.Geometry.BoundingBox> boundingBoxes =
            new List<Autodesk.DesignScript.Geometry.BoundingBox>();
        for (int i = 0; i < value; i++)
        {
            for (int j = 0; j < value; j++)
            {
                for (int k = 0; k < value; k++)
                {
                    var min = Autodesk.DesignScript.Geometry.Point.ByCoordinates(minPoint.X + i * x, minPoint.Y + j * y,
                        minPoint.Z + k * z);
                    var max = Autodesk.DesignScript.Geometry.Point.ByCoordinates(minPoint.X + (i + 1) * x,
                        minPoint.Y + (j + 1) * y, minPoint.Z + (k + 1) * z);
                    boundingBoxes.Add(Autodesk.DesignScript.Geometry.BoundingBox.ByCorners(min, max));
                }
            }
        }

        return boundingBoxes;
    }
    
    /// <summary>
    /// Divides a bounding box into smaller bounding boxes by width and length.
    /// </summary>
    /// <param name="boundingBox">The original bounding box to be divided.</param>
    /// <param name="width">The number of divisions along the width.</param>
    /// <param name="length">The number of divisions along the length.</param>
    /// <returns name="boundingBox">A list of smaller bounding boxes resulting from the division.</returns>
    /// /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/BoundingBox.DivideByWidthAndLength.gif)
    /// [BoundingBox.DivideByWidthAndLength.dyn](../OpenMEPPage/geometry/dyn/BoundingBox.DivideByWidthAndLength.dyn)
    ///</example>
    public static List<Autodesk.DesignScript.Geometry.BoundingBox> DivideByWidthAndLength(
        Autodesk.DesignScript.Geometry.BoundingBox boundingBox, double width, double length)
    {
        var minPoint = boundingBox.MinPoint;
        var maxPoint = boundingBox.MaxPoint;
        var x = (maxPoint.X - minPoint.X) / width;
        var y = (maxPoint.Y - minPoint.Y) / length;
        List<Autodesk.DesignScript.Geometry.BoundingBox> boundingBoxes =
            new List<Autodesk.DesignScript.Geometry.BoundingBox>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                var min = Autodesk.DesignScript.Geometry.Point.ByCoordinates(minPoint.X + i * x, minPoint.Y + j * y,
                    minPoint.Z);
                var max = Autodesk.DesignScript.Geometry.Point.ByCoordinates(minPoint.X + (i + 1) * x,
                    minPoint.Y + (j + 1) * y, maxPoint.Z);
                boundingBoxes.Add(Autodesk.DesignScript.Geometry.BoundingBox.ByCorners(min, max));
            }
        }
        return boundingBoxes;
    }

    /// <summary>
    /// Scale the bounding box by value
    /// </summary>
    /// <param name="boundingBox">the boundingBox need scale</param>
    /// <param name="value">scale value</param>
    /// <returns name="boundingBox">boundingBox</returns>
    /// /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/BoundingBox.Scale.png)
    /// [BoundingBox.Scale.dyn](../OpenMEPPage/geometry/dyn/BoundingBox.Scale.dyn)
    ///</example>
    public static Autodesk.DesignScript.Geometry.BoundingBox Scale(
        Autodesk.DesignScript.Geometry.BoundingBox boundingBox, double value)
    {
        // scale apply for value > 1 and value < 1
        var minPoint = boundingBox.MinPoint;
        var maxPoint = boundingBox.MaxPoint;
        var x = (maxPoint.X - minPoint.X) * value;
        var y = (maxPoint.Y - minPoint.Y) * value;
        var z = (maxPoint.Z - minPoint.Z) * value;
        var min = Autodesk.DesignScript.Geometry.Point.ByCoordinates(minPoint.X - x, minPoint.Y - y, minPoint.Z - z);
        var max = Autodesk.DesignScript.Geometry.Point.ByCoordinates(maxPoint.X + x, maxPoint.Y + y, maxPoint.Z + z);
        return Autodesk.DesignScript.Geometry.BoundingBox.ByCorners(min, max);
    }

    /// <summary>
    /// Determines whether two bounding boxes are similar within a specified tolerance.
    /// </summary>
    /// <param name="boundingBox1">The first bounding box to compare.</param>
    /// <param name="boundingBox2">The second bounding box to compare.</param>
    /// <param name="tolerance">The acceptable tolerance for dimension differences (default is 0.001).</param>
    /// <returns>True if the bounding boxes are similar within the tolerance; otherwise, false.</returns>
    /// /// <example>
    /// ![](../OpenMEPPage/geometry/dyn/pic/BoundingBox.IsSimilar.png)
    /// [BoundingBox.IsSimilar.dyn](../OpenMEPPage/geometry/dyn/BoundingBox.IsSimilar.dyn)
    ///</example>
    [NodeCategory("Query")]
    public static bool IsSimilar(Autodesk.DesignScript.Geometry.BoundingBox boundingBox1, Autodesk.DesignScript.Geometry.BoundingBox boundingBox2,double tolerance = 0.001)
    {
        var minPoint1 = boundingBox1.MinPoint;
        var maxPoint1 = boundingBox1.MaxPoint;
        var minPoint2 = boundingBox2.MinPoint;
        var maxPoint2 = boundingBox2.MaxPoint;
        var x1 = maxPoint1.X - minPoint1.X;
        var y1 = maxPoint1.Y - minPoint1.Y;
        var z1 = maxPoint1.Z - minPoint1.Z;
        var x2 = maxPoint2.X - minPoint2.X;
        var y2 = maxPoint2.Y - minPoint2.Y;
        var z2 = maxPoint2.Z - minPoint2.Z;
        if (System.Math.Abs(x1 - x2) > tolerance) return false;
        if (System.Math.Abs(y1 - y2) > tolerance) return false;
        if (System.Math.Abs(z1 - z2) > tolerance) return false;
        return true;
    }
}