using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using GShark.Geometry;

namespace OpenMEPSandbox.Helpers
{
    [IsVisibleInDynamoLibrary(false)]
    public static class Convert
    {
        
        /// <summary>
        /// Convert a G-Shark vector to a Dynamo vector
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns name="vector">Dynamo vector</returns>
        public static Autodesk.DesignScript.Geometry.Vector ToDynamoType(this GShark.Geometry.Vector3 vector3)
        {
            return Autodesk.DesignScript.Geometry.Vector.ByCoordinates(vector3.X, vector3.Y, vector3.Z);
        }
        
        /// <summary>
        /// Convert a Dynamo Vector to a GShark Vector
        /// </summary>
        /// <param name="vector">Dynamo vector</param>
        /// <returns name="vector">G-shark vector</returns>
        public static GShark.Geometry.Vector3 ToGSharkType(this Autodesk.DesignScript.Geometry.Vector vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }
        
        /// <summary>
        /// Convert a G-Shark point to a Dynamo point
        /// </summary>
        /// <param name="point3">G-Shark point</param>
        /// <returns name="point">Dynamo point</returns>
        public static Autodesk.DesignScript.Geometry.Point ToDynamoType(this Point3 point3)
        {
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(point3.X, point3.Y, point3.Z);
        }
        
        /// <summary>
        /// Convert a Dynamo Point to a GShark Point
        /// </summary>
        /// <param name="point">Dynamo point</param>
        /// <returns name="point">G-Shark point</returns>
        public static Point3 ToGSharkType(this Autodesk.DesignScript.Geometry.Point point)
        {
            return new Point3(point.X, point.Y, point.Z);
        }
        
        /// <summary>
        /// Convert a GShark Line to a Dynamo Line
        /// </summary>
        /// <param name="line">G-Shark line</param>
        /// <returns name="line">Dynamo line</returns>
        public static Autodesk.DesignScript.Geometry.Line ToDynamoType(this GShark.Geometry.Line line)
        {
           return Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(ToDynamoType(line.StartPoint), ToDynamoType(line.EndPoint));
        }
        
        /// <summary>
        /// Convert a Dynamo Line to a GShark Line
        /// </summary>
        /// <param name="line">Dynamo line</param>
        /// <returns name="line">G-Shark line</returns>
        public static GShark.Geometry.Line ToGSharkType(this Autodesk.DesignScript.Geometry.Line line)
        {
            return new GShark.Geometry.Line(ToGSharkType(line.StartPoint), ToGSharkType(line.EndPoint));
        }
        
        /// <summary>
        /// Convert a GShark Plane to a Dynamo Plane
        /// </summary>
        /// <param name="plane">G-Shark plane</param>
        /// <returns name="plane">Dynamo plane</returns>
        public static Autodesk.DesignScript.Geometry.Plane ToDynamoType(this GShark.Geometry.Plane plane)
        {
            return Autodesk.DesignScript.Geometry.Plane.ByOriginNormal(plane.Origin.ToDynamoType(), plane.ZAxis.ToDynamoType());
        }
        
        /// <summary>
        /// Convert a Dynamo Plane to a GShark Plane
        /// </summary>
        /// <param name="plane">Dynamo plane</param>
        /// <returns name="plane">G-shark plane</returns>
        public static GShark.Geometry.Plane ToGSharkType(this Autodesk.DesignScript.Geometry.Plane plane)
        {
            return new GShark.Geometry.Plane(plane.Origin.ToGSharkType(), plane.Normal.ToGSharkType());
        }
        
        /// <summary>
        /// Convert Dynamo Polygon to GShark Polygon
        /// </summary>
        /// <param name="polygon">Dynamo polygon</param>
        /// <returns name="polygon">G-Shark polygon</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static GShark.Geometry.Polygon ToGSharkType(this Autodesk.DesignScript.Geometry.Polygon polygon)
        {
            if (polygon == null) throw new ArgumentNullException(nameof(polygon));
            Point3[] point3s = polygon.Corners().Select(x => x.ToGSharkType()).ToArray();
            return new GShark.Geometry.Polygon(point3s);
        }
        
        /// <summary>
        /// Convert a GShark Polygon to a Dynamo Polygon
        /// </summary>
        /// <param name="polygon">G-Shark polygon</param>
        /// <returns name="polygon">Dynamo polygon</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Autodesk.DesignScript.Geometry.Polygon ToDynamoType(this GShark.Geometry.Polygon polygon)
        {
            if (polygon == null) throw new ArgumentNullException(nameof(polygon));
            Point[] point3s = polygon.ControlPointLocations.Select(x => x.ToDynamoType()).ToArray();
            return Autodesk.DesignScript.Geometry.Polygon.ByPoints(point3s);
        }
        
    }
}