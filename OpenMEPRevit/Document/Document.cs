using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
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
    [MultiReturn("RevitDocument", "DynamoDocument")]
    public static Dictionary<string, object?> Current(bool toggle = true)
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

    /// <summary>
    /// Get Document Title
    /// </summary>
    /// <param name="revitDocument">Autodesk.Revit.DB.Document</param>
    /// <returns name="str">Title of Document</returns>
    public static string Title(Autodesk.Revit.DB.Document revitDocument)
    {
        return revitDocument.Title;
    }

    /// <summary>
    /// Identifies if a document is a linked RVT
    /// </summary>
    /// <param name="revitDocument">Autodesk.Revit.DB.Document</param>
    /// <returns name="bool">true if document is linked RVT</returns>
    [NodeCategory("Query")]
    public static bool IsLinked(Autodesk.Revit.DB.Document revitDocument)
    {
        return revitDocument.IsLinked;
    }

    /// <summary>
    /// Provides access to display unit type with in the document.
    /// </summary>
    /// <param name="revitDocument">Autodesk.Revit.DB.Document</param>
    /// <returns name="DisplayUnitSystem">Return the display unit type, metric or imperial.</returns>
    public static string DisplayUnitSystem(Autodesk.Revit.DB.Document revitDocument)
    {
        return revitDocument.DisplayUnitSystem.ToString();
    }
    /// <summary>
    /// Identifies if the current document is a family document.
    /// </summary>
    /// <param name="revitDocument">Autodesk.Revit.DB.Document</param>
    /// <returns name="bool">true if document is family document</returns>
    [NodeCategory("Query")]
    public static bool IsFamilyDocument(Autodesk.Revit.DB.Document revitDocument)
    {
        return revitDocument.IsFamilyDocument;
    }
    /// <summary>
    /// Identifies if a work-shared document is detached.
    /// Also, see <see cref="P:Autodesk.Revit.DB.Document.IsWorkshared" /></summary>
    /// <remarks>
    ///    Note that <see cref="P:Autodesk.Revit.DB.Document.Title" /> and <see cref="P:Autodesk.Revit.DB.Document.PathName" /> will be empty strings if a document is detached.
    /// </remarks>
    /// <para name="revitDocument">Autodesk.Revit.DB.Document</para>
    /// <returns name="bool">true if work-shared document is detached</returns>
    /// <since>2015</since>
    [NodeCategory("Query")]
    public static bool IsDetached(Autodesk.Revit.DB.Document revitDocument)
    {
        return revitDocument.IsDetached;
    }

    /// <summary>Gets ForgeDM folder id where the model locates.</summary>
    /// <remarks>
    ///    It is empty for non-cloud model;
    ///    It is cached in Revit model opened session after getting it if forceRefresh is false;
    ///    ForgeDM folder id can be changed during Revit model opened session, set forceRefresh as 'true' to get new value.
    /// </remarks>
    /// <param name="revitDocument">Autodesk.Revit.DB.Document</param>
    /// <param name="forceRefresh">
    ///    Cached value will be refreshed by sending a service call when forceRefresh is true.
    /// </param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.RevitServerUnauthorizedException">
    ///    Thrown when cannot get data from ForgeDM for Revit cloud model.
    /// </exception>
    /// <since>2022</since>
    public static string GetCloudFolderId(Autodesk.Revit.DB.Document revitDocument,bool forceRefresh =false)
    {
        return revitDocument.GetCloudFolderId(forceRefresh);
    }
    
    /// <summary>Gets the cloud model path of the cloud model.</summary>
    /// <returns>The cloud model path</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    ///    This Document is a not cloud model, cannot execute this operation.
    /// </exception>
    /// <para name="CentralServerPath">The path to the location of the central Revit server or cloud.</para>
    /// <para name="CloudPath">Whether this path represents a path on an Autodesk server such as BIM360.</para>
    /// <para name="Region">The region of the BIM 360 Docs or Autodesk Docs account and project which contains this model.</para>
    /// <para name="ServerPath">Whether this path is a server path (as opposed to a file path or cloud path).</para>
    /// <para name="ModelGUID">A GUID identifying the Revit cloud model.</para>
    /// <para name="ProjectGUID">A GUID identifying the BIM 360 Docs or Autodesk Docs project to which the model is associated.</para>
    /// <since>2019.1</since>
    [MultiReturn("CentralServerPath", "CloudPath", "Region", "ServerPath", "ModelGUID", "ProjectGUID")]
    public static Dictionary<string,object> GetCloudModelPath(Autodesk.Revit.DB.Document revitDocument)
    {
        ModelPath? modelPath = revitDocument.GetCloudModelPath();
        if (modelPath == null)
        {
            return new Dictionary<string, object>()
            {
                {"CentralServerPath", ""},
                {"CloudPath",""},
                {"Region",""},
                {"ServerPath",""},
                {"ModelGUID",""},
                {"ProjectGUID",""}
            };
        }
        return new Dictionary<string, object>()
        {
            {"CentralServerPath", modelPath.CentralServerPath},
            {"CloudPath",modelPath.CloudPath},
            {"Region",modelPath.Region},
            {"ServerPath",modelPath.ServerPath},
            {"ModelGUID",modelPath.GetModelGUID()},
            {"ProjectGUID",modelPath.GetProjectGUID()}
        };
    }
    /// <summary>A ForgeDM Urn identifying the model.</summary>
    /// <param name="revitDocument">Autodesk.Revit.DB.Document</param>
    /// <remarks>
    ///    It is empty for non-cloud model;
    ///    It is cached in Revit model opened session after getting it;
    /// </remarks>
    /// <exception cref="T:Autodesk.Revit.Exceptions.RevitServerUnauthorizedException">
    ///    Thrown when cannot get data from ForgeDM.
    /// </exception>
    /// <since>2022</since>
    public static string GetCloudModelUrn(Autodesk.Revit.DB.Document revitDocument)
    {
        return revitDocument.GetCloudModelUrn();
    }
    /// <summary>Gets ForgeDM project id where the model locates.</summary>
    /// <remarks>
    ///    It is empty for non-cloud model;
    ///    It is cached in Revit model opened session after getting it;
    /// </remarks>
    /// <exception cref="T:Autodesk.Revit.Exceptions.RevitServerUnauthorizedException">
        return revitDocument.GetProjectId();
    }
    /// <summary>Gets ForgeDM hub id where the model locates. It is cached in session.</summary>
    /// <remarks>
    ///    It is empty for non-cloud model;
    ///    It is cached in Revit model opened session after getting it;
    /// </remarks>
    /// <param name="revitDocument">Autodesk.Revit.DB.Document</param>
    /// <returns name="hubId">Id hub locates the model</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.RevitServerUnauthorizedException">
    ///    Thrown when cannot get data from ForgeDM for Revit cloud model.
    /// </exception>
    /// <since>2022</since>
    public static string GetHubId(Autodesk.Revit.DB.Document revitDocument)
    {
        return revitDocument.GetHubId();
    }
    /// <summary>
    /// Convert Revit Document to Dynamo Document
    /// </summary>
    /// <param name="revitDocument">Autodesk.Revit.DB.Document</param>
    /// <returns name="Document">Revit.Application.Document</returns>
    public static Revit.Application.Document? ToDynamoDocument(Autodesk.Revit.DB.Document? revitDocument)
    {
        if (revitDocument == null) return null;
        return revitDocument.ToDynamoType();
    }
    
    /// <summary>
    /// Convert Dynamo Document to Revit Document
    /// </summary>
    /// <param name="dynamoDocument">Revit.Application.Document</param>
    /// <returns name="revitDocument">Autodesk.Revit.DB.Document</returns>
    public static Autodesk.Revit.DB.Document? ToRevitDocument(Revit.Application.Document? dynamoDocument)
    {
        if (dynamoDocument == null) return null;
        return dynamoDocument.ToRevitType();
    }
    /// <summary>
    /// Get Application From Revit Document
    /// </summary>
    /// <param name="revitDocument">Autodesk.Revit.DB.Document</param>
    /// <returns name="Application">Autodesk.Revit.ApplicationServices.Application</returns>
    public static Autodesk.Revit.ApplicationServices.Application Application(
        Autodesk.Revit.DB.Document revitDocument)
    {
        return revitDocument.Application;
    }
}