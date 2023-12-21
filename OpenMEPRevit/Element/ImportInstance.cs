using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;

namespace OpenMEPRevit.Element;

public class ImportInstance
{
    private ImportInstance()
    {
    }
    /// <summary>
    /// Retrieves information about external resources associated with a Revit element.
    /// </summary>
    /// <param name="importInstance">The Revit element for which external resource information is retrieved.</param>
    /// <returns>
    /// A dictionary containing information about external resources with keys:
    /// - "ModelIdentity": Identity information of the model.
    /// - "Path": The path of the external resource.
    /// - "PathType": The type of path for the external resource.
    /// </returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/ImportInstance.GetPath.png)
    /// </example>
    [MultiReturn("ModelIdentity","Path","PathType")]
    public static Dictionary<string, object?> GetPath(global::Revit.Elements.Element importInstance)
    {
        Autodesk.Revit.DB.Element internalElement = importInstance.InternalElement;
        Dictionary<string,object?> result = new Dictionary<string, object?>();
        if (internalElement is Autodesk.Revit.DB.ImportInstance import)
        {
            ElementId elementId = import.GetTypeId();
            ElementType? elementType = import.Document.GetElement(elementId) as Autodesk.Revit.DB.ElementType;
            IDictionary<ExternalResourceType,ExternalResourceReference> externalResourceReferences = elementType.GetExternalResourceReferences();
            foreach (KeyValuePair<ExternalResourceType, ExternalResourceReference> externalResourceReference in externalResourceReferences)
            {
                IDictionary<string,string> information = externalResourceReference.Value.GetReferenceInformation();
                foreach (KeyValuePair<string, string> keyValuePair in information)
                {
                    result.Add(keyValuePair.Key,keyValuePair.Value);
                }
            }
        }
        else
        {
            result.Add("ModelIdentity",null);
            result.Add("Path",null);
            result.Add("PathType",null);
        }
        return result;
    }

}