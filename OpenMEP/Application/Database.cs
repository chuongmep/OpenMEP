using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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
}