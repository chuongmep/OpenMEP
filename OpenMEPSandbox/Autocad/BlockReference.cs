using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Autocad
{
    public class BlockReference
    {
        private BlockReference()
        {
        
        }
    
        /// <summary>
        /// Return name of the block reference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="str">name of block reference object</returns>
        public static dynamic Name(dynamic AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.Name;
        }
        /// <summary>
        /// Return ObjectName of the block reference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="str">type name of object entity</returns>
        public static dynamic ObjectName(dynamic AcadBlockReference)
        {
            return AcadBlockReference.ObjectName;
        }
    
        /// <summary>
        /// Check if block reference is dynamic 
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="bool">True if block is Dynamic </returns>
        public static dynamic IsDynamicBlock(dynamic AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.IsDynamicBlock;
        }
    
        /// <summary>
        /// Return Layer name of the block reference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="str"></returns>
        public static string Layer(dynamic AcadBlockReference)
        {
            return AcadBlockReference.Layer;
        }
    
        /// <summary>
        /// Return Rotation of the block reference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="double">angle(degrees)</returns>
        public static double Rotation(dynamic AcadBlockReference)
        {
            double radRotate = AcadBlockReference.CadEntity.Rotation;
            // convert to degress
            return radRotate * 180 / System.Math.PI;
        }
    
        /// <summary>
        /// Return True if block reference is visible
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="bool">true if visible</returns>
        public static bool Visible(dynamic AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.Visible;
        }
        
        /// <summary>
        /// Return Location Insert of Block Reference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="point">location inserted </returns>
        /// <exception cref="Exception"></exception>
        public static Point InsertionPoint(dynamic AcadBlockReference)
        {
            double[] point = AcadBlockReference.CadEntity.InsertionPoint;
            if(point.Length != 3) throw new Exception("Insertion point is not 3D");
            return Point.ByCoordinates(point[0], point[1], point[2]);
        }
        
        /// <summary>
        /// Check Block Reference InsUnits
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="Units">Units</returns>
        public static string Units(dynamic AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.InsUnits;
        }
        
        /// <summary>
        /// Return result of InsUnitsFactor
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="double">UnitsFactor</returns>
        public static double UnitsFactor(dynamic AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.InsUnitsFactor;
        }
        
        /// <summary>
        /// Check Attributes of Cad Block Reference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="bool"></returns>
        public static bool HasAttributes(dynamic AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.HasAttributes;
        }
        
        /// <summary>
        /// Return Handle value of Block Reference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns></returns>
        public static string Handle(dynamic AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.Handle;
        }
        
        /// <summary>
        /// Highlight BlockReference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <param name="HighlightFlag"></param>
        /// <returns></returns>
        public static void Highlight(dynamic AcadBlockReference,bool HighlightFlag)
        {
            AcadBlockReference.CadEntity.Highlight(HighlightFlag);
        }
        
        /// <summary>
        /// Get Bounding box of BlockReference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="BoundingBox"></returns>
        public static BoundingBox BoundingBox(dynamic AcadBlockReference)
        {
            AcadBlockReference.CadEntity.GetBoundingBox(out object minpoint,out object maxpoint);
            double[]? minPoints = minpoint as double[];
            double[]? maxPoints = maxpoint as double[];
            Point min = Point.ByCoordinates(minPoints[0], minPoints[1], minPoints[2]);
            Point max = Point.ByCoordinates(maxPoints[0], maxPoints[1], maxPoints[2]);
            return Autodesk.DesignScript.Geometry.BoundingBox.ByCorners(min, max);
        }

        /// <summary>
        /// Get Dynamic Block Properties
        /// </summary>
        /// <param name="AcadBlockReference">AcadBlockReference</param>
        /// <returns></returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.GetDynamicBlockProperties.png)
        /// [BlockReference.GetDynamicBlockProperties.dyn](../OpenMEPPage/autocad/BlockReference.GetDynamicBlockProperties.dyn)
        /// </example>
        [MultiReturn("names","values","allowValues")]
        public static Dictionary<string,object> GetDynamicBlockProperties(dynamic AcadBlockReference)
        {
            List<dynamic> names = new List<dynamic>();
            List<dynamic> values = new List<dynamic>();
            List<dynamic> allowValues = new List<dynamic>();
            foreach (dynamic dyn in AcadBlockReference.CadEntity.GetDynamicBlockProperties())
            {
                names.Add(dyn.PropertyName);
                values.Add(dyn.Value);
                allowValues.Add(dyn.AllowedValues);
            }
            return new Dictionary<string, object>
            {
                {"names", names},
                {"values", values},
                {"allowValues", allowValues}
            };
        }
    }
}