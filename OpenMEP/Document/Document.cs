using System.Collections;
using Autodesk.DesignScript.Runtime;
using OpenMEP.Helpers;
using RevitServices.Persistence;

namespace OpenMEP.Document;

public class Document
{
    private Document()
    {

    }
    /// <summary>
    /// Get Document Current
    /// </summary>
    /// <param name="toggle">toggle true to get current document</param>
    /// <returns name="RevitDocument">Document of Revit Project</returns>
    /// <returns name="DynamoDocument">Document of Dynamo Wrap</returns>
    /// <example>
    /// ![](../OpenMEPPage/document/dyn/pic/Document.Current.png)
    /// </example>
    [MultiReturn("RevitDocument","DynamoDocument")]
    public static Dictionary<string,object?> Current(bool toggle = true)
    {
        Autodesk.Revit.DB.Document? RevitDocument = null;
        Revit.Application.Document? DynamoDocument = null;
        if (toggle)
        {
            RevitDocument = DocumentManager.Instance.CurrentDBDocument;
            DynamoDocument = RevitDocument.ToDynamoType();
        }

        return new Dictionary<string, object?>()
        {
            {nameof(RevitDocument), RevitDocument},
            {nameof(DynamoDocument), DynamoDocument}
        };
    }
}