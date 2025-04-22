using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using RevitServices.Persistence;

namespace OpenMEPRevit.Element.Family;

/// <summary>The family manager object to manage the family types and parameters in family document.</summary>
public class FamilyManager
{
    private FamilyManager()
    {
    }

    /// <summary>
    /// Set the string value of a family parameter of the current family type.
    /// </summary>
    /// <param name="familyManager">family manager</param>
    /// <param name="familyParameter">Autodesk.Revit.DB.FamilyParameter</param>
    /// <param name="value">The new value for family parameter.</param>
    /// <returns name="current type">Autodesk.Revit.DB.FamilyType</returns>
    public static void Set(Autodesk.Revit.DB.FamilyManager familyManager,
        Autodesk.Revit.DB.FamilyParameter familyParameter, object value)
    {
        switch (value)
        {
            case string s:
                familyManager.Set(familyParameter, s);
                break;
            case int number:
                familyManager.Set(familyParameter, number);
                break;
            case double number:
                familyManager.Set(familyParameter, number);
                break;
            case ElementId elementId:
                familyManager.Set(familyParameter, elementId);
                break;
        }
    }

    /// <summary>
    /// The current family type.
    /// </summary>
    /// <param name="familyManager">family manager</param>
    /// <returns name="current type">Autodesk.Revit.DB.FamilyType</returns>
    public static Autodesk.Revit.DB.FamilyType CurrentType(Autodesk.Revit.DB.FamilyManager familyManager)
    {
        return familyManager.CurrentType;
    }

    /// <summary>
    /// check family type exist or not by family type name
    /// </summary>
    /// <param name="familyManager">family manager</param>
    /// <param name="typeName">family type name</param>
    /// <returns name="bool">result</returns>
    [NodeCategory("Query")]
    public static bool TypeExits(Autodesk.Revit.DB.FamilyManager familyManager, string typeName)
    {
        FamilyTypeSet familyTypeCurrent = familyManager.Types;
        foreach (Autodesk.Revit.DB.FamilyType type in familyTypeCurrent)
        {
            if (type.Name == typeName) return true;
        }

        return false;
    }

    /// <summary>
    /// check family type exist or not by family type
    /// </summary>
    /// <param name="familyManager">family manager</param>
    /// <param name="familyType">family type</param>
    /// <returns name="bool">result</returns>
    [NodeCategory("Query")]
    public static bool TypeExits(Autodesk.Revit.DB.FamilyManager familyManager, Autodesk.Revit.DB.FamilyType familyType)
    {
        FamilyTypeSet familyTypeCurrent = familyManager.Types;
        foreach (Autodesk.Revit.DB.FamilyType type in familyTypeCurrent)
        {
            if (type.Name == familyType.Name) return true;
        }

        return false;
    }

    /// <summary>
    /// Add a new family type with a given name and makes it be the current type.
    /// </summary>
    /// <param name="doc">family document</param>
    /// <param name="familyManager">family manager</param>
    /// <param name="typeName">type name</param>
    /// <returns name="new family type">Autodesk.Revit.DB.FamilyType</returns>
    [NodeCategory("Create")]
    public static Autodesk.Revit.DB.FamilyType NewType([DefaultArgument("null")] Autodesk.Revit.DB.Document doc,
        Autodesk.Revit.DB.FamilyManager familyManager, string typeName)
    {
        Autodesk.Revit.DB.Document newDoc = doc ?? DocumentManager.Instance.CurrentDBDocument;
        using (Autodesk.Revit.DB.Transaction tran = new Autodesk.Revit.DB.Transaction(newDoc, "NewType"))
        {
            tran.Start();
            Autodesk.Revit.DB.FamilyType familyType = familyManager.NewType(typeName);
            tran.Commit();
            return familyType;
        }
    }

    /// <summary>
    /// All family parameters in this family.
    /// </summary>
    /// <param name="familyManager">family manager</param>
    /// <returns name="family parameters">family parameters</returns>
    public static IEnumerable<Autodesk.Revit.DB.FamilyParameter> GetParameters(
        Autodesk.Revit.DB.FamilyManager familyManager)
    {
        FamilyParameterSet familyManagerParameters = familyManager.Parameters;
        foreach (Autodesk.Revit.DB.FamilyParameter parameter in familyManagerParameters)
        {
            yield return parameter;
        }
    }

    /// <summary>
    /// all family types in the family.
    /// </summary>
    /// <param name="familyManager">family manager</param>
    /// <returns name="family types">family types</returns>
    public static IEnumerable<Autodesk.Revit.DB.FamilyType> Types(Autodesk.Revit.DB.FamilyManager familyManager)
    {
        FamilyTypeSet familyManagerParameters = familyManager.Types;
        foreach (Autodesk.Revit.DB.FamilyType familyType in familyManagerParameters)
        {
            yield return familyType;
        }
    }

    /// <summary>
    /// return family parameter from name of parameter
    /// </summary>
    /// <param name="familyManager"></param>
    /// <param name="parametertName"></param>
    /// <returns name="family parameter"></returns>
    public static Autodesk.Revit.DB.FamilyParameter? GetParameter(Autodesk.Revit.DB.FamilyManager familyManager,
        string parametertName)
    {
        Autodesk.Revit.DB.FamilyParameter familyParameter = familyManager.get_Parameter(parametertName);
        return familyParameter;
    }
}