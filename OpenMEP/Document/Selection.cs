using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OpenMEP.Helpers;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using Point = Autodesk.DesignScript.Geometry.Point;

namespace OpenMEP.Document;

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
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/document/dyn/pic/Selection.PickOrderElements.gif)
    /// [Selection.PickOrderElements.dyn](../OpenMEPPage/document/dyn/Selection.PickOrderElements.dyn)
    /// </example>
    public static List<Revit.Elements.Element> PickOrderElements(bool flag)
    {
        List<Revit.Elements.Element> elements = new List<Revit.Elements.Element>();
        try
        {
            TaskDialog.Show("OpenMEP", "Select Ordered By Pick Element,Press Esc to Cancel", TaskDialogCommonButtons.Ok);
            while (true)
            {
                Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
                UIDocument uidoc = DocumentManager.Instance.CurrentUIApplication.ActiveUIDocument;
                Autodesk.Revit.UI.Selection.Selection selection = uidoc.Selection;
                Autodesk.Revit.DB.ElementId elementId = selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element).ElementId;
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
}