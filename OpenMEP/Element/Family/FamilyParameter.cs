using System.Collections;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using OpenMEP.Helpers;

namespace OpenMEP.Element.Family;

public class FamilyParameter
{
    private FamilyParameter()
    {
    }

    /// <summary>
    /// Set the family parameter as an instance parameter.
    /// </summary>
    /// <param name="doc">document</param>
    /// <param name="familyParameter">family parameter</param>
    /// <returns name="familyparameter">family parameter</returns>
    public static object? MakeInstance(Autodesk.Revit.DB.Document doc,
        Autodesk.Revit.DB.FamilyParameter? familyParameter)
    {
        if (!doc.IsFamilyDocument) throw new ArgumentException("just support family document");
        using Autodesk.Revit.DB.Transaction tran = new Autodesk.Revit.DB.Transaction(doc, "make instance");
        tran.Start();
        try
        {
            doc.FamilyManager.MakeInstance(familyParameter);
        }
        catch (Exception e)
        {
            return null;
        }

        tran.Commit();
        return familyParameter;
    }

    /// <summary>
    /// Set the family parameter as a reporting parameter.
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="familyParameter"></param>
    public static void MakeReporting(Autodesk.Revit.DB.Document doc, Autodesk.Revit.DB.FamilyParameter familyParameter)
    {
        if (!doc.IsFamilyDocument) throw new ArgumentException("just support family document");
        using Autodesk.Revit.DB.Transaction tran = new Autodesk.Revit.DB.Transaction(doc, "make reporting");
        tran.Start();
        doc.FamilyManager.MakeReporting(familyParameter);
        tran.Commit();
    }

    /// <summary>
    /// Set the reporting family parameter as a regular/driving parameter.
    /// </summary>
    /// <param name="doc">document</param>
    /// <param name="familyParameter"></param>
    public static void MakeNonReporting(Autodesk.Revit.DB.Document doc,
        Autodesk.Revit.DB.FamilyParameter familyParameter)
    {
        if (!doc.IsFamilyDocument) throw new ArgumentException("just support family document");
        using Autodesk.Revit.DB.Transaction tran = new Autodesk.Revit.DB.Transaction(doc, "make none reporting");
        tran.Start();
        doc.FamilyManager.MakeNonReporting(familyParameter);
        tran.Commit();
    }

    /// <summary>
    /// Set the family parameter as a type parameter.
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="familyParameter"></param>
    public static void MakeType(Autodesk.Revit.DB.Document doc, Autodesk.Revit.DB.FamilyParameter familyParameter)
    {
        if (!doc.IsFamilyDocument) throw new ArgumentException("just support family document");
        using Autodesk.Revit.DB.Transaction tran = new Autodesk.Revit.DB.Transaction(doc, "make reporting");
        tran.Start();
        doc.FamilyManager.MakeType(familyParameter);
        tran.Commit();
    }

    /// <summary>
    /// Gets the identifier of the unit quantifying the parameter value.
    /// <remarks>
    /// Old Function Name: DisplayUnitType lower Revit 2022
    /// </remarks>
    /// </summary>
    /// <param name="familyParameter">family parameter</param>
    /// <returns name="forgeTypeId">forgeTypeId</returns>
    public static dynamic GetUnitTypeId(Autodesk.Revit.DB.FamilyParameter familyParameter)
    {
#if R20 || R21
        dynamic? displayUnitType = familyParameter.DisplayUnitType;
            
#else
        dynamic? displayUnitType = familyParameter.GetUnitTypeId();
#endif
        return displayUnitType;
    }

    /// <summary>
    /// The definition.
    /// </summary>
    /// <param name="familyParameter">family parameter</param>
    /// <returns name="definition">Definition</returns>
    public static Autodesk.Revit.DB.Definition Definition(Autodesk.Revit.DB.FamilyParameter familyParameter)
    {
        if (familyParameter == null) throw new ArgumentNullException(nameof(familyParameter));
        return familyParameter.Definition;
    }

    /// <summary>
    /// return all information properties of family parameter
    /// </summary>
    /// <param name="familyParameter">family parameter</param>
    /// <returns></returns>
    [MultiReturn("definition", "formula", "id", "associatedParameters", "displayUnitType", "isInstance", "isShared",
        "storageType", "userModifiable",
        "canAssignFormula", "isReadOnly", "guid", "isDeterminedByFormula", "isReporting")]
    public static IDictionary? GetProperties(Autodesk.Revit.DB.FamilyParameter? familyParameter)
    {
        Definition? definition = familyParameter?.Definition;
        string? formula = familyParameter?.Formula;
        int? id = familyParameter?.Id.IntegerValue;
        ParameterSet? parameterSet = familyParameter?.AssociatedParameters;
        List<global::Revit.Elements.Parameter?> associatedParameters = new List<global::Revit.Elements.Parameter?>();
        bool? isInstance = familyParameter?.IsInstance;
        bool? isShared = familyParameter?.IsShared;
        StorageType? storageType = familyParameter?.StorageType;
        bool? userModifiable = familyParameter?.UserModifiable;
        bool? canAssignFormula = familyParameter?.CanAssignFormula;
        bool? isReadOnly = familyParameter?.IsReadOnly;
#if R20 || R21
        object? displayUnitType = familyParameter?.DisplayUnitType;
#else
            object? displayUnitType = familyParameter?.GetUnitTypeId();
#endif
        bool? isReporting = familyParameter?.IsReporting;
        Guid? guid = null;
        if (familyParameter!.IsShared)
        {
            guid = familyParameter.GUID;
        }

        bool isDeterminedByFormula = familyParameter.IsDeterminedByFormula;
        foreach (Autodesk.Revit.DB.Parameter p in parameterSet!)
        {
            global::Revit.Elements.Parameter? dynamoType = p.ToDynamoType();
            associatedParameters.Add(dynamoType);
        }

        return new Dictionary<string, object?>()
        {
            {nameof(definition), definition},
            {nameof(formula), formula},
            {nameof(id), id},
            {nameof(associatedParameters), associatedParameters},
            {nameof(displayUnitType), displayUnitType},
            {nameof(isInstance), isInstance},
            {nameof(isShared), isShared},
            {nameof(storageType), storageType},
            {nameof(userModifiable), userModifiable},
            {nameof(canAssignFormula), canAssignFormula},
            {nameof(isReadOnly), isReadOnly},
            {nameof(guid), guid},
            {nameof(isDeterminedByFormula), isDeterminedByFormula},
            {nameof(isReporting), isReporting},
        };
    }

#if R20
#else
 /// <summary>
    /// Convert Revit FamilyParameter to Dynamo FamilyParameter
    /// </summary>
    /// <param name="familyParameter">family parameter</param>
    /// <returns name="Revit.Elements.FamilyParameter">family parameter</returns>
    public static global::Revit.Elements.FamilyParameter? ToDynamoFamilyParameter(
        Autodesk.Revit.DB.FamilyParameter familyParameter)
    {
        return familyParameter.ToDynamoType();
    }

    /// <summary>
    /// Convert Dynamo FamilyParameter to Revit FamilyParameter
    /// </summary>
    /// <param name="familyParameter">family parameter</param>
    /// <returns name="Autodesk.Revit.DB.FamilyParameter">family parameter</returns>
    public static Autodesk.Revit.DB.FamilyParameter? ToRevitFamilyParameter(
        global::Revit.Elements.FamilyParameter familyParameter)
    {
        return familyParameter.ToRevitType();
    }
#endif

   
}