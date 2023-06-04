using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OpenMEP.Helpers;
using RevitServices.Persistence;

namespace OpenMEP.Application;

public class Database
{
    private Database()
    {
    }

    /// <summary>
    /// Snoop Explore Elements
    /// </summary>
    /// <param name="elements">elements need to snoop</param>
    /// <returns name="string">result</returns>
    /// <example> 
    /// ![](../OpenMEPPage/application/dyn/pic/Database.SnoopElements.png)
    /// [Database.SnoopElements.dyn](../OpenMEPPage/application/dyn/Database.SnoopElements.dyn)
    /// </example>
    public static string SnoopElements(List<global::Revit.Elements.Element> elements)
    {
        ICollection<ElementId> elementIds = elements.Select(x => x.InternalElement.Id).ToList();
        DocumentManager.Instance.CurrentUIDocument.Selection.SetElementIds(elementIds);
        RevitCommandId lookupCommandId = RevitCommandId.LookupCommandId("CustomCtrl_%CustomCtrl_%Add-Ins%Explorer%RevitDBExplorer.Command");
        if(lookupCommandId == null)
        {
            TaskDialog.Show("Error", "Please install RevitDBExplorer");
            Process.Start("https://github.com/NeVeSpl/RevitDBExplorer");
            return "Please install RevitDBExplorer";
        }
        // execute the command
        DocumentManager.Instance.CurrentUIApplication.PostCommand(lookupCommandId);
        return "Success";
    }

    /// <summary>
    /// Snoop Explore Elements By Id
    /// </summary>
    /// <param name="id">id of element</param>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/application/dyn/pic/Database.SnoopElementById.png)
    /// [Database.SnoopElementById.dyn](../OpenMEPPage/application/dyn/Database.SnoopElementById.dyn)
    /// </example>
    public static string SnoopElementById(List<string> id)
    {
        Autodesk.Revit.DB.Document document = DocumentManager.Instance.CurrentDBDocument;
        List<global::Revit.Elements.Element> elements = new List<global::Revit.Elements.Element>();
        foreach (string s in id)
        {
            ElementId elementId = new ElementId(int.Parse(s));
            global::Revit.Elements.Element element = document.GetElement(elementId).ToDynamoType();
            elements.Add(element);
        }
        return SnoopElements(elements);
    }
}