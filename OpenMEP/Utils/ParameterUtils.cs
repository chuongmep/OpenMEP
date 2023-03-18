using Autodesk.Revit.DB;

namespace OpenMEP.Utils;
/// <summary>A utility class of functions related to parameters.</summary>
/// <since>2022</since>
public class ParameterUtils
{
    private ParameterUtils()
    {
        
    }

#if R22 || R23
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
    
    /// <summary>
    /// Checks whether a ForgeTypeId identifies a built-in parameter.
    /// </summary>
    /// <param name="parameterTypeId">The identifier to check.</param>
    /// <returns>True if the ForgeTypeId identifies a built-in parameter, false otherwise.</returns>
    public static bool IsBuiltInParameter(ForgeTypeId parameterTypeId)
    {
        bool result = Autodesk.Revit.DB.ParameterUtils.IsBuiltInParameter(parameterTypeId);
        return result;
    }
    
    /// <summary>
    /// Gets the ForgeTypeId identifying the built-in parameter corresponding to the given BuiltInParameter value.
    /// </summary>
    /// <param name="builtInParameter">The BuiltInParameter value.</param>
    /// <returns>Identifier of the parameter corresponding to the given BuiltInParameter value.</returns>
    public static ForgeTypeId GetParameterTypeId(object builtInParameter)
    {
        BuiltInParameter bip = (BuiltInParameter)builtInParameter;
        ForgeTypeId result = Autodesk.Revit.DB.ParameterUtils.GetParameterTypeId(bip);
        return result;
    }
    
#endif
    
    
}