using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using RevitServices.Persistence;

namespace OpenMEP.Parameter;
/// <summary>An Element that stores a user-defined parameter.</summary>
/// <remarks>
///    Revit supports both built-in and user-defined parameters.  Built-in parameters
///    ship with the application, and they are not stored in Revit documents.
///    User-defined parameters are dynamically created, and they are stored in the
///    documents that use them, wrapped in ParameterElement objects.  Different
///    subclasses of ParemeterElement represent different kinds of user-defined
///    parameters.
/// </remarks>
/// <since>2016</since>
public class ProjectParameter
{
    private ProjectParameter()
    {
        
    }
    
    [MultiReturn("ParameterNames","Parameters")]
    public static Dictionary<string,object?> All([DefaultArgument("null")]Autodesk.Revit.DB.Document doc)
    {
        Autodesk.Revit.DB.Document document = doc ?? DocumentManager.Instance.CurrentDBDocument;
        // Get all the project parameters
        FilteredElementCollector collector = new FilteredElementCollector(document);
        collector.OfClass(typeof(ParameterElement));
        List<ParameterElement> parameters = collector.ToElements().Cast<ParameterElement>().ToList();
        // Get the parameter names
        List<string> parameterNames = new List<string>();
        foreach (ParameterElement parameter in parameters)
        {
            parameterNames.Add(parameter.Name);
        }
        return new Dictionary<string,object?>()
        {
            {"ParameterNames", parameterNames},
            {"Parameters", parameters}
        };
    }
}