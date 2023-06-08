using Autodesk.Revit.DB;

namespace OpenMEPRevit.Document;

/// <summary>An object that represents an open Family In Autodesk Revit project.</summary>
/// <remarks>
///    The Document object represents an Autodesk Revit project. Revit can have multiple
///    projects open and multiple views to those projects. The active or top most view will be the
///    active project and hence the active document which is available from the Application object.
/// </remarks>
public class FamilyDocument
{
    private FamilyDocument(){}
    
    /// <summary>
    /// The family manager object provides access to family types and parameters.
    /// </summary>
    /// <param name="familyDocument">family document</param>
    /// <returns name="familyManager">family manager</returns>
    /// <example>
    /// ![](../OpenMEPPage/document/dyn/pic/FamilyDocument.FamilyManager.png)
    /// </example>
    public static FamilyManager FamilyManager(Autodesk.Revit.DB.Document familyDocument)
    {
        if (familyDocument.IsFamilyDocument)
        {
            return familyDocument.FamilyManager;
        }
        throw new ArgumentException("input document require is family document");
    }
}