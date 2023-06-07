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
    /// Get Real Name of the Dynamic BlockReference
    /// </summary>
    /// <param name="blockRef"></param>
    /// <returns name="string">real name of Dynamic BlockReference</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string RealName(Autodesk.AutoCAD.DynamoNodes.BlockReference blockRef)
    {
        ObjectId objectId = blockRef.InternalObjectId;
        // cast to Autodesk.AutoCAD.DatabaseServices.BlockReference
        Autodesk.AutoCAD.DatabaseServices.BlockReference? blockReference =
            objectId.GetObject(OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.BlockReference;
        if(blockReference== null) throw new ArgumentNullException(nameof(blockReference));
        Document doc = Application.DocumentManager.MdiActiveDocument;
        Editor editor = doc.Editor;
        Database db = doc.Database;
        string realName;
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            // Get name of anonymous block
            if (blockReference.IsDynamicBlock)
            {
                BlockTableRecord? blockTableRecord =
                    tr.GetObject(blockReference.DynamicBlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
                realName = blockTableRecord?.Name ?? String.Empty;
            }
            else
            {
                realName = blockReference?.Name ?? String.Empty;
            }
        }
        return realName;
    }
}