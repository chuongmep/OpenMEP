using System.Collections;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;
using RevitServices.Persistence;
using RevitServices.Transactions;

namespace OpenMEP.Element;

public class Fitting
{
    private Fitting()
    {
    }

    /// <summary>
    /// Add a new family instance of an union fitting into the Autodesk Revit document, using two connectors.
    /// </summary>
    /// <param name="c1"> The first connector to be connected to the union.</param>
    /// <param name="c2"> The second connector to be connected to the union.</param>
    /// <returns name="familyinstance">If creation was successful then an family instance to the new object is returned, otherwise an exception with failure information will be thrown.</returns>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewUnionFitting(Autodesk.Revit.DB.Connector c1,
        Autodesk.Revit.DB.Connector c2)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.ForceCloseTransaction();
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.FamilyInstance unionFitting = doc.Create.NewUnionFitting(c1, c2);
        TransactionManager.Instance.TransactionTaskDone();
        if (unionFitting == null)
        {
            ConnectorSet connectorSet = c1.AllRefs;
            IEnumerator enumerator = connectorSet.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Connector? current = enumerator.Current as Autodesk.Revit.DB.Connector;
                if (current == null) continue;
                if (current.Owner is Autodesk.Revit.DB.Plumbing.Pipe) continue;
                if (current.Owner is Autodesk.Revit.DB.Plumbing.PipingSystem) continue;
                global::Revit.Elements.Element? dynamoType = current.Owner.ToDynamoType();
                return dynamoType;
            }
        }

        ;
        if (unionFitting == null) return null;
        return unionFitting.ToDynamoType();
    }

    /// <summary>
    /// Add a new family instance of an elbow fitting into the Autodesk Revit document, using two connectors.
    /// </summary>
    /// <param name="firstConnector"> The first connector to be connected to the union.</param>
    /// <param name="secondConnector"> The second connector to be connected to the union.</param>
    /// <returns name="familyinstance">If creation was successful then an family instance to the new object is returned, otherwise an exception with failure information will be thrown.</returns>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewElbowFitting(Autodesk.Revit.DB.Connector firstConnector,
        Autodesk.Revit.DB.Connector secondConnector)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.FamilyInstance elbowFitting = doc.Create.NewElbowFitting(firstConnector, secondConnector);
        TransactionManager.Instance.TransactionTaskDone();
        if (elbowFitting == null) return null;
        return elbowFitting.ToDynamoType();
    }

    /// <summary>
    /// Add a new family instance of a cross fitting into the Autodesk Revit document, using four connectors.
    /// </summary>
    /// <param name="c1"> The first connector to be connected to the cross.</param>
    /// <param name="c2"> The second connector to be connected to the cross.</param>
    /// <param name="c3">The third connector to be connected to the cross.</param>
    /// <param name="c4">The fourth connector to be connected to the cross</param>
    ///<returns name="familyInstance">If creation was successful then an family instance to the new object is returned, and the transition fitting will be added at the connectors' end if necessary, otherwise an exception with failure information will be thrown.</returns>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewCrossFitting(Autodesk.Revit.DB.Connector c1,
        Autodesk.Revit.DB.Connector c2,
        Autodesk.Revit.DB.Connector c3, Autodesk.Revit.DB.Connector c4)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        bool flag1 = c1.CoordinateSystem.BasisZ.ToDynamoVector().IsParallel(c2.CoordinateSystem.BasisZ.ToDynamoVector());
        bool flag2 = c1.CoordinateSystem.BasisZ.ToDynamoVector().IsParallel(c3.CoordinateSystem.BasisZ.ToDynamoVector());
        Autodesk.Revit.DB.FamilyInstance newCrossFitting;
        // resolve problem of cross fitting with side-side-main-main input
        TransactionManager.Instance.EnsureInTransaction(doc);
        if (flag1)
        {
            newCrossFitting = doc.Create.NewCrossFitting(c1, c2, c3, c4);
        }
        else if (flag2)
        {
            newCrossFitting = doc.Create.NewCrossFitting(c1, c3, c2, c4);
        }
        else
        {
            newCrossFitting = doc.Create.NewCrossFitting(c1, c4, c2, c3);
        }
        TransactionManager.Instance.TransactionTaskDone();
        if (newCrossFitting == null) return null;
        return newCrossFitting.ToDynamoType();
    }

    /// <summary>
    /// Add a new family instance of a tee fitting into the Autodesk Revit document, using three connectors.
    /// </summary>
    /// <param name="c1">The first connector to be connected to the tee</param>
    /// <param name="c2">The second connector to be connected to the tee.</param>
    /// <param name="c3">The third connector to be connected to the tee. This should be connected to the branch of the tee.</param>
    /// <returns name="familyinstance">If creation was successful then an family instance to the new object is returned, and the transition fitting will be added at the connectors' end if necessary, otherwise an exception with failure information will be thrown</returns>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewTeeFitting(Autodesk.Revit.DB.Connector c1,
        Autodesk.Revit.DB.Connector c2,
        Autodesk.Revit.DB.Connector c3)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.FamilyInstance familyInstance = doc.Create.NewTeeFitting(c1, c2, c3);
        TransactionManager.Instance.TransactionTaskDone();
        if (familyInstance == null) return null;
        return familyInstance.ToDynamoType();
    }

    /// <summary>
    /// Add a new family instance of an takeoff fitting into the Autodesk Revit document, using one connector and one MEP curve.
    /// </summary>
    /// <param name="connector">The connector to be connected to the takeoff.</param>
    /// <param name="mepCurve">The duct or pipe which is the trunk for the takeoff.</param>
    /// <returns></returns>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewTakeoffFitting(Autodesk.Revit.DB.Connector connector,
        global::Revit.Elements.Element mepCurve)
    {
        Autodesk.Revit.DB.MEPCurve? mCurve = mepCurve.InternalElement as Autodesk.Revit.DB.MEPCurve;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.FamilyInstance familyInstance = doc.Create.NewTakeoffFitting(connector, mCurve);
        TransactionManager.Instance.TransactionTaskDone();
        if (familyInstance == null) return null;
        return familyInstance.ToDynamoType();
    }

    /// <summary>
    /// Add a new family instance of an transition fitting into the Autodesk Revit document, using two connectors.
    /// </summary>
    /// <param name="c1">The first connector to be connected to the transition.</param>
    /// <param name="c2">The second connector to be connected to the transition.</param>
    /// <returns></returns>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewTransitionFitting(Autodesk.Revit.DB.Connector c1,
        Autodesk.Revit.DB.Connector c2)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.FamilyInstance familyInstance = doc.Create.NewTransitionFitting(c1, c2);
        TransactionManager.Instance.TransactionTaskDone();
        if (familyInstance == null) return null;
        return familyInstance.ToDynamoType();
    }

    /// <summary>
    /// return system type of fitting
    /// </summary>
    /// <param name="fitting">fitting to get</param>
    /// <returns name="systemtype">system type of fitting</returns>
    public static global::Revit.Elements.Element? SystemType(global::Revit.Elements.Element fitting)
    {
        Autodesk.Revit.DB.Element element = fitting.InternalElement;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        ElementId systemTypeId =
            element!.get_Parameter(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM).AsElementId();
        if (systemTypeId == null) return null;
        var systemType = doc.GetElement(systemTypeId).ToDynamoType();
        return systemType;
    }

    /// <summary>
    /// Set Angle of tee fitting
    /// </summary>
    /// <remarks>this function just apply for tee have two connector</remarks>
    /// <param name="fitting">fitting need to change angle</param>
    /// <param name="angle">angle to set (degrees)</param>
    /// <returns name="fitting">fitting</returns>
    [NodeCategory("Action")]
    public static global::Revit.Elements.Element? SetAngle(global::Revit.Elements.Element? fitting, double angle)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        List<Autodesk.Revit.DB.Connector?> connectors =
            OpenMEP.ConnectorManager.Connector.GetConnectors(fitting);
        connectors.FirstOrDefault(x => x!.Angle != 0)!.Angle = angle * Math.PI / 180;
        TransactionManager.Instance.TransactionTaskDone();
        return fitting;
    }

#if !R20
     /// <summary>
    /// Set radius of fitting
    /// </summary>
    /// <param name="fitting">fitting will be set</param>
    /// <param name="radius">value radius</param>
    /// <returns name="fitting">fitting</returns> 
    [NodeCategory("Action")]
    public static global::Revit.Elements.Element? SetRadius(global::Revit.Elements.Element? fitting, double radius)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        List<Autodesk.Revit.DB.Connector?> connectors =
            OpenMEP.ConnectorManager.Connector.GetConnectors(fitting);
        // Get Current Unit 
        FormatOptions units = doc.GetUnits().GetFormatOptions(SpecTypeId.PipeSize);
        double value = UnitUtils.ConvertToInternalUnits(radius, units.GetUnitTypeId());
        connectors.ForEach(x => x.Radius = value);
        TransactionManager.Instance.TransactionTaskDone();
        return fitting;
    }
#endif
   

    /// <summary>
    /// Set Rotate of fitting
    /// </summary>
    /// <param name="fitting">family instance</param>
    /// <param name="Axis">Line Axis</param>
    /// <param name="angle">angle to rotate(Degrees)</param>
    /// <returns name="fitting">family instance</returns>
    [NodeCategory("Action")]
    public static global::Revit.Elements.Element Rotate(global::Revit.Elements.Element fitting, Autodesk.DesignScript.Geometry.Line Axis,
        double angle)
    {
        TransactionManager.Instance.ForceCloseTransaction();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        double degree2Radian = angle * System.Math.PI / 180.0;
        ElementTransformUtils.RotateElement(doc, fitting.InternalElement.Id,
            (Autodesk.Revit.DB.Line) Axis.ToRevitType(), degree2Radian);
        TransactionManager.Instance.TransactionTaskDone();
        return fitting;
    }

    /// <summary>
    /// return information connector of fitting
    /// </summary>
    /// <param name="fitting">family instance</param>
    [MultiReturn("Size", "PartType")]
    [NodeCategory("Query")]
    public static IDictionary ConnectorInfo(Revit.Elements.Element fitting)
    {
        List<Connector?> connectors = OpenMEP.ConnectorManager.Connector.GetConnectors(fitting);
        Autodesk.Revit.DB.FamilyInstance? element = fitting.InternalElement as Autodesk.Revit.DB.FamilyInstance;
        Autodesk.Revit.DB.MEPModel mepModel = element!.MEPModel;
        dynamic? partType = OpenMEP.ConnectorManager.MEPModel.PartType(mepModel);
        return new Dictionary<string, object?>()
        {
            {"Size", connectors.Count},
            {"PartType", partType}
        };
    }
}