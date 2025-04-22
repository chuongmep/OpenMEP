using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using Revit.Elements;
using Category = Revit.Elements.Category;

namespace OpenMEPRevit.Element;

[IsVisibleInDynamoLibrary(true)]
public class ParameterFilterElement
{
    private ParameterFilterElement()
    {
    }

    public List<Category>? Categories(Revit.Elements.Element parameterElement)
    {
        List<Category> categories = new List<Category>();
        Autodesk.Revit.DB.ParameterFilterElement? parameterFilterElement = parameterElement.InternalElement as Autodesk.Revit.DB.ParameterFilterElement;
        ICollection<ElementId>? elementIds = parameterFilterElement?.GetCategories();
        if (elementIds == null)
        {
            return categories;
        }
#if R20 || R21 || R22 || R23
        foreach (ElementId elementId in elementIds)
        {
            categories.Add(Category.ById(elementId.IntegerValue));
        }
#else
        foreach (ElementId elementId in elementIds)
        {
            categories.Add(Category.ById(elementId.Value));
        }
#endif
        return categories;
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
        Autodesk.Revit.DB.Document doc = viewFilter.InternalElement.Document;
        Autodesk.Revit.DB.ParameterFilterElement? parameterFilterElement = viewFilter.InternalElement as Autodesk.Revit.DB.ParameterFilterElement;
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