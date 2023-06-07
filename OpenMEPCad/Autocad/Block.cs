using Autodesk.DesignScript.Geometry;
using Dynamo.Graph.Nodes;

namespace OpenMEPCad.Autocad;

/// <summary>
/// A block definition containing a name and a set of objects.
/// https://help.autodesk.com/view/OARX/2023/ENU/?guid=GUID-E6F7B03B-F5CC-4A18-9C48-BBF1D32A31FD
/// </summary>
public class Block
{
    private Block()
    {
        
    }
    
    /// <summary>
    /// Specifies the name of the object. 
    /// </summary>
    /// <param name="AcadBlock"></param>
    /// <returns name="string">Name of the object.</returns>
    /// <example>
    /// ![](../OpenMEPPage/autocad/pic/Block.Name.png)
    /// [Block.Name.dyn](../OpenMEPPage/autocad/Block.Name.dyn)
    /// </example>
    public static string Name(dynamic AcadBlock)
    {
        return AcadBlock.Name;
    }
    
    /// <summary>
    /// Gets the AutoCAD class name of the object. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="string">The AutoCAD class name of an object. </returns>
    /// <example>
    /// ![](../OpenMEPPage/autocad/pic/Block.ObjectName.png)
    /// [Block.ObjectName.dyn](../OpenMEPPage/autocad/Block.ObjectName.dyn)
    /// </example>
    public static string ObjectName(dynamic AcadBlock)
    {
        return AcadBlock.ObjectName;
    }
    
    /// <summary>
    /// Gets the object ID of the owner (parent) object. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="ownerID">The object ID of an object's owner. </returns>
    /// <example>
    /// ![](../OpenMEPPage/autocad/pic/Block.OwnerID.png)
    /// [Block.OwnerID.dyn](../OpenMEPPage/autocad/Block.OwnerID.dyn)
    /// </example>
    public static dynamic OwnerID(dynamic AcadBlock)
    {
        return AcadBlock.OwnerID;
    }
    
    /// <summary>
    /// Specifies whether this is a dynamic block. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="bool">true if The block is a dynamic block. </returns>
    /// <example>
    /// ![](../OpenMEPPage/autocad/pic/Block.IsDynamicBlock.png)
    /// [Block.IsDynamicBlock.dyn](../OpenMEPPage/autocad/Block.IsDynamicBlock.dyn)
    /// </example>
    [NodeCategory("Query")]
    public static bool IsDynamicBlock(dynamic AcadBlock)
    {
        return AcadBlock.IsDynamicBlock;
    }
    
    /// <summary>
    /// Specifies the scaling allowed for the block.
    /// https://help.autodesk.com/view/OARX/2023/ENU/?guid=GUID-F8A1C955-9669-4132-BBF3-6F7BE4710471
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="BlockScaling">enum of scaling</returns>
    /// <example>
    /// ![](../OpenMEPPage/autocad/pic/Block.BlockScaling.png)
    /// [Block.BlockScaling.dyn](../OpenMEPPage/autocad/Block.BlockScaling.dyn)
    /// </example>
    public static dynamic BlockScaling(dynamic AcadBlock)
    {
        return AcadBlock.BlockScaling;
    }
    
    /// <summary>
    /// Gets the handle of an object. 
    /// </summary>
    /// <param name="AcadBlock"></param>
    /// <returns name="handle">The handle of the entity. </returns>
    /// <example>
    /// ![](../OpenMEPPage/autocad/pic/Block.Handle.png)
    /// [Block.Handle.dyn](../OpenMEPPage/autocad/Block.Handle.dyn)
    /// </example>
    public static string Handle(dynamic AcadBlock)
    {
        return AcadBlock.Handle;
    }
    
    /// <summary>
    /// Determines whether the object has an extension dictionary associated with it. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="bool">true if The object has an extension dictionary associated with it. </returns>
    /// <example>
    /// ![](../OpenMEPPage/autocad/pic/Block.HasExtensionDictionary.png)
    /// [Block.HasExtensionDictionary.dyn](../OpenMEPPage/autocad/Block.HasExtensionDictionary.dyn)
    /// </example>
    [NodeCategory("Query")]
    public static bool HasExtensionDictionary(dynamic AcadBlock)
    {
        return AcadBlock.HasExtensionDictionary;
    }
    
    /// <summary>
    /// Gets the object ID. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="long">The object ID of an object. </returns>
    /// <example>
    /// ![](../OpenMEPPage/autocad/pic/Block.ObjectID.png)
    /// [Block.ObjectID.dyn](../OpenMEPPage/autocad/Block.ObjectID.dyn)
    /// </example>
    public static long ObjectID(dynamic AcadBlock)
    {
        return AcadBlock.ObjectID;
    }
    
    /// <summary>
    /// Determines whether the given block is an XRef block. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns>true if block is an Xref block</returns>
    /// <example>
    /// ![](../OpenMEPPage/autocad/pic/Block.IsXRef.png)
    /// [Block.IsXRef.dyn](../OpenMEPPage/autocad/Block.IsXRef.dyn)
    /// </example>
    [NodeCategory("Query")]
    public static bool IsXRef(dynamic AcadBlock)
    {
        return AcadBlock.IsXRef;
    }
    /// <summary>
    /// Determines whether the given block is a layout block. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="bool"> true if block is layout block</returns>
    /// <example>
    /// ![](../OpenMEPPage/autocad/pic/Block.IsLayout.png)
    /// [Block.IsLayout.dyn](../OpenMEPPage/autocad/Block.IsLayout.dyn)
    /// </example>
    [NodeCategory("Query")]
    public static bool IsLayout(dynamic AcadBlock)
    {
        return AcadBlock.IsLayout;
    }
    /// <summary>
    /// Specifies the origin of the UCS, block, hatch, or raster image in WCS coordinates. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns anme="point"></returns>
    /// <example>
    /// ![](../OpenMEPPage/autocad/pic/Block.Origin.png)
    /// [Block.Origin.dyn](../OpenMEPPage/autocad/Block.Origin.dyn)
    /// </example>
    public static Point Origin(dynamic AcadBlock)
    {
        double[]? doubles = AcadBlock.Origin as double[];
        return Point.ByCoordinates(doubles[0], doubles[1], doubles[2]);
    }
    /// <summary>
    /// Specifies the native units of measure for the block. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="enum unit"></returns>
    /// <example>
    /// ![](../OpenMEPPage/autocad/pic/Block.Units.png)
    /// [Block.Units.dyn](../OpenMEPPage/autocad/Block.Units.dyn)
    /// </example>
    public static dynamic Units(dynamic AcadBlock)
    {
        return AcadBlock.Units;
    }
    
}