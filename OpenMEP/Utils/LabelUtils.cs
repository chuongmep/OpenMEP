namespace Utils;

public class LabelUtils
{
    private LabelUtils()
    {
        
    }

    /// <summary>
    /// Gets the user-visible name for a BuiltInParameter.
    /// </summary>
    /// <param name="builtInParameter">The BuiltInParameter to get the user-visible name.</param>
    /// <returns name="label">label of BuiltInParameter </returns>
    /// <remarks>The name is obtained in the current Revit language</remarks>
    public static string GetLabelFor(Autodesk.Revit.DB.BuiltInParameter builtInParameter)
    {
        string label = Autodesk.Revit.DB.LabelUtils.GetLabelFor(builtInParameter);
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