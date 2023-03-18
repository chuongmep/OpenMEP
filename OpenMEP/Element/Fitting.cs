using System.Collections;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;
using OpenMEPSandbox.Geometry;
using Revit.GeometryConversion;
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
    /// <param name="firstConnector"> The first connector to be connected to the union.</param>
    /// <param name="secondConnector"> The second connector to be connected to the union.</param>
    /// <returns name="familyinstance">If creation was successful then an family instance to the new object is returned, otherwise an exception with failure information will be thrown.</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Fitting.NewUnionFitting.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewUnionFitting(Autodesk.Revit.DB.Connector firstConnector,
        Autodesk.Revit.DB.Connector secondConnector)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.ForceCloseTransaction();
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.FamilyInstance unionFitting = doc.Create.NewUnionFitting(firstConnector, secondConnector);
        TransactionManager.Instance.TransactionTaskDone();
        if (unionFitting == null)
        {
            ConnectorSet connectorSet = firstConnector.AllRefs;
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
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Fitting.NewElbowFitting.png)
    /// </example>
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
    /// <param name="connector1"> The first connector to be connected to the cross.</param>
    /// <param name="connector2"> The second connector to be connected to the cross.</param>
    /// <param name="connector3">The third connector to be connected to the cross.</param>
    /// <param name="connector4">The fourth connector to be connected to the cross</param>
    ///<returns name="familyinstance">If creation was successful then an family instance to the new object is returned, and the transition fitting will be added at the connectors' end if necessary, otherwise an exception with failure information will be thrown.</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Fitting.NewCrossFitting.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewCrossFitting(Autodesk.Revit.DB.Connector connector1,
        Autodesk.Revit.DB.Connector connector2,
        Autodesk.Revit.DB.Connector connector3, Autodesk.Revit.DB.Connector connector4)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        bool flag1 = connector1.CoordinateSystem.BasisZ.ToDynamoVector()
            .IsParallel(connector2.CoordinateSystem.BasisZ.ToDynamoVector());
        bool flag2 = connector1.CoordinateSystem.BasisZ.ToDynamoVector()
            .IsParallel(connector3.CoordinateSystem.BasisZ.ToDynamoVector());
        Autodesk.Revit.DB.FamilyInstance newCrossFitting;
        // resolve problem of cross fitting with side-side-main-main input
        TransactionManager.Instance.EnsureInTransaction(doc);
        if (flag1)
        {
            newCrossFitting = doc.Create.NewCrossFitting(connector1, connector2, connector3, connector4);
        }
        else if (flag2)
        {
            newCrossFitting = doc.Create.NewCrossFitting(connector1, connector3, connector2, connector4);
        }
        else
        {
            newCrossFitting = doc.Create.NewCrossFitting(connector1, connector4, connector2, connector3);
        }

        TransactionManager.Instance.TransactionTaskDone();
        if (newCrossFitting == null) return null;
        return newCrossFitting.ToDynamoType();
    }

    /// <summary>
    /// Add a new family instance of a tee fitting into the Autodesk Revit document, using three connectors.
    /// </summary>
    /// <param name="connector1">The first connector to be connected to the tee</param>
    /// <param name="connector2">The second connector to be connected to the tee.</param>
    /// <param name="connector3">The third connector to be connected to the tee. This should be connected to the branch of the tee.</param>
    /// <returns name="familyinstance">If creation was successful then an family instance to the new object is returned, and the transition fitting will be added at the connectors' end if necessary, otherwise an exception with failure information will be thrown</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Fitting.NewTeeFitting.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewTeeFitting(Autodesk.Revit.DB.Connector connector1,
        Connector? connector2,
        Connector? connector3)
    {
        if (connector1 == null) throw new ArgumentNullException(nameof(connector1));
        if (connector2 == null) throw new ArgumentNullException(nameof(connector2));
        if (connector3 == null) throw new ArgumentNullException(nameof(connector3));
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        // sort connectors main-main-branch
        Connector? c1;
        Connector? c2;
        Connector? c3;
        bool flag = Vector.IsPerpendicular(connector1.CoordinateSystem.BasisZ.ToVector(),
            connector2!.CoordinateSystem.BasisZ.ToVector());
        if (flag)
        {
            c1 = connector1;
            c2 = connector3;
            c3 = connector2;
        }
        else
        {
            c1 = connector1;
            c2 = connector2;
            c3 = connector3;
        }

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
    /// <returns name="familyinstance">new takeoff</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Fitting.NewTakeoffFitting.png)
    /// </example>
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
    /// <param name="connector1">The first connector to be connected to the transition.</param>
    /// <param name="connector2">The second connector to be connected to the transition.</param>
    /// <returns name="familyinsntace">new transition</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Fitting.NewTransitionFitting.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewTransitionFitting(Autodesk.Revit.DB.Connector connector1,
        Autodesk.Revit.DB.Connector connector2)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.FamilyInstance familyInstance = doc.Create.NewTransitionFitting(connector1, connector2);
        TransactionManager.Instance.TransactionTaskDone();
        if (familyInstance == null) return null;
        return familyInstance.ToDynamoType();
    }

    /// <summary>
    /// Set Angle of tee fitting
    /// </summary>
    /// <remarks>this function just apply for tee have two connector</remarks>
    /// <param name="fitting">fitting need to change angle</param>
    /// <param name="angle">angle to set (degrees)</param>
    /// <returns name="fitting">fitting</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Fitting.SetAngle.png)
    /// </example>
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
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Fitting.SetRadius.png)
    /// </example>
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
        connectors.ForEach(x => x!.Radius = value);
        TransactionManager.Instance.TransactionTaskDone();
        return fitting;
    }
#endif

    /// <summary>
    /// return information connector of fitting
    /// </summary>
    /// <param name="fitting">family instance</param>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Fitting.ConnectorInfo.png)
    /// </example>
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