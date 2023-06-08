using Autodesk.DesignScript.Runtime;
using OpenMEPRevit.Helpers;
using RevitServices.Persistence;

namespace OpenMEPRevit.Document;

/// <summary>An object that represents an open Autodesk Revit project.</summary>
/// <remarks>
///    The Document object represents an Autodesk Revit project. Revit can have multiple
///    projects open and multiple views to those projects. The active or top most view will be the
///    active project and hence the active document which is available from the Application object.
/// </remarks>
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