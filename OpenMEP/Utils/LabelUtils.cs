using Autodesk.Revit.DB.Structure;

namespace Utils;

public class LabelUtils
{
    private LabelUtils()
    {
        
    }

    /// <summary> Gets the user-visible name for a BuiltInParameter. </summary>
    /// <param name="builtInParam"> The BuiltInParameter to get the user-visible name. </param>
    /// <remarks> The name is obtained in the current Revit language. </remarks>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException"> Thrown when the BuiltInParameter cannot be found. </exception>
    public static string GetLabelFor(Autodesk.Revit.DB.BuiltInParameter builtInParam)
    {
        string label = Autodesk.Revit.DB.LabelUtils.GetLabelFor(builtInParam);
        return label;
    }
    
    /// <summary>
    /// Gets the user-visible name for a BuiltInCategory.
    /// </summary>
    /// <param name="builtInCategory">The BuiltInCategory to get the user-visible name.</param>
    /// <returns name="label">The name is obtained in the current Revit language</returns>
    public static string GetLabelFor(Autodesk.Revit.DB.BuiltInCategory builtInCategory)
    {
        string label = Autodesk.Revit.DB.LabelUtils.GetLabelFor(builtInCategory);
        return label;
    }
}