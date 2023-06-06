using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Dynamo.Graph.Nodes;

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
        /// <returns name="string">name of block reference object</returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.Name.png)
        /// [BlockReference.Name.dyn](../OpenMEPPage/autocad/BlockReference.Name.dyn)
        /// </example>
        public static dynamic Name(CadObject AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.Name;
        }

        /// <summary>
        /// Converts a dynamic block to a regular anonymous block. 
        /// </summary>
        /// <param name="AcadBlockReference">AcadBlockReference</param>
        /// <returns name="AcadBlockReference">AcadBlockReference</returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.ConvertToAnonymousBlock.png)
        /// [BlockReference.ConvertToAnonymousBlock.dyn](../OpenMEPPage/autocad/BlockReference.ConvertToAnonymousBlock.dyn)
        /// </example>
        public static dynamic ConvertToAnonymousBlock(CadObject AcadBlockReference)
        {
            AcadBlockReference.CadEntity.ConvertToAnonymousBlock();
            return AcadBlockReference;
        }

        /// <summary>
        /// Converts a dynamic block to a regular named block. 
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <param name="newBlockName">new name of block</param>
        /// <returns name="AcadBlockReference">AcadBlockReference</returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.ConvertToStaticBlock.png)
        /// [BlockReference.ConvertToStaticBlock.dyn](../OpenMEPPage/autocad/BlockReference.ConvertToStaticBlock.dyn)
        /// </example>
        public static dynamic ConvertToStaticBlock(CadObject AcadBlockReference,string newBlockName)
        {
            AcadBlockReference.CadEntity.ConvertToStaticBlock(newBlockName);
            return AcadBlockReference;
        }
        
        /// <summary>
        /// Get Attributes of the block reference
        /// https://help.autodesk.com/view/OARX/2022/ENU/?guid=GUID-0630EFF2-51A2-46E4-A5A1-0377FB7E38E8
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns>information of AcadAttributeReference</returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.GetAttributes.png)
        /// [BlockReference.GetAttributes.dyn](../OpenMEPPage/autocad/BlockReference.GetAttributes.dyn)
        /// </example>
        [MultiReturn("Tags","TextStrings")]
        public static Dictionary<string,object> GetAttributes(CadObject AcadBlockReference)
        {
            List<string> TextStrings = new List<string>();
            List<string> Tags = new List<string>();
            foreach (var attribute in AcadBlockReference.CadEntity.GetAttributes())
            {
                TextStrings.Add(attribute.TextString);
                Tags.Add(attribute.TagString);
                
            }
            return new Dictionary<string, object>()
            {
                {"Tags",Tags},
                {"TextStrings",TextStrings},
                
            };
        }
        /// <summary>
        /// Gets the AutoCAD class name of the object. 
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="string">The AutoCAD class name of an object. </returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.ObjectName.png)
        /// [BlockReference.ObjectName.dyn](../OpenMEPPage/autocad/BlockReference.ObjectName.dyn)
        /// </example>
        public static dynamic ObjectName(CadObject AcadBlockReference)
        {
            return AcadBlockReference.ObjectName;
        }
        /// <summary>
        /// Gets the object ID. 
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="objectId">Id of Autocad Object</returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.ObjectID.png)
        /// [BlockReference.ObjectID.dyn](../OpenMEPPage/autocad/BlockReference.ObjectID.dyn)
        /// </example>
        public static dynamic ObjectID(CadObject AcadBlockReference)
        {
            return AcadBlockReference.ObjectID;
        }
        
        /// <summary>
        /// Specifies the original block name. 
        /// </summary>
        /// <param name="AcadBlockReference">AcadBlockReference</param>
        /// <returns name="string">The effective name is the name of the block as the user would see it in the user interface.
        /// Dynamic blocks may draw themselves with an anonymous block whose name is
        /// different than the block name the user sees for the block in the user interface.
        /// The Name property returns the name of the block used to draw the reference,
        /// while the EffectiveName is the name the user sees for the reference. </returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.EffectiveName.png)
        /// [BlockReference.EffectiveName.dyn](../OpenMEPPage/autocad/BlockReference.EffectiveName.dyn)
        /// </example>
        public static string EffectiveName(CadObject AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.EffectiveName;
        }
        
        /// <summary>
        /// Specifies the transparency value for the entity. 
        /// </summary>
        /// <param name="AcadBlockReference">AcadBlockReference</param>
        /// <returns name="string">Use one of the following values:
        /// ByLayer: Transparency value determined by layer
        /// ByBlock: Transparency value determined by block
        /// 0: Fully opaque (not transparent)
        /// 1-90: Transparency value defined as a percentage 
        /// </returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.EntityTransparency.png)
        /// [BlockReference.EntityTransparency.dyn](../OpenMEPPage/autocad/BlockReference.EntityTransparency.dyn)
        /// </example>
        public static dynamic EntityTransparency(CadObject AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.EntityTransparency;
        }
        
        /// <summary>
        /// Gets the object ID of the owner (parent) object. 
        /// </summary>
        /// <param name="AcadBlockReference">AcadBlockReference</param>
        /// <returns name="ownerId">The object ID of an object's owner. </returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.OwnerID.png)
        /// [BlockReference.OwnerID.dyn](../OpenMEPPage/autocad/BlockReference.OwnerID.dyn)
        /// </example>
        public static dynamic OwnerID(CadObject AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.OwnerID;
        }
        /// <summary>
        /// Check if block reference is dynamic 
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="bool">True if block is Dynamic </returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.IsDynamicBlock.png)
        /// [BlockReference.IsDynamicBlock.dyn](../OpenMEPPage/autocad/BlockReference.IsDynamicBlock.dyn)
        /// </example>
        [NodeCategory("Query")]
        public static dynamic IsDynamicBlock(CadObject AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.IsDynamicBlock;
        }
    
        /// <summary>
        /// Return Layer name of the block reference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="str"></returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.Layer.png)
        /// [BlockReference.Layer.dyn](../OpenMEPPage/autocad/BlockReference.Layer.dyn)
        /// </example>
        public static string Layer(CadObject AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.Layer;
        }
    
        /// <summary>
        /// Return Rotation of the block reference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="double">angle(degrees)</returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.Rotation.png)
        /// [BlockReference.Rotation.dyn](../OpenMEPPage/autocad/BlockReference.Rotation.dyn)
        /// </example>
        public static double Rotation(CadObject AcadBlockReference)
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
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.Visible.png)
        /// [BlockReference.Visible.dyn](../OpenMEPPage/autocad/BlockReference.Visible.dyn)
        /// </example>
        [NodeCategory("Query")]
        public static bool Visible(CadObject AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.Visible;
        }
        
        /// <summary>
        /// Return Location Insert of Block Reference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="point">location inserted </returns>
        /// <exception cref="Exception"></exception>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.InsertionPoint.png)
        /// [BlockReference.InsertionPoint.dyn](../OpenMEPPage/autocad/BlockReference.InsertionPoint.dyn)
        /// </example>
        public static Point InsertionPoint(CadObject AcadBlockReference)
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
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.Units.png)
        /// [BlockReference.Units.dyn](../OpenMEPPage/autocad/BlockReference.Units.dyn)
        /// </example>
        public static string Units(CadObject AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.InsUnits;
        }
        
        /// <summary>
        /// Return result of InsUnitsFactor
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="double">UnitsFactor</returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.UnitsFactor.png)
        /// [BlockReference.UnitsFactor.dyn](../OpenMEPPage/autocad/BlockReference.UnitsFactor.dyn)
        /// </example>
        public static double UnitsFactor(CadObject AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.InsUnitsFactor;
        }
        
        /// <summary>
        /// Check Attributes of Cad Block Reference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="bool"></returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.HasAttributes.png)
        /// [BlockReference.HasAttributes.dyn](../OpenMEPPage/autocad/BlockReference.HasAttributes.dyn)
        /// </example>
        [NodeCategory("Query")]
        public static bool HasAttributes(CadObject AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.HasAttributes;
        }
        
        /// <summary>
        /// Return Handle value of Block Reference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns></returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.Handle.png)
        /// [BlockReference.Handle.dyn](../OpenMEPPage/autocad/BlockReference.Handle.dyn)
        /// </example>
        public static string Handle(CadObject AcadBlockReference)
        {
            return AcadBlockReference.CadEntity.Handle;
        }
        
        /// <summary>
        /// Highlight BlockReference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <param name="HighlightFlag"></param>
        /// <returns></returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.Highlight.png)
        /// [BlockReference.Highlight.dyn](../OpenMEPPage/autocad/BlockReference.Highlight.dyn)
        /// </example>
        public static void Highlight(CadObject AcadBlockReference,bool HighlightFlag)
        {
            AcadBlockReference.CadEntity.Highlight(HighlightFlag);
        }
        
        /// <summary>
        /// Get Bounding box of BlockReference
        /// </summary>
        /// <param name="AcadBlockReference"></param>
        /// <returns name="BoundingBox"></returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.BoundingBox.png)
        /// [BlockReference.BoundingBox.dyn](../OpenMEPPage/autocad/BlockReference.BoundingBox.dyn)
        /// </example>
        public static BoundingBox BoundingBox(CadObject AcadBlockReference)
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
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/BlockReference.GetDynamicBlockProperties.png)
        /// [BlockReference.GetDynamicBlockProperties.dyn](../OpenMEPPage/autocad/BlockReference.GetDynamicBlockProperties.dyn)
        /// </example>
        [MultiReturn("names","descriptions","unitsTypes","values","allowValues")]
        public static Dictionary<string,object> GetDynamicBlockProperties(CadObject AcadBlockReference)
        {
            List<dynamic> names = new List<dynamic>();
            List<dynamic> Descriptions = new List<dynamic>();
            List<dynamic> UnitsTypes = new List<dynamic>();
            List<dynamic> values = new List<dynamic>();
            List<dynamic> allowValues = new List<dynamic>();
            foreach (dynamic dyn in AcadBlockReference.CadEntity.GetDynamicBlockProperties())
            {
                names.Add(dyn.PropertyName);
                Descriptions.Add(dyn.Description);
                UnitsTypes.Add(dyn.UnitsType);
                values.Add(dyn.Value);
                allowValues.Add(dyn.AllowedValues);
            }
            return new Dictionary<string, object>
            {
                {"names", names},
                {"descriptions", Descriptions},
                {"unitsTypes", UnitsTypes},
                {"values", values},
                {"allowValues", allowValues}
            };
        }
    }
}