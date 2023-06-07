using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using Revit.Elements;
using RevitServices.Persistence;

namespace OpenMEP.Element;

public class View
{
    private View()
    {
    }

    /// <summary>
    /// Get All View Filters Created In Current Document
    /// </summary>
    /// <param name="flag">toggle true false to fresh data</param>
    /// <returns name="view filters"></returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/View.GetAllViewFilters.png)
    /// [View.GetAllViewFilters.dyn](../OpenMEPPage/element/dyn/View.GetAllViewFilters.dyn)
    /// </example>
    [NodeCategory("Action")]
    public static List<Revit.Elements.Element> GetAllViewFilters(bool flag)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        List<ParameterFilterElement> parameterFilterElements = new FilteredElementCollector(doc)
            .OfClass(typeof(ParameterFilterElement))
            .OfType<ParameterFilterElement>()
            .OrderBy(x => x.Name)
            .ToList();
        if (parameterFilterElements.Count == 0)
            return new List<Revit.Elements.Element>();
        return parameterFilterElements.Select(x => x.ToDSType(true)).ToList();
    }

    /// <summary>
    /// Return All Element Inside View Filter
    /// </summary>
    /// <param name="viewFilter"></param>
    /// <returns name="elements">elements get from view filter</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/View.GetAllElementByViewFilter.png)
    /// [View.GetAllElementByViewFilter.dyn](../OpenMEPPage/element/dyn/View.GetAllElementByViewFilter.dyn)
    /// </example>
    [NodeCategory("Action")]
    [NodeSearchTags("get element")]
    public static List<Revit.Elements.Element> GetAllElementByViewFilter(Revit.Elements.Element viewFilter)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        ParameterFilterElement? parameterFilterElement = viewFilter.InternalElement as ParameterFilterElement;
        if (parameterFilterElement == null)
            return new List<Revit.Elements.Element>();
        ElementFilter elementFilter = parameterFilterElement.GetElementFilter();
        ICollection<ElementId> cates = parameterFilterElement.GetCategories();
        IList<ElementFilter> eleFilters = new List<ElementFilter>();
        foreach (var cat in cates)
        {
            eleFilters.Add(new ElementCategoryFilter(cat));
        }
        var cateFilter = new LogicalOrFilter(eleFilters);
        if (elementFilter != null)
        {
            return new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .WherePasses(cateFilter)
                .WherePasses(elementFilter)
                .ToElements().Select(x => x.ToDSType(true)).ToList();
        }
        return new FilteredElementCollector(doc)
            .WhereElementIsNotElementType()
            .WhereElementIsViewIndependent()
            .WherePasses(cateFilter)
            .ToElements().Select(x => x.ToDSType(true)).ToList();
    }
}