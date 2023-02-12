using Autodesk.Revit.DB;

namespace Parameter;

public class Definition
{
    private Definition()
    {
    }

    /// <summary>
    /// The user visible name for the parameter.
    /// </summary>
    /// <param name="definition">definition</param>
    /// <returns name="name">name</returns>
    public static string Name(Autodesk.Revit.DB.Definition definition)
    {
        return definition.Name;
    }

    /// <summary>
    /// The user visible name for the parameter.
    /// </summary>
    /// <param name="definition">definition</param>
    /// <returns name="BuiltInParameterGroup">BuiltInParameterGroup</returns>
    public static dynamic ParameterGroup(Autodesk.Revit.DB.Definition definition)
    {
        return definition.ParameterGroup;
    }


#if R21
    /// <summary>
    /// Returns the identifier of the parameter definition's parameter group.
    /// </summary>
    /// <param name="definition">definition</param>
    /// <returns name="name">ForgeTypeId</returns>
    public static dynamic ParameterType(Autodesk.Revit.DB.Definition definition)
    {
        return definition.ParameterType;
    }

    /// <summary>
    /// Gets the identifier of the spec describing values of the parameter.
    /// </summary>
    /// <param name="definition">definition</param>
    /// <returns name="ForgeTypeId">ForgeTypeId</returns>
    [Obsolete(
        "This method is deprecated in Revit 2022 and may be removed in a future version of Revit. Please use the GetDataType() method instead")]
    public static Autodesk.Revit.DB.ForgeTypeId GetSpecTypeId(Autodesk.Revit.DB.Definition definition)
    {
        return definition.GetSpecTypeId();
    }

    /// <summary>
    /// Gets the identifier of the spec describing values of the parameter.
    /// </summary>
    /// <param name="definition">definition</param>
    /// <returns name="ForgeTypeId">ForgeTypeId</returns>
    [Obsolete(
        "This property is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `GetSpecTypeId()")]
    public static UnitType UnitType(Autodesk.Revit.DB.Definition definition)
    {
        return definition.UnitType;
    }
#elif R22
        /// <summary>
        /// Returns the identifier of the parameter definition's parameter group.
        /// This property is deprecated in Revit 2022 and may be removed in a future version of Revit. Please use the GetDataType() method 
        /// </summary>
        /// <param name="definition">definition</param>
        /// <returns name="name">ParameterType</returns>
        [Obsolete("This property is deprecated in Revit 2022 and may be removed in a future version of Revit. Please use the GetDataType() method ")]
        public static ParameterType ParameterType(Autodesk.Revit.DB.Definition definition)
        {
            return definition.ParameterType;
        }
        /// <summary>
        /// Gets the identifier of the spec describing values of the parameter.
        /// </summary>
        /// <param name="definition">definition</param>
        /// <returns name="ForgeTypeId">ForgeTypeId</returns>
        [Obsolete("This method is deprecated in Revit 2022 and may be removed in a future version of Revit. Please use the GetDataType() method instead")]
        public static Autodesk.Revit.DB.ForgeTypeId GetSpecTypeId(Autodesk.Revit.DB.Definition definition)
        {
            return definition.GetSpecTypeId();
        }
        /// <summary>
        /// Gets a ForgeTypeId identifying the data type describing values of the parameter.
        /// </summary>
        /// <param name="definition">definition</param>
        /// <returns name="ForgeTypeId">ForgeTypeId</returns>
        public static Autodesk.Revit.DB.ForgeTypeId GetDataType(Autodesk.Revit.DB.Definition definition)
        {
            return definition.GetDataType();
        }

#elif R23
    /// <summary>
    /// Gets a ForgeTypeId identifying the data type describing values of the parameter.
    /// </summary>
    /// <param name="definition">definition</param>
    /// <returns name="name">ForgeTypeId</returns>
    public static Autodesk.Revit.DB.ForgeTypeId GetDataType(Autodesk.Revit.DB.Definition definition)
    {
        return definition.GetDataType();
    }
#endif


#if R22 || R23
        /// <summary>
        /// Returns the identifier of the parameter definition's parameter group.
        /// </summary>
        /// <param name="definition">definition</param>
        /// <returns name="name">ForgeTypeId</returns>
        public static Autodesk.Revit.DB.ForgeTypeId GetGroupTypeId(Autodesk.Revit.DB.Definition definition)
        {
            return definition.GetGroupTypeId();
        }
#endif
}