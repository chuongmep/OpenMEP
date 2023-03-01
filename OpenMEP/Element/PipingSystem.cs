using Autodesk.Revit.DB.Plumbing;
using OpenMEP.Helpers;
using RevitServices.Persistence;

namespace OpenMEP.Element;

public class PipingSystem
{
    private PipingSystem()
    {
    }

    /// <summary>
    /// flag true to return all piping systems
    /// </summary>
    /// <param name="toggle">flag true or false to fresh</param>
    /// <returns name="pipeingSystemTypes">pipePingSystemTypes</returns>
    public static IEnumerable<Revit.Elements.Element?> GetAllPipeSystemTypes(bool toggle)
    {
        // filter for all piping systems
        Autodesk.Revit.DB.FilteredElementCollector collector = new Autodesk.Revit.DB.FilteredElementCollector(DocumentManager.Instance.CurrentDBDocument);
        Autodesk.Revit.DB.ElementClassFilter filter = new Autodesk.Revit.DB.ElementClassFilter(typeof(Autodesk.Revit.DB.Plumbing.PipingSystemType));
        Autodesk.Revit.DB.FilteredElementIterator iterator = collector.WherePasses(filter).GetElementIterator();
        iterator.Reset();
        while (iterator.MoveNext())
        {
            Autodesk.Revit.DB.Element element = iterator.Current!;
            if (element is PipingSystemType pipingSystemType)
            {
                
                yield return pipingSystemType.ToDynamoType();
            }
        }
    }
    
}