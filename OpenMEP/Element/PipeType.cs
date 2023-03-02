using OpenMEP.Helpers;
using RevitServices.Persistence;

namespace OpenMEP.Element;

public class PipeType
{
    private PipeType()
    {
        
    }
    
    /// <summary>
    /// get pipe type by name
    /// </summary>
    /// <param name="typeName">type name of pipe</param>
    /// <returns></returns>
    public static Revit.Elements.Element? GetPipeTypeByName(string typeName)
    {
        return GetAllPipeTypes().FirstOrDefault(x => x!.Name == typeName);
    }
    
    /// <summary>
    ///  return all pipe types of the current document
    /// </summary>
    /// <returns name="pipeTypes">All Pipe Types</returns>
    public static List<Revit.Elements.Element?> GetAllPipeTypes()
    {
        // Get all the pipe types in the current document
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.DB.FilteredElementCollector collector = new Autodesk.Revit.DB.FilteredElementCollector(doc);
        Autodesk.Revit.DB.ElementClassFilter filter = new Autodesk.Revit.DB.ElementClassFilter(typeof(Autodesk.Revit.DB.Plumbing.PipeType));
        Autodesk.Revit.DB.FilteredElementIterator iterator = collector.WherePasses(filter).GetElementIterator();
        iterator.Reset();
        List<Revit.Elements.Element?> pipeTypes = new List<Revit.Elements.Element?>();
        while (iterator.MoveNext())
        {
            Autodesk.Revit.DB.Element element = iterator.Current!;
            if (element is Autodesk.Revit.DB.Plumbing.PipeType pipeType)
            {
                pipeTypes.Add(pipeType.ToDynamoType());
            }
        }
        return pipeTypes;
    }

    
}