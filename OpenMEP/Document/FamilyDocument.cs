using Autodesk.Revit.DB;

namespace OpenMEP.Document;

public class FamilyDocument
{
    private FamilyDocument(){}
    
    /// <summary>
    /// The family manager object provides access to family types and parameters.
    /// </summary>
    /// <param name="doc">family document</param>
    /// <returns name="familyManager">family manager</returns>
    public static FamilyManager FamilyManager(Autodesk.Revit.DB.Document doc)
    {
        if (doc.IsFamilyDocument)
        {
            return doc.FamilyManager;
        }
        throw new ArgumentException("just support family document");
    }
}