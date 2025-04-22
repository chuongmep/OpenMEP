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
using RevitServices.Transactions;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;
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
    /// <returns name="points">list point picked orders</returns>
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
    /// Return list point pick orders on curve element (Pipe, Duct, Cable Tray, Conduit, Flex Duct, Flex Pipe, Wire)
    /// </summary>
    /// <param name="flag">toggle true false to fresh pick point</param>
    /// <returns name="points">list point orders picked</returns>
    /// <example>
    /// ![](../OpenMEPPage/document/dyn/pic/Selection.PickPointOnCurveElement.png)
    /// [Selection.PickPointOnCurveElement.dyn](../OpenMEPPage/document/dyn/Selection.PickPointOnCurveElement.dyn)
    /// </example>
    public static List<Point> PickPointOnCurveElement(bool flag)
    {
        UIDocument uiDoc = DocumentManager.Instance.CurrentUIDocument;
        Autodesk.Revit.DB.Document doc = uiDoc.Document;
        List<Point> Points = new List<Point>();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Select Point On Curve Element");
        stringBuilder.AppendLine("Press [Esc] to Cancel.");
        TaskDialog.Show("OpenMEP", stringBuilder.ToString(), TaskDialogCommonButtons.Ok);
        Transaction? tran = default;
        ViewDetailLevel detailLevel = doc.ActiveView.DetailLevel;
        try
        {
            while (true)
            {
                tran = new Transaction(doc);
                //set detail level is fine
                tran.Start("Set Detail Level");
                doc.ActiveView.DetailLevel = ViewDetailLevel.Coarse;
                tran.Commit();
                tran.Start("Pick Point On Curve Element");
                Reference r =
                    uiDoc.Selection.PickObject(ObjectType.PointOnElement, "Please pick point on Curve Element");
                XYZ rGlobalPoint = r.GlobalPoint;
                Points.Add(rGlobalPoint.ToPoint());
                tran.RollBack();
            }
        }
        catch (Autodesk.Revit.Exceptions.OperationCanceledException)
        {
            doc.ActiveView.DetailLevel = detailLevel;
            tran?.Commit();
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
    /// Retrieves a list of linked elements from the host Revit document based on the user's selection.
    /// </summary>
    /// <param name="flag">A boolean flag indicating fresh the selection process.</param>
    /// <returns>A list of Revit elements from linked documents.</returns>
    /// <exception cref="System.ArgumentException">Thrown when an error occurs during the element retrieval process.</exception>
    /// <example>
    /// ![](../OpenMEPPage/document/dyn/pic/Selection.PickOrderLinkElements.png)
    /// [Selection.PickOrderLinkElements.dyn](../OpenMEPPage/document/dyn/Selection.PickOrderLinkElements.dyn)
    /// </example>
    public static List<Revit.Elements.Element> PickOrderLinkElements(bool flag)
    {
        List<Revit.Elements.Element> elements = new List<Revit.Elements.Element>();
        UIDocument uiDoc = DocumentManager.Instance.CurrentUIApplication.ActiveUIDocument;
        Autodesk.Revit.DB.Document doc = uiDoc.Document;
        if (doc==null) return elements;
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("1.Select Ordered By Pick Link Element");
            sb.AppendLine("2.Press Esc to Cancel");
            TaskDialog.Show("OpenMEP", sb.ToString(), TaskDialogCommonButtons.Ok);
            while (true)
            {
                var reference = uiDoc.Selection.PickObject(ObjectType
                    .LinkedElement);
                var representation = reference.ConvertToStableRepresentation(doc).Split(':')[0];
                var parsedReference = Reference.ParseFromStableRepresentation(doc, representation);
                var revitLinkInstance = (RevitLinkInstance)doc.GetElement(parsedReference);
                var linkedDocument = revitLinkInstance.GetLinkDocument();
                var linkedElement = linkedDocument.GetElement(reference.LinkedElementId);
                Revit.Elements.Element? element = linkedElement.ToDynamoType();
                elements.Add(element);
            }
        }
        catch (OperationCanceledException e)
        {
            return elements;
        }
        catch (Exception e)
        {
            throw new ArgumentException(e.Message);
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

    /// <summary>
    /// Zoom to element in Revit Project
    /// </summary>
    /// <param name="elements">the list element need zoom to</param>
    /// <example>
    /// ![](../OpenMEPPage/document/dyn/pic/Selection.ZoomToElement.png)
    /// [Selection.ZoomToElement.dyn](../OpenMEPPage/document/dyn/Selection.ZoomToElement.dyn)
    /// </example>
    [NodeCategory("Action")]
    public static void ZoomToElement(List<Revit.Elements.Element> elements)
    {
        if (elements.Any(x => x.InternalElement.Document.IsLinked))
        {
            ZoomToLinkElement(elements);
            return;
        }

        if (!elements.Any()) return;
        List<ElementId> elementIds = new List<ElementId>();
        elements.ForEach(x => elementIds.Add(x.InternalElement.Id));
        Autodesk.Revit.UI.Selection.Selection selection = DocumentManager.Instance.CurrentUIDocument.Selection;
        selection.SetElementIds(elementIds);
        DocumentManager.Instance.CurrentUIDocument.ShowElements(elementIds);
    }

    /// <summary>
    /// Zooms to specified elements within a Revit project.
    /// </summary>
    /// <param name="elements">A list of Revit elements to zoom to.</param>
    /// <param name="isCropView">Specifies whether to use a crop view for zooming (optional, default is false).</param>
    /// <example>
    /// ![](../OpenMEPPage/document/dyn/pic/Selection.ZoomToLinkElement.png)
    /// [Selection.ZoomToLinkElement.dyn](../OpenMEPPage/document/dyn/Selection.ZoomToLinkElement.dyn)
    /// </example>
    public static void ZoomToLinkElement(List<Revit.Elements.Element> elements, bool isCropView = false)
    {
        if (elements == null) throw new ArgumentNullException($"{nameof(elements)} is null");
        UIDocument uidoc = DocumentManager.Instance.CurrentUIDocument;
        ElementId viewId = uidoc.ActiveView.Id;
        List<UIView> uiViews = uidoc.GetOpenUIViews().Where(x => x.ViewId == viewId).ToList();
        if (uiViews.Count == 0) return;
        UIView uiView = uiViews.First();
        BoundingBoxXYZ boundingBoxXYZ = new BoundingBoxXYZ();
        boundingBoxXYZ.Min = new XYZ(double.MaxValue, double.MaxValue, double.MaxValue);
        boundingBoxXYZ.Max = new XYZ(double.MinValue, double.MinValue, double.MinValue);
        foreach (var element in elements)
        {
            BoundingBoxXYZ boundingBox = element.InternalElement.get_BoundingBox(uidoc.ActiveView);
            if (boundingBox == null) continue;
            boundingBoxXYZ.Min = new XYZ(Math.Min(boundingBoxXYZ.Min.X, boundingBox.Min.X),
                Math.Min(boundingBoxXYZ.Min.Y, boundingBox.Min.Y),
                Math.Min(boundingBoxXYZ.Min.Z, boundingBox.Min.Z));
            boundingBoxXYZ.Max = new XYZ(Math.Max(boundingBoxXYZ.Max.X, boundingBox.Max.X),
                Math.Max(boundingBoxXYZ.Max.Y, boundingBox.Max.Y),
                Math.Max(boundingBoxXYZ.Max.Z, boundingBox.Max.Z));
        }

        uiView.ZoomAndCenterRectangle(boundingBoxXYZ.Min, boundingBoxXYZ.Max);
        if (isCropView)
        {
            TransactionManager.Instance.EnsureInTransaction(DocumentManager.Instance.CurrentDBDocument);
            uidoc.ActiveView.CropBoxActive = true;
            uidoc.ActiveView.CropBoxVisible = true;
            uidoc.ActiveView.CropBox = boundingBoxXYZ;
            TransactionManager.Instance.TransactionTaskDone();
        }
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