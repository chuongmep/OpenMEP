using Autodesk.Revit.DB;

namespace Utils;

public class ParameterUtils
{
    private ParameterUtils()
    {
        
    }
    
    /// <summary>
    ///  Gets the BuiltInParameter value corresponding to built-in parameter identified by the given ForgeTypeId.
    /// </summary>
    /// <returns>The BuiltInParameter value corresponding to the given parameter identifier.</returns>
    /// <param name="forgeTypeId">The parameter identifier.</param>
    public static dynamic GetBuiltInParameter(ForgeTypeId forgeTypeId)
    {
        BuiltInParameter builtInParameter = Autodesk.Revit.DB.ParameterUtils.GetBuiltInParameter(forgeTypeId);
        return builtInParameter;
    }
}