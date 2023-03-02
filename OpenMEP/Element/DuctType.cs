using OpenMEP.Helpers;
using RevitServices.Persistence;

namespace OpenMEP.Element;

public class DuctType
{
    private DuctType()
    {
        
    }
    
    /// <summary>
    /// get pipe type by name
    /// </summary>
    /// <param name="typeName">type name of duct</param>
    /// <returns name="ductType">type of duct</returns>
    public static Revit.Elements.Element? GetDuctTypeByName(string typeName)
    {
        return GetAllDuctTypes().FirstOrDefault(x => x!.Name == typeName);
    }
    
    /// <summary>
    ///  return all duct types of the current document
    /// </summary>
    /// <returns name="ductTypes">All Duct Types</returns>
    public static List<Revit.Elements.Element?> GetAllDuctTypes()
    {
        // Get all the pipe types in the current document
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.DB.FilteredElementCollector collector = new Autodesk.Revit.DB.FilteredElementCollector(doc);
        Autodesk.Revit.DB.ElementClassFilter filter = new Autodesk.Revit.DB.ElementClassFilter(typeof(Autodesk.Revit.DB.Mechanical.DuctType));
        Autodesk.Revit.DB.FilteredElementIterator iterator = collector.WherePasses(filter).GetElementIterator();
        iterator.Reset();
        List<Revit.Elements.Element?> pipeTypes = new List<Revit.Elements.Element?>();
        while (iterator.MoveNext())
        {
            Autodesk.Revit.DB.Element element = iterator.Current!;
            if (element is Autodesk.Revit.DB.Mechanical.DuctType ductType)
            {
                pipeTypes.Add(ductType.ToDynamoType());
            }
        }
        return pipeTypes;
    }

    
}