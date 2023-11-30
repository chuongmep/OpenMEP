using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Geometry;

public class Rectangle
{
    private Rectangle()
    {

    }

    /// <summary>
    /// Divide a rectangle into smaller rectangles
    /// </summary>
    /// <param name="rectangle"></param>
    /// <param name="distanceX"></param>
    /// <param name="distanceY"></param>
    /// <returns name="DividedRectangles">Rectangles</returns>
    [MultiReturn("DividedRectangles")]
    public static Dictionary<string, object> Divide(Autodesk.DesignScript.Geometry.Rectangle rectangle, double distanceX, double distanceY)
    {
        Autodesk.DesignScript.Geometry.Surface surface;
        List<Autodesk.DesignScript.Geometry.Rectangle> dividedRectangles = new List<Autodesk.DesignScript.Geometry.Rectangle>();

        double currentX = rectangle.BoundingBox.MinPoint.X;
        while (currentX < rectangle.BoundingBox.MaxPoint.X)
        {
            double currentY = rectangle.BoundingBox.MinPoint.Y;
            while (currentY < rectangle.BoundingBox.MaxPoint.Y)
            {
                double rectWidth = DSCore.Math.Min(distanceX, rectangle.BoundingBox.MaxPoint.X - currentX);
                double rectHeight = DSCore.Math.Min(distanceY, rectangle.BoundingBox.MaxPoint.Y - currentY);

                Autodesk.DesignScript.Geometry.Point startPoint =
                    Autodesk.DesignScript.Geometry.Point.ByCoordinates(currentX, currentY, 0);
                Autodesk.DesignScript.Geometry.Point endPoint =
                    Autodesk.DesignScript.Geometry.Point.ByCoordinates(currentX + rectWidth, currentY + rectHeight, 0);

                // Create the other two points
                Autodesk.DesignScript.Geometry.Point bottomLeft =
                    Autodesk.DesignScript.Geometry.Point.ByCoordinates(currentX, currentY + rectHeight, 0);
                Autodesk.DesignScript.Geometry.Point topRight =
                    Autodesk.DesignScript.Geometry.Point.ByCoordinates(currentX + rectWidth, currentY, 0);

                dividedRectangles.Add(Autodesk.DesignScript.Geometry.Rectangle.ByCornerPoints(startPoint, topRight, endPoint, bottomLeft));

                currentY += distanceY;
            }

            currentX += distanceX;
        }

        return new Dictionary<string, object>
        {
            {"DividedRectangles", dividedRectangles}
        };
    }

}