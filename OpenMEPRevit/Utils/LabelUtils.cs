using Autodesk.Revit.DB;

namespace OpenMEPRevit.Utils;
/// <summary>Used to obtain user-visible names for enums.</summary>
/// <since>2011</since>
public class LabelUtils
{
    private LabelUtils()
    {
        
    }
    
    /// <summary> Gets the user-visible name for a BuiltInParameter. </summary>
    /// <param name="builtInParam"> The BuiltInParameter to get the user-visible name. </param>
    /// <remarks> The name is obtained in the current Revit language. </remarks>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException"> Thrown when the BuiltInParameter cannot be found. </exception>
    public static string GetLabelForBuiltInParameter(object builtInParam)
    {
        Autodesk.Revit.DB.BuiltInParameter builtInParameter = (Autodesk.Revit.DB.BuiltInParameter)builtInParam;
        string label = Autodesk.Revit.DB.LabelUtils.GetLabelFor(builtInParameter);
        return label;
    }

#if !(R20 || R21)
    /// <summary>Gets the user-visible name for a unit.</summary>
    /// <remarks>The name is obtained in the current Revit language.</remarks>
    /// <param name="unitTypeId">Identifier of the unit to get the user-visible name.</param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    Cannot find DisplayUnitTypeInfo for the given unit identifier.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <since>2011</since>
    public static string GetLabelForUnit(ForgeTypeId unitTypeId)
    {
        string label = Autodesk.Revit.DB.LabelUtils.GetLabelForUnit(unitTypeId);
        return label;
    }
#endif
}