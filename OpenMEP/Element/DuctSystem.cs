using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using OpenMEP.Helpers;
using RevitServices.Persistence;

namespace OpenMEP.Element;

public class DuctSystem
{
    private DuctSystem()
    {
    }

    /// <summary>
    /// flag true to return all ducting systems
    /// </summary>
    /// <param name="toggle">flag true or false to fresh</param>
    /// <returns name="ductSystemTypes">pipePingSystemTypes</returns>
    public static IEnumerable<Revit.Elements.Element?> GetAllDuctSystemTypes(bool toggle)
    {
        // filter for all piping systems
        Autodesk.Revit.DB.FilteredElementCollector collector = new Autodesk.Revit.DB.FilteredElementCollector(DocumentManager.Instance.CurrentDBDocument);
        Autodesk.Revit.DB.ElementClassFilter filter = new Autodesk.Revit.DB.ElementClassFilter(typeof(Autodesk.Revit.DB.Mechanical.MechanicalSystemType));
        Autodesk.Revit.DB.FilteredElementIterator iterator = collector.WherePasses(filter).GetElementIterator();
        iterator.Reset();
        while (iterator.MoveNext())
        {
            Autodesk.Revit.DB.Element element = iterator.Current!;
            if (element is MechanicalSystemType pipingSystemType)
            {
                yield return pipingSystemType.ToDynamoType();
            }
        }
    }
    
    /// <summary>
    /// return duct system type by name
    /// </summary>
    /// <param name="typeName">name of pipe system type</param>
    /// <returns name="ductSystemType">the element system type</returns>
    public static Revit.Elements.Element? GetDuctSystemTypeByName(string typeName)
    {
        return GetAllDuctSystemTypes(true).FirstOrDefault(x => x!.Name == typeName);
    }

}