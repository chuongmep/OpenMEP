namespace OpenMEP.Element;

public class FamilyInstance
{
    private FamilyInstance()
    {
        
    }
    /// <summary>
    /// Retrieves the MEP model for the family instance.</summary>
    /// <remarks>If the family instance has a MEP model it is returned by this method, otherwise <see langword="null" /> is
    /// returned. Different types of MEP model will be returned based on the type of the instance, for
    /// example - if the instance is a lighting device then a lighting device model will be returned.
    /// This property will only function with the Autodesk Revit MEP product.</remarks>
    /// <param name="familyInstance">the element to get MepModel</param>
    /// <returns name="mepModel">Autodesk.Revit.DB.MEPModel</returns>
    public static Autodesk.Revit.DB.MEPModel? GetMEPModel(global::Revit.Elements.Element familyInstance)
    {
        Autodesk.Revit.DB.Element internalElement = familyInstance.InternalElement;
        if (internalElement is Autodesk.Revit.DB.FamilyInstance fam)
        {
            return fam.MEPModel;
        }
        return null;
    }
}