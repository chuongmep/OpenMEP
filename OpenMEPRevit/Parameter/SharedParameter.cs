using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Revit.Elements;
using RevitServices.Persistence;

namespace OpenMEPRevit.Parameter;
/// <summary>
///    An element that stores the definition of a shared parameter which is loaded into the document.
/// </summary>
/// <remarks>
///    Shared parameters are user-defined parameters that can be shared by multiple
///    Revit documents. A shared parameter is identified by a GUID.
///    Basic information of the shared parameter are accessed through GetDefinition().
/// </remarks>
/// <since>2016</since>
public class SharedParameter
{
    private SharedParameter()
    {
    }
    /// <summary>
    /// return all share parameter in document
    /// </summary>
    public static IEnumerable<Revit.Elements.Element> All([DefaultArgument("null")] Autodesk.Revit.DB.Document doc)
    {
        Autodesk.Revit.DB.Document currentDbDocument = doc ?? DocumentManager.Instance.CurrentDBDocument;
        global::Revit.Elements.Element ele;
        FilteredElementCollector collector
            = new FilteredElementCollector(currentDbDocument)
                .WhereElementIsNotElementType();
        List<SharedParameterElement> shareEles =
            collector.OfClass(typeof(SharedParameterElement)).Cast<SharedParameterElement>().ToList();
        foreach (SharedParameterElement parameterElement in shareEles)
        {
            Revit.Elements.Element dsType = parameterElement.ToDSType(true);
            yield return dsType;
        }
    }


    /// <summary>
    /// return all information of 
    /// </summary>
    /// <param name="shareParameterElement"></param>
    /// <returns></returns>
    [MultiReturn("name", "id", "uniqueId", "guidValue", "versionGuid", "groupId", "pinned", "shouldHideWhenNoValue",
        "isValidObject")]
    public static Dictionary<string,object?> GetInformation(Revit.Elements.Element shareParameterElement)
    {
        SharedParameterElement? sharedParameterElement =
            shareParameterElement.InternalElement as SharedParameterElement;
        string? guidValue = sharedParameterElement?.GuidValue.ToString();
#if R20
        string? versionGuid = String.Empty;
        #else
        string? versionGuid = sharedParameterElement?.VersionGuid.ToString();
#endif
#if R20 || R21 || R22 || R23
        int? groupId = sharedParameterElement?.GroupId.IntegerValue;
        int? id = sharedParameterElement?.Id?.IntegerValue;

#else
        long? groupId = sharedParameterElement?.GroupId.Value;
        long? id = sharedParameterElement?.Id?.Value;

#endif
        string? name = sharedParameterElement?.Name;
        string? uniqueId = sharedParameterElement?.UniqueId;
        bool? pinned = sharedParameterElement?.Pinned;
        bool? shouldHideWhenNoValue = sharedParameterElement?.ShouldHideWhenNoValue();
        bool? isValidObject = sharedParameterElement?.IsValidObject;
        return new Dictionary<string, object?>()
        {
            {nameof(name), name},
            {nameof(id), id},
            {nameof(uniqueId), uniqueId},
            {nameof(guidValue), guidValue},
            {nameof(versionGuid), versionGuid},
            {nameof(groupId), groupId},
            {nameof(pinned), pinned},
            {nameof(shouldHideWhenNoValue), shouldHideWhenNoValue},
            {nameof(isValidObject), isValidObject},
        };
    }

#if R20 || R21
    // /// <summary>
    // /// Add a new parameter to share parameter file
    // /// </summary>
    // /// <param name="parameterName">parameter name</param>
    // /// <param name="groupName">group name</param>
    // /// <param name="SpecTypeId">SpecTypeId</param>
    // /// <returns></returns>
    // public static bool AddParameterShareFile(string parameterName,string groupName,Autodesk.Revit.DB.ParameterType SpecTypeId)
    // {
    //     Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
    //     using Autodesk.Revit.DB.Transaction tran = new Autodesk.Revit.DB.Transaction(doc, "test");
    //     tran.Start();
    //     // open file share parameter and add new revit api 
    //     string parametersFilename = doc.Application.SharedParametersFilename;
    //     if(string.IsNullOrEmpty(parametersFilename)) throw new ArgumentException("Please add share parameter file");
    //     // add new share parameter
    //     DefinitionFile definitionFile = doc.Application.OpenSharedParameterFile();
    //     if(definitionFile==null) throw new ArgumentException("Please add share parameter file");
    //     DefinitionGroup definitionGroup = definitionFile.Groups.Create(groupName);
    //     definitionGroup.Definitions.Create(new ExternalDefinitionCreationOptions(parameterName,SpecTypeId));
    //     // add new parameter to all elements
    //     tran.Commit();
    //     return true;
    // }
#else
        /// <summary>
        /// Add a new parameter to share parameter file
        /// </summary>
        /// <param name="parameterName">parameter name</param>
        /// <param name="groupName">group name</param>
        /// <param name="SpecTypeId">SpecTypeId</param>
        /// <returns></returns>
        public static bool AddParameterShareFile(string parameterName,string groupName,Autodesk.Revit.DB.ForgeTypeId SpecTypeId)
        {
            Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
            using Autodesk.Revit.DB.Transaction tran = new Autodesk.Revit.DB.Transaction(doc, "test");
            tran.Start();
            // open file share parameter and add new revit api 
            string parametersFilename = doc.Application.SharedParametersFilename;
            if(string.IsNullOrEmpty(parametersFilename)) throw new ArgumentException("Please add share parameter file");
            // add new share parameter
            DefinitionFile definitionFile = doc.Application.OpenSharedParameterFile();
            if(definitionFile==null) throw new ArgumentException("Please add share parameter file");
            DefinitionGroup group = definitionFile.Groups.get_Item(groupName);
            if (group == null)
            {
                group = definitionFile.Groups.Create(groupName);
            }
            Autodesk.Revit.DB.Definition definition = group.Definitions.get_Item(parameterName);
            if (definition == null)
            {
                group.Definitions.Create(new ExternalDefinitionCreationOptions(parameterName,SpecTypeId));
            }
            // add new parameter to all elements
            tran.Commit();
            return true;
        }
#endif
}