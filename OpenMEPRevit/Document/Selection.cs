using System.Text;
using System.Windows;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Dynamo.Graph.Nodes;
using OpenMEPRevit.Helpers;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using Point = Autodesk.DesignScript.Geometry.Point;

namespace OpenMEPRevit.Document;

public class Selection
{
    private Selection()
    {
    }

    /// <summary>
    /// Returns the selected elements in the current document.
    /// </summary>
    /// <param name="flag">flag true false to fresh</param>
    /// <returns name="elements">element selected</returns>
    /// <example>
    /// ![](../OpenMEPPage/document/dyn/pic/Selection.GetSelectedElements.png)
    /// [Selection.GetSelectedElements.dyn](../OpenMEPPage/document/dyn/Selection.GetSelectedElements.dyn)
    /// </example>
    [NodeCategory("Action")]
    [NodeSearchTags("selection", "get", "selected", "elements")]
    public static List<Revit.Elements.Element> GetSelectedElements(bool flag)
    {
        UIDocument uidoc = DocumentManager.Instance.CurrentUIDocument;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.UI.Selection.Selection selection = uidoc.Selection;
        List<Revit.Elements.Element> elements = new List<Revit.Elements.Element>();
        foreach (Autodesk.Revit.DB.ElementId elementId in selection.GetElementIds())
        {
            elements.Add(doc.GetElement(elementId).ToDynamoType());
        }

        return elements;
    }


    /// <summary>
    /// Return Point Picked In Current View
    /// </summary>
    /// <param name="flag">toggle to fresh pick point</param>
    /// <returns name="point">point</returns>
    /// <example>
    /// ![](../OpenMEPPage/document/dyn/pic/Selection.PickPoint.gif)
    /// [Selection.PickPoint.dyn](../OpenMEPPage/document/dyn/Selection.PickPoint.dyn)
    /// </example>
    [NodeCategory("Action")]
    [NodeSearchTags("selection", "pick", "point")]
    public static Autodesk.DesignScript.Geometry.Point PickPoint(bool flag)

    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        UIDocument uidoc = DocumentManager.Instance.CurrentUIApplication.ActiveUIDocument;
        using (Transaction t = new Transaction(doc))
        {
            View activeView = doc.ActiveView;
            t.Start("Pick Point");
            Autodesk.Revit.DB.SketchPlane sp = Autodesk.Revit.DB.SketchPlane.Create(doc,
                Plane.CreateByNormalAndOrigin(
                    activeView.ViewDirection,
                    activeView.Origin));
            activeView.SketchPlane = sp;
            var pickPoint = uidoc.Selection.PickPoint();
            //clear Sketch Plane
            doc.Delete(sp.Id);
            t.RollBack();
            return pickPoint.ToPoint();
        }
    }

    /// <summary>
    /// Return a list points pick order in Current View
    /// </summary>
    /// <param name="flag">toggle true false to fresh pick point</param>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/document/dyn/pic/Selection.PickPointOrders.gif)
    /// [Selection.PickPointOrders.dyn](../OpenMEPPage/document/dyn/Selection.PickPointOrders.dyn)
    /// </example>
    [NodeCategory("Action")]
    [NodeSearchTags("selection", "pick", "point", "order")]
    public static List<Point> PickPointOrders(bool flag)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        UIDocument uidoc = DocumentManager.Instance.CurrentUIApplication.ActiveUIDocument;
        List<Point> Points = new List<Point>();
        TaskDialog.Show("OpenMEP", "Select Ordered By Pick Point,Press Esc to Cancel", TaskDialogCommonButtons.Ok);
        try
        {
            while (true)
            {
                using (Transaction t = new Transaction(doc))
                {
                    View activeView = doc.ActiveView;
                    t.Start("Transaction");
                    SketchPlane sp = SketchPlane.Create(doc,
                        Plane.CreateByNormalAndOrigin(
                            activeView.ViewDirection,
                            activeView.Origin));
                    activeView.SketchPlane = sp;
                    var pickPoint = uidoc.Selection.PickPoint();
                    //clear Sketch Plane
                    doc.Delete(sp.Id);
                    t.RollBack();
                    Points.Add(pickPoint.ToPoint());
                }
            }
        }
        catch (Autodesk.Revit.Exceptions.OperationCanceledException)
        {
            return Points;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
            return Points;
        }
    }

    /// <summary>
    /// Pick Select Order Element In Current View
    /// </summary>
    /// <param name="flag"></param>
    /// <returns name="elements">list element pick ordered</returns>
    /// <example>
    /// ![](../OpenMEPPage/document/dyn/pic/Selection.PickOrderElements.gif)
    /// [Selection.PickOrderElements.dyn](../OpenMEPPage/document/dyn/Selection.PickOrderElements.dyn)
    /// </example>
    [NodeCategory("Action")]
    [NodeSearchTags("selection", "pick", "order", "element")]
    public static List<Revit.Elements.Element> PickOrderElements(bool flag)
    {
        List<Revit.Elements.Element> elements = new List<Revit.Elements.Element>();
        try
        {
            TaskDialog.Show("OpenMEP", "Select Ordered By Pick Element,Press Esc to Cancel",
                TaskDialogCommonButtons.Ok);
            while (true)
            {
                Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
                UIDocument uidoc = DocumentManager.Instance.CurrentUIApplication.ActiveUIDocument;
                Autodesk.Revit.UI.Selection.Selection selection = uidoc.Selection;
                Autodesk.Revit.DB.ElementId elementId =
                    selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element).ElementId;
                Autodesk.Revit.DB.Element element1 = doc.GetElement(elementId);
                Revit.Elements.Element? element = element1.ToDynamoType();
                elements.Add(element);
            }
        }
        catch (Exception e)
        {
            return elements;
        }
    }


    /// <summary>
    /// Pick Element By Rectangle In Current View
    /// </summary>
    /// <param name="flag">toggle true false to pick again</param>
    /// <returns name="elements">list element pick by Rectangle</returns>
    /// <example>
    /// ![](../OpenMEPPage/document/dyn/pic/Selection.PickElementsByRectangle.gif)
    /// [Selection.PickElementsByRectangle.dyn](../OpenMEPPage/document/dyn/Selection.PickElementsByRectangle.dyn)
    /// </example>
    [NodeCategory("Action")]
    [NodeSearchTags("selection", "pick", "rectangle", "element")]
    public static List<Revit.Elements.Element> PickElementsByRectangle(bool flag)
    {
        List<Revit.Elements.Element> elements = new List<Revit.Elements.Element>();
        UIDocument uidoc = DocumentManager.Instance.CurrentUIDocument;
        try
        {
            TaskDialog.Show("OpenMEP", "Select Element By Drag Mouse Rectangle,Press Esc to Cancel",
                TaskDialogCommonButtons.Ok);
            while (true)
            {
                IList<Autodesk.Revit.DB.Element> elementsByRectangle = uidoc.Selection.PickElementsByRectangle(
                    new SelectionFilter(),
                    "Select Element By Drag Mouse Rectangle");
                elementsByRectangle.ToList().ForEach(x => elements.Add(x.ToDynamoType()));
            }
        }
        catch (Autodesk.Revit.Exceptions.OperationCanceledException e)
        {
            return elements;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
            return elements;
        }
    }

    /// <summary>
    /// Pick Element In Linked Document
    /// </summary>
    /// <param name="flag">flag true false to fresh pick element</param>
    /// <returns name="elements">list element inside link elements</returns>
    /// <example>
    /// ![](../OpenMEPPage/document/dyn/pic/Selection.PickLinkElements.gif)
    /// [Selection.PickLinkElements.dyn](../OpenMEPPage/document/dyn/Selection.PickLinkElements.dyn)
    /// </example>
    [NodeCategory("Action")]
    [NodeSearchTags("selection", "pick", "link", "element")]
    public static List<Revit.Elements.Element> PickLinkElements(bool flag)
    {
        List<Revit.Elements.Element> elements = new List<Revit.Elements.Element>();
        UIDocument uidoc = DocumentManager.Instance.CurrentUIDocument;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("1.Select Link Instance First And Select Elements");
            sb.AppendLine("2.Select Elements In Linked");
            sb.AppendLine("3.Click Finish to end selection");
            TaskDialog.Show("OpenMEP", sb.ToString(),
                TaskDialogCommonButtons.Ok);
            Reference? link_ref = uidoc.Selection.PickObject(ObjectType.Element, new LinkSelectionFilter(),
                "Select a link instance first");
            if (link_ref == null) return elements;
            IList<Reference> references = uidoc.Selection.PickObjects(ObjectType.LinkedElement,
                "Select Elements In Linked");
            RevitLinkInstance? link1 = doc.GetElement(link_ref.ElementId) as RevitLinkInstance;
            if (link1 == null) return elements;
            Autodesk.Revit.DB.Document linkDoc = link1.GetLinkDocument();
            foreach (var r1 in references)
            {
                ElementId elementId = r1.LinkedElementId;
                elements.Add(linkDoc.GetElement(elementId).ToDynamoType());
            }
        }
        catch (Autodesk.Revit.Exceptions.OperationCanceledException e)
        {
            return elements;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
            return elements;
        }

        return elements;
    }

    /// <summary>
    /// Set selected element in Revit Project
    /// </summary>
    /// <param name="elements">list element need set selected</param>
    /// <returns name="elements">list selected element</returns>
    /// <example>
    /// ![](../OpenMEPPage/document/dyn/pic/Selection.SetSelectedElement.gif)
    /// [Selection.SetSelectedElement.dyn](../OpenMEPPage/document/dyn/Selection.SetSelectedElement.dyn)
    /// </example>
    [NodeCategory("Action")]
    [NodeSearchTags("selection", "pick", "link", "element")]
    public static List<Revit.Elements.Element> SetSelectedElement(List<Revit.Elements.Element> elements)
    {
        if (!elements.Any()) return new List<Revit.Elements.Element>();
        List<ElementId> elementIds = new List<ElementId>();
        elements.ForEach(x => elementIds.Add(x.InternalElement.Id));
        Autodesk.Revit.UI.Selection.Selection selection = DocumentManager.Instance.CurrentUIDocument.Selection;
        selection.SetElementIds(elementIds);
        return elements;
    }

    [IsVisibleInDynamoLibrary(false)]
    public class SelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Autodesk.Revit.DB.Element? elem)
        {
            if (elem != null) return true;
            return false;
        }

        public bool AllowReference(Reference refer, XYZ pos)
        {
            return false;
        }
    }

    [IsVisibleInDynamoLibrary(false)]
    public class LinkSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Autodesk.Revit.DB.Element? elem)
        {
            if (elem is Autodesk.Revit.DB.RevitLinkInstance) return true;
            return false;
        }

        public bool AllowReference(Reference refer, XYZ pos)
        {
            return false;
        }
    }
}