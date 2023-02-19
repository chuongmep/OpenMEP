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
    [MultiReturn("RevitDocument","DynamoDocument")]
    public static IDictionary Current(bool toggle = true)
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