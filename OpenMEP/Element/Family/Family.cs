using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;
using RevitServices.Persistence;

namespace OpenMEP.Element.Family;

/// <summary>
///    An element that represents a custom family (not a system family) in Autodesk Revit.
/// </summary>
/// <remarks>
///    Custom families within the Revit API represented by three objects - Family, <see cref="T:Autodesk.Revit.DB.FamilySymbol" />
///    and <see cref="T:Autodesk.Revit.DB.FamilyInstance" />.
///    Each object plays a significant part in the structure of families. The Family element represents the entire family
///    that consists of a collection of types, such as an 'I Beam'. You can think of that object as representing the entire
///    family file. The Family object contains a number of <see cref="T:Autodesk.Revit.DB.FamilySymbol" /> elements. The
///    <see cref="T:Autodesk.Revit.DB.FamilySymbol" /> object represents a specific set
///    of family settings within that Family and represents what is known in the Revit user interface as a
///    Type, such as 'W14x32'. The <see cref="T:Autodesk.Revit.DB.FamilyInstance" /> object represents an actual instance of that
///    type placed the Autodesk Revit project. For example the <see cref="T:Autodesk.Revit.DB.FamilyInstance" /> would
///    be a single instance of a W14x32 column within the project.
/// </remarks>
public class Family
{
    
    private Family()
    {
        
    }
    
    /// <summary>
    /// return family document
    /// </summary>
    /// <param name="family">family</param>
    /// <returns name="family document">family document</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/family/dyn/pic/Family.EditFamily.png)
    /// </example>
    public static Autodesk.Revit.DB.Document EditFamily(global::Revit.Elements.Family family)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.DB.Family familyInternalElement = (Autodesk.Revit.DB.Family) family.InternalElement;
        Autodesk.Revit.DB.Document document = doc.EditFamily(familyInternalElement);
        return document;
    }


    /// <summary>
    /// Retrieves or sets a Category object that represents the category or sub category in which the elements ( this family could generate ) reside.
    /// All category objects can be retrieved from the application by using the Categories property of the Application.Settings object.
    /// </summary>
    /// <param name="family">family</param>
    /// <returns name="category">category</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/family/dyn/pic/Family.FamilyCategory.png)
    /// </example>
    public static Revit.Elements.Category? FamilyCategory(global::Revit.Elements.Family? family)
    {
        Autodesk.Revit.DB.Family? internalFamily = family?.InternalElement as Autodesk.Revit.DB.Family;
        return internalFamily?.FamilyCategory.ToDynamoType();
    }

    /// <summary>
    /// True if the family is the owner family for its own editable document, false otherwise.
    /// </summary>
    /// <param name="family">family</param>
    /// <returns name="bool">result</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/family/dyn/pic/Family.IsOwnerFamily.png)
    /// </example>
    [NodeCategory("Query")]
    public static bool? IsOwnerFamily(global::Revit.Elements.Family? family)
    {
        Autodesk.Revit.DB.Family? internalFamily = family?.InternalElement as Autodesk.Revit.DB.Family;
        return internalFamily?.IsOwnerFamily;
    }
}