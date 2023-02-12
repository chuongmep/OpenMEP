using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using RevitServices.Persistence;

namespace Parameter;

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