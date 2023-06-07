using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace OpenMEPCivil.Autocad;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
public class BlockReference
{
    private BlockReference()
    {
        
    }
    
    /// <summary>
    /// Convert Dynamo BlockReference to Origin AutoCAD BlockReference
    /// </summary>
    /// <param name="BlockReference">blockReference</param>
    /// <returns name="AcadBlockReference">AcadBlockReference</returns>
    /// <exception cref="ArgumentNullException"></exception>
    private static Autodesk.AutoCAD.DatabaseServices.BlockReference ToAcadBlockReference(Autodesk.AutoCAD.DynamoNodes.BlockReference BlockReference)
    {
        ObjectId objectId = BlockReference.InternalObjectId;
        // cast to Autodesk.AutoCAD.DatabaseServices.BlockReference
        Autodesk.AutoCAD.DatabaseServices.BlockReference? AcadBlockReference =
            objectId.GetObject(OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.BlockReference;
        if(AcadBlockReference== null) throw new ArgumentNullException(nameof(BlockReference));
        return AcadBlockReference;
    }
    
    /// <summary>
    /// Get Real Name of the Dynamic BlockReference
    /// </summary>
    /// <param name="BlockReference">BlockReference</param>
    /// <returns name="string">real name of Dynamic BlockReference</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string RealName(Autodesk.AutoCAD.DynamoNodes.BlockReference BlockReference)
    {
        var acadBlockReference = ToAcadBlockReference(BlockReference);
        if(acadBlockReference== null) throw new ArgumentNullException(nameof(BlockReference));
        Document doc = Application.DocumentManager.MdiActiveDocument;
        Editor editor = doc.Editor;
        Database db = doc.Database;
        string realName;
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            // Get name of anonymous block
            if (acadBlockReference.IsDynamicBlock)
            {
                BlockTableRecord? blockTableRecord =
                    tr.GetObject(acadBlockReference.DynamicBlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
                realName = blockTableRecord?.Name ?? String.Empty;
            }
            else
            {
                realName = acadBlockReference?.Name ?? String.Empty;
            }
        }
        return realName;
    }

   
}