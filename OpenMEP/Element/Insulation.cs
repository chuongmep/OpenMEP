using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;
using RevitServices.Persistence;
using RevitServices.Transactions;

namespace OpenMEP.Element;

/// <summary>
///    Acts as the base class for duct insulation, pipe insulation and duct lining elements.
/// </summary>
/// <since>2012</since>
public class Insulation
{
    private Insulation()
    {
    }

    /// <summary>Returns the insulation elements associated to a given element.</summary>
    /// <param name="element">The element.</param>
    /// <returns>A collection of the insulation elements.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    This id does not represent a valid host for insulation.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was NULL
    /// </exception>
    /// <since>2012</since>
    ///<returns name="elements">Insulation Elements</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Insulation.GetInsulation.png)
    /// </example>
    public static List<Revit.Elements.Element?> GetInsulation(Revit.Elements.Element element)
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.DB.Element e = element.InternalElement;
        ICollection<ElementId> ids = InsulationLiningBase.GetInsulationIds(doc, e.Id);
        List<Revit.Elements.Element?> elements = ids.Select(id => doc.GetElement(id).ToDynamoType()).ToList();
        return elements;
    }
    /// <summary>Returns the ids of the lining elements associated to a given element.</summary>
    /// <param name="element">The element.</param>
    /// <returns>A collection of the ids of the lining elements.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    This id does not represent a duct, fitting, or accessory element.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <since>2012</since>
    /// <returns name="elements">lining elements</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Insulation.GetLining.png)
    /// </example>
    public static List<Revit.Elements.Element?> GetLining(Revit.Elements.Element element)
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.DB.Element e = element.InternalElement;
        ICollection<ElementId> ids = InsulationLiningBase.GetLiningIds(doc, e.Id);
        List<Revit.Elements.Element?> elements = ids.Select(id => doc.GetElement(id).ToDynamoType()).ToList();
        return elements;
    }
    
    /// <summary>
    /// Checks if the element is added insulation or added lining.
    /// </summary>
    /// <param name="element">the element to check</param>
    /// <returns name="bool">true if element is insulation</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Insulation.IsAddInsulationOrLining.png)
    /// </example>
    [NodeCategory("Query")]
    public static bool IsAddInsulationOrLining(Revit.Elements.Element element)
    {
        List<Revit.Elements.Element?> insulation = GetInsulation(element);
        List<Revit.Elements.Element?> lining = GetLining(element);
        if (insulation.Count > 0 || lining.Count > 0) return true;
        return false;
    }

    /// <summary>Creates a new instance of insulation.</summary>
    /// <param name="element">
    ///    The pipe, fitting, accessory Element to which insulation will be added.
    /// </param>
    /// <param name="insulationType">
    ///    The insulation type.
    ///    If the input pipe insulation type is InvalidElementId, the default insulation type from the document will be used.
    /// </param>
    /// <param name="thickness">The thickness of the insulation.</param>
    /// <returns>The newly created pipe insulation.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    This id does not represent a pipe, fitting, or accessory element.
    ///    -or-
    ///    This pipe insulation type is invalid.
    ///    -or-
    ///    Thickness is not valid for assignment to insulation or lining elements.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    ///    The document is in failure mode: an operation has failed,
    ///    and Revit requires the user to either cancel the operation
    ///    or fix the problem (usually by deleting certain elements).
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ModificationForbiddenException">
    ///    The document is in failure mode: an operation has failed,
    ///    and Revit requires the user to either cancel the operation
    ///    or fix the problem (usually by deleting certain elements).
    ///    -or-
    ///    The document is being loaded, or is in the midst of another
    ///    sensitive process.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ModificationOutsideTransactionException">
    ///    The document has no open transaction.
    /// </exception>
    /// <since>2012</since>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Insulation.AddInsulation.png)
    /// </example>
    public static Revit.Elements.Element? AddInsulation(Revit.Elements.Element element,
        Revit.Elements.Element insulationType, double thickness)
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        if (insulationType == null) throw new ArgumentNullException(nameof(insulationType));
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Autodesk.Revit.DB.Element e = element.InternalElement;
        if(GetInsulation(element).Count > 0) throw new ArgumentException("Insulation already exists");
#if R20 || R21
        DisplayUnitType unitTypeId =
            doc.GetUnits().GetFormatOptions(UnitType.UT_PipeInsulationThickness).DisplayUnits;
        if (element.InternalElement is Autodesk.Revit.DB.Mechanical.DuctInsulation)
        {
            unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_HVAC_DuctInsulationThickness).DisplayUnits;
        }

        if (element.InternalElement is Autodesk.Revit.DB.Plumbing.PipeInsulation)
        {
            unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_PipeInsulationThickness).DisplayUnits;
        }

        if (element.InternalElement is Autodesk.Revit.DB.Mechanical.DuctLining)
        {
            unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_HVAC_DuctLiningThickness).DisplayUnits;
        }

        thickness = UnitUtils.ConvertToInternalUnits(thickness, unitTypeId);
#else
        var unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.DuctInsulationThickness).GetUnitTypeId();
        if (element.InternalElement is Autodesk.Revit.DB.Mechanical.DuctInsulation)
        {
            unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.DuctInsulationThickness).GetUnitTypeId();
        }

        if (element.InternalElement is Autodesk.Revit.DB.Plumbing.PipeInsulation)
        {
            unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.PipeInsulationThickness).GetUnitTypeId();
        }

        if (element.InternalElement is Autodesk.Revit.DB.Mechanical.DuctLining)
        {
            unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.DuctLiningThickness).GetUnitTypeId();
        }

        thickness = UnitUtils.ConvertToInternalUnits(thickness, unitTypeId);
#endif
        TransactionManager.Instance.EnsureInTransaction(doc);
        if (e is Autodesk.Revit.DB.MEPCurve mepCurve)
        {
            if (mepCurve.MEPSystem is Autodesk.Revit.DB.Mechanical.MechanicalSystem)
            {
                return DuctInsulation
                    .Create(doc, new ElementId(element.Id), new ElementId(insulationType.Id), thickness).ToDynamoType();
            }

            if (mepCurve.MEPSystem is Autodesk.Revit.DB.Plumbing.PipingSystem)
            {
                return Autodesk.Revit.DB.Plumbing.PipeInsulation
                    .Create(doc, new ElementId(element.Id), new ElementId(insulationType.Id), thickness).ToDynamoType();
            }
        }

        if (e is Autodesk.Revit.DB.FamilyInstance fam)
        {
#if R20 || R21 || R22
            if (fam.Category.Id.IntegerValue == -2008010)
            {
                return DuctInsulation
                    .Create(doc, new ElementId(element.Id), new ElementId(insulationType.Id), thickness).ToDynamoType();
            }

            if (fam.Category.Id.IntegerValue == -2008049)
            {
                return Autodesk.Revit.DB.Plumbing.PipeInsulation.Create(doc, new ElementId(element.Id),
                    new ElementId(insulationType.Id), thickness).ToDynamoType();
            }
#else
             if (fam.Category.BuiltInCategory == BuiltInCategory.OST_DuctFitting)
            {
                return DuctInsulation
                    .Create(doc, new ElementId(element.Id), new ElementId(insulationType.Id), thickness).ToDynamoType();
            }

            if (fam.Category.BuiltInCategory == BuiltInCategory.OST_PipeFitting)
            {
                return Autodesk.Revit.DB.Plumbing.PipeInsulation.Create(doc, new ElementId(element.Id),
                    new ElementId(insulationType.Id), thickness).ToDynamoType();
            }
#endif
        }

        TransactionManager.Instance.TransactionTaskDone();
        return null;
    }

    

    /// <summary>
    /// The the host element for the insulation or lining element.
    /// <since>2012</since>
    /// </summary>
    /// <param name="element">the insulation element</param>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Insulation.HostElement.png)
    /// </example>
    [NodeCategory("Query")]
    public static Revit.Elements.Element? HostElement(Revit.Elements.Element element)
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        InsulationLiningBase? insulationLiningBase = element.InternalElement as InsulationLiningBase;
        return insulationLiningBase?.HostElementId.ToDynamoType();
    }

    /// <summary>
    /// The id of the host element for the insulation or lining element.
    /// <since>2012</since>
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Insulation.Thickness.png)
    /// </example>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Insulation.Thickness.png)
    /// </example>
    [NodeCategory("Query")]
    public static double? Thickness(Revit.Elements.Element element)
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        if (element.InternalElement is InsulationLiningBase insulationLiningBase)
        {
            double thickness = insulationLiningBase.Thickness;
            Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20 || R21
            DisplayUnitType unitTypeId =
                doc.GetUnits().GetFormatOptions(UnitType.UT_PipeInsulationThickness).DisplayUnits;
            if (element.InternalElement is Autodesk.Revit.DB.Mechanical.DuctInsulation)
            {
                unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_HVAC_DuctInsulationThickness).DisplayUnits;
            }

            if (element.InternalElement is Autodesk.Revit.DB.Plumbing.PipeInsulation)
            {
                unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_PipeInsulationThickness).DisplayUnits;
            }

            if (element.InternalElement is Autodesk.Revit.DB.Mechanical.DuctLining)
            {
                unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_HVAC_DuctLiningThickness).DisplayUnits;
            }

            double thicknessInMeters = UnitUtils.ConvertFromInternalUnits(thickness, unitTypeId);
            return thicknessInMeters;
#else
            var unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.DuctInsulationThickness).GetUnitTypeId();
            if (element.InternalElement is Autodesk.Revit.DB.Mechanical.DuctInsulation)
            {
                unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.DuctInsulationThickness).GetUnitTypeId();
            }

            if (element.InternalElement is Autodesk.Revit.DB.Plumbing.PipeInsulation)
            {
                unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.PipeInsulationThickness).GetUnitTypeId();
            }

            if (element.InternalElement is Autodesk.Revit.DB.Mechanical.DuctLining)
            {
                unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.DuctLiningThickness).GetUnitTypeId();
            }

            double thicknessInMeters = UnitUtils.ConvertFromInternalUnits(thickness, unitTypeId);
            return thicknessInMeters;
#endif
        }

        return null;
    }
}