using Autodesk.DesignScript.Geometry;
using Dynamo.Graph.Nodes;

namespace OpenMEPSandbox.Autocad;

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
    public static string Name(CadObject AcadBlock)
    {
        return AcadBlock.CadEntity.Name;
    }
    
    /// <summary>
    /// Gets the AutoCAD class name of the object. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="string">The AutoCAD class name of an object. </returns>
    public static string ObjectName(CadObject AcadBlock)
    {
        return AcadBlock.CadEntity.ObjectName;
    }
    
    /// <summary>
    /// Gets the object ID of the owner (parent) object. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="ownerID">The object ID of an object's owner. </returns>
    public static dynamic OwnerID(CadObject AcadBlock)
    {
        return AcadBlock.CadEntity.OwnerID;
    }
    
    /// <summary>
    /// Specifies whether this is a dynamic block. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="bool">true if The block is a dynamic block. </returns>
    [NodeCategory("Query")]
    public static bool IsDynamicBlock(CadObject AcadBlock)
    {
        return AcadBlock.CadEntity.IsDynamicBlock;
    }
    
    /// <summary>
    /// Specifies the scaling allowed for the block.
    /// https://help.autodesk.com/view/OARX/2023/ENU/?guid=GUID-F8A1C955-9669-4132-BBF3-6F7BE4710471
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="BlockScaling">enum of scaling</returns>
    public static dynamic BlockScaling(CadObject AcadBlock)
    {
        return AcadBlock.CadEntity.BlockScaling;
    }
    
    /// <summary>
    /// Gets the handle of an object. 
    /// </summary>
    /// <param name="AcadBlock"></param>
    /// <returns name="handle">The handle of the entity. </returns>
    public static string Handle(CadObject AcadBlock)
    {
        return AcadBlock.CadEntity.Handle;
    }
    
    /// <summary>
    /// Determines whether the object has an extension dictionary associated with it. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="bool">true if The object has an extension dictionary associated with it. </returns>
    [NodeCategory("Query")]
    public static bool HasExtensionDictionary(CadObject AcadBlock)
    {
        return AcadBlock.CadEntity.HasExtensionDictionary;
    }
    
    /// <summary>
    /// Gets the object ID. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="long">The object ID of an object. </returns>
    public static long ObjectID(CadObject AcadBlock)
    {
        return AcadBlock.ObjectID;
    }
    
    /// <summary>
    /// Determines whether the given block is an XRef block. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns>true if block is an Xref block</returns>
    [NodeCategory("Query")]
    public static bool IsXRef(CadObject AcadBlock)
    {
        return AcadBlock.CadEntity.IsXRef;
    }
    /// <summary>
    /// Determines whether the given block is a layout block. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="bool"> true if block is layout block</returns>
    [NodeCategory("Query")]
    public static bool IsLayout(CadObject AcadBlock)
    {
        return AcadBlock.CadEntity.IsLayout;
    }
    /// <summary>
    /// Specifies the origin of the UCS, block, hatch, or raster image in WCS coordinates. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns anme="point"></returns>
    public static Point Origin(CadObject AcadBlock)
    {
        double[]? doubles = AcadBlock.CadEntity.Origin as double[];
        return Point.ByCoordinates(doubles[0], doubles[1], doubles[2]);
    }
    /// <summary>
    /// Specifies the native units of measure for the block. 
    /// </summary>
    /// <param name="AcadBlock">AcadBlock</param>
    /// <returns name="enum unit"></returns>
    public static dynamic Units(CadObject AcadBlock)
    {
        return AcadBlock.CadEntity.Units;
    }
    
}