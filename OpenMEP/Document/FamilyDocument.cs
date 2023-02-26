using Autodesk.Revit.DB;

namespace OpenMEP.Document;

public class FamilyDocument
{
    private FamilyDocument(){}
    
    /// <summary>
    /// The family manager object provides access to family types and parameters.
    /// </summary>
    /// <param name="familyDocument">family document</param>
    /// <returns name="familyManager">family manager</returns>
    public static FamilyManager FamilyManager(Autodesk.Revit.DB.Document familyDocument)
    {
        if (familyDocument.IsFamilyDocument)
        {
            return familyDocument.FamilyManager;
        }
        throw new ArgumentException("input document require is family document");
    }
}