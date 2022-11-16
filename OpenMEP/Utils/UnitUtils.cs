using Autodesk.Revit.DB;

namespace Utils;

public class UnitUtils
{
    private UnitUtils()
    {
        // TODO 
        // UnitUtils.IsSymbol()
        //
        // UnitUtils.GetSpecTypeId()
        //
        // UnitUtils.GetUnitType()
        //
        // UnitUtils.GetUnitTypeId()
        //
        // UnitUtils.GetDisplayUnitType()
        //
        // UnitUtils.GetSymbolTypeId()
        //
        // UnitUtils.GetUnitSymbolType()
    }

#if R21 || R22 || R23
    /// <summary>
    /// Gets the identifiers of all available units.
    /// </summary>
    /// <returns name="Units">The unit identifiers.</returns>
    public static IList<ForgeTypeId> GetAllUnits()
    {
        return Autodesk.Revit.DB.UnitUtils.GetAllUnits();
    }
#endif
#if R20
#elif R21
    public static IList<Autodesk.Revit.DB.ForgeTypeId> GetAllSpec()
    {
        return Autodesk.Revit.DB.UnitUtils.GetAllSpecs();
    }
#elif R22
    [Obsolete("This method is deprecated in Revit 2022 and may be removed in a future version of Revit. Please use the `GetAllMeasurableSpecs()` method instead.")]
    public static IList<Autodesk.Revit.DB.ForgeTypeId> GetAllSpec()
    {
        return Autodesk.Revit.DB.UnitUtils.GetAllSpecs();
    }
#else
    /// <summary>
    /// Gets the identifiers of all available measurable specs.
    /// </summary>
    /// <returns name="spec">The spec identifiers.</returns>
    public static IList<Autodesk.Revit.DB.ForgeTypeId> GetAllMeasurableSpecs()
    {
        return Autodesk.Revit.DB.UnitUtils.GetAllMeasurableSpecs();
    }
#endif

#if R22 || R23
    /// <summary>
    /// Gets the identifiers of all available disciplines.
    /// </summary>
    /// <returns name="Disciplines">The discipline identifiers.</returns>
    /// <since>2022</since>
    public static IList<Autodesk.Revit.DB.ForgeTypeId> GetAllDisciplines()
    {
        return  Autodesk.Revit.DB.UnitUtils.GetAllDisciplines();
    }
#endif

#if R20
    #else
    /// <summary>
    /// Gets the identifiers of all valid units for a given spec.
    /// </summary>
    /// <returns name="Units">The discipline identifiers.</returns>
    /// <since>2021</since>
    public static IList<Autodesk.Revit.DB.ForgeTypeId> GetValidUnits(Autodesk.Revit.DB.ForgeTypeId specTypeId)
    {
        return Autodesk.Revit.DB.UnitUtils.GetValidUnits(specTypeId);
    }
#endif
}