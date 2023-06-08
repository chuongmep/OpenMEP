using System.Collections;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Dynamo.Graph.Nodes;
using OpenMEPRevit.Helpers;
using OpenMEPSandbox.Geometry;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;
using Line = Autodesk.Revit.DB.Line;
using Point = OpenMEPSandbox.Geometry.Point;

namespace OpenMEPRevit.Element;
/// <summary>
/// A class can use for DuctType, PipeType, CableTrayType, ConduitType, WireType, MEPCurveType
/// </summary>
public class MEPCurve
{
    private MEPCurve()
    {
    }

    /// <summary>
    /// break mep curve at point
    /// </summary>
    /// <param name="mepCurve">A curve object for duct or pipe blend elements.</param>
    /// <param name="point">location to break on mep curve</param>
    /// <returns name="element">new element break from mep curve</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.BreakCurve.png)
    /// </example>
    [NodeCategory("Action")]
    [NodeSearchTags("split", "cut", "divide")]
    public static global::Revit.Elements.Element? BreakCurve(global::Revit.Elements.Element mepCurve,
        Autodesk.DesignScript.Geometry.Point point)
    {
        if (mepCurve == null) throw new ArgumentNullException(nameof(mepCurve));
        if (point == null) throw new ArgumentNullException(nameof(point));
        Autodesk.Revit.DB.Element internalElement = mepCurve.InternalElement;
        TransactionManager.Instance.ForceCloseTransaction();
        TransactionManager.Instance.EnsureInTransaction(internalElement.Document);
        if (internalElement is Autodesk.Revit.DB.MEPCurve mCurve)
        {
            XYZ location = point.ToXyz();
            ElementId id = BreakCurve(mCurve, location);
            if (id != ElementId.InvalidElementId)
            {
                return mCurve.Document.GetElement(id).ToDynamoType();
            }
        }

        TransactionManager.Instance.TransactionTaskDone();
        return null;
    }

    /// <summary>
    /// Break MEP curve at given point
    /// </summary>
    /// <param name="mepCurve">Autodesk.Revit.DB.MEPCurve</param>
    /// <param name="ptBreak">point to break</param>
    /// <returns></returns>
    private static ElementId BreakCurve(Autodesk.Revit.DB.MEPCurve? mepCurve, XYZ? ptBreak)
    {
        if (mepCurve == null) throw new ArgumentNullException(nameof(mepCurve));
        if (ptBreak == null) throw new ArgumentNullException(nameof(ptBreak));
        if (mepCurve is Autodesk.Revit.DB.Plumbing.Pipe || mepCurve is Autodesk.Revit.DB.Plumbing.FlexPipe)
        {
            return PlumbingUtils.BreakCurve(mepCurve.Document, mepCurve.Id, ptBreak);
        }

        if (mepCurve is Autodesk.Revit.DB.Mechanical.Duct || mepCurve is Autodesk.Revit.DB.Mechanical.FlexDuct)
        {
            return MechanicalUtils.BreakCurve(mepCurve.Document, mepCurve.Id, ptBreak);
        }

        if (mepCurve is Autodesk.Revit.DB.Electrical.Conduit || mepCurve is Autodesk.Revit.DB.Electrical.CableTray)
        {
            ElementId elementId = BreakConduitCableTray(mepCurve.Document, mepCurve.Id, ptBreak);
            return elementId;
        }

        return ElementId.InvalidElementId;
    }

    private static ElementId BreakConduitCableTray(Autodesk.Revit.DB.Document doc, ElementId conduitId, XYZ breakPoint)
    {
        var conduit = doc.GetElement(conduitId);
        //copy mepCurveToOptimize as newPipe and move to brkPoint
        var location = conduit.Location as LocationCurve;
        var start = location!.Curve.GetEndPoint(0);
        var end = location.Curve.GetEndPoint(1);
        var copiedEls = ElementTransformUtils.CopyElement(doc, conduit.Id, breakPoint - start);
        var newId = copiedEls.First();

        //shorten mepCurveToOptimize and newPipe (adjust endpoints)
        AdjustMepCurve(conduit, start, breakPoint);
        AdjustMepCurve(doc.GetElement(newId), breakPoint, end);

        return newId;
    }

    private static void AdjustMepCurve(Autodesk.Revit.DB.Element mepCurve, XYZ p1, XYZ p2)
    {
        var location = mepCurve.Location as LocationCurve;
        location!.Curve = Line.CreateBound(p1, p2);
    }

    /// <summary>
    /// create a union fitting from two mep curve
    /// </summary>
    /// <param name="mepCurve1">A curve object for duct or pipe blend first elements.</param>
    /// <param name="mepCurve2">A curve object for duct or pipe blend second elements.</param>
    /// <returns name="family instance">union fitting</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.NewUnionFitting.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewUnionFitting(global::Revit.Elements.Element mepCurve1,
        global::Revit.Elements.Element mepCurve2)
    {
        TransactionManager.Instance.ForceCloseTransaction();
        Autodesk.Revit.DB.Document doc = mepCurve1.InternalElement.Document;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Connector? c1 = ConnectorManager.Connector.GetConnectorClosest(mepCurve1, mepCurve2);
        Connector? c2 = ConnectorManager.Connector.GetConnectorClosest(mepCurve2, mepCurve1);
        Autodesk.Revit.DB.FamilyInstance newUnionFitting = doc.Create.NewUnionFitting(c2, c1);
        TransactionManager.Instance.TransactionTaskDone();
        if (newUnionFitting == null)
        {
            List<Connector> connectors = ConnectorManager.Connector.GetConnectors(mepCurve1);
            Connector? c11 = ConnectorManager.Connector.GetConnectorClosest(c1, connectors);
            ConnectorSet connectorSet = c11!.AllRefs;
            IEnumerator enumerator = connectorSet.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Connector? current = enumerator.Current as Autodesk.Revit.DB.Connector;
                if (current == null) continue;
                if (current.Owner.Id.IntegerValue == mepCurve1.Id) continue;
                if (current.Owner is Autodesk.Revit.DB.Plumbing.PipingSystem) continue;
                global::Revit.Elements.Element? dynamoType = current.Owner.ToDynamoType();
                return dynamoType;
            }
        }
        if(newUnionFitting != null) return newUnionFitting.ToDynamoType();
        return null;
    }

    /// <summary>
    /// create a elbow fitting from two mep curve
    /// </summary>
    /// <param name="mepCurve1">A curve object for duct or pipe blend first elements.</param>
    /// <param name="mepCurve2">A curve object for duct or pipe second elements.</param>
    /// <returns name="family instance">elbow fitting</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.NewElbowFitting.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewElbowFitting(global::Revit.Elements.Element mepCurve1,
        global::Revit.Elements.Element mepCurve2)
    {
        TransactionManager.Instance.ForceCloseTransaction();
        Autodesk.Revit.DB.Document doc = mepCurve1.InternalElement.Document;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Connector? c1 = ConnectorManager.Connector.GetConnectorClosest(mepCurve1, mepCurve2);
        Connector? c2 = ConnectorManager.Connector.GetConnectorClosest(mepCurve2, mepCurve1);
        Autodesk.Revit.DB.FamilyInstance newElbowFitting = doc.Create.NewElbowFitting(c2, c1);
        TransactionManager.Instance.TransactionTaskDone();
        if (newElbowFitting == null) return null;
        return newElbowFitting.ToDynamoType();
    }

    /// <summary>
    /// Add a new family instance of a tee fitting into the Autodesk Revit document,
    /// using four mep curves.
    /// </summary>
    /// <param name="mepCurve1">the first mepCurve(Pipe/Duct/CableTray)</param>
    /// <param name="mepCurve2">the second mepCurve(Pipe/Duct/CableTray)</param>
    /// <param name="mepCurve3">the three mepCurve(Pipe/Duct/CableTray)</param>
    /// <param name="mepCurve4">the four mepCurve(Pipe/Duct/CableTray)</param>
    /// <returns name="familyinstance">new cross fitting</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.NewCrossFitting.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewCrossFitting(global::Revit.Elements.Element mepCurve1,
        global::Revit.Elements.Element mepCurve2, global::Revit.Elements.Element mepCurve3,
        global::Revit.Elements.Element mepCurve4)
    {
        TransactionManager.Instance.ForceCloseTransaction();
        Autodesk.Revit.DB.Document doc = mepCurve1.InternalElement.Document;
        Connector? c1 = ConnectorManager.Connector.GetConnectorClosest(mepCurve1, mepCurve2);
        Connector? c2 = ConnectorManager.Connector.GetConnectorClosest(mepCurve2, mepCurve1);
        Connector? c3 = ConnectorManager.Connector.GetConnectorClosest(mepCurve3, mepCurve1);
        Connector? c4 = ConnectorManager.Connector.GetConnectorClosest(mepCurve4, mepCurve3);
        bool flag1 = c1!.CoordinateSystem.BasisZ.ToDynamoVector()
            .IsParallel(c2!.CoordinateSystem.BasisZ.ToDynamoVector());
        bool flag2 = c1.CoordinateSystem.BasisZ.ToDynamoVector()
            .IsParallel(c3!.CoordinateSystem.BasisZ.ToDynamoVector());
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
    /// Add a new family instance of a tee fitting into the Autodesk Revit document,
    /// using three mep curves.
    /// </summary>
    /// <param name="mepCurve1">A curve object for duct or pipe blend first elements.</param>
    /// <param name="mepCurve2">A curve object for duct or pipe blend second elements.</param>
    /// <param name="mepCurve3">A curve object for duct or pipe blend three elements.</param>
    /// <returns name="familyinstance">new tee fitting</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.NewTeeFitting3.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewTeeFitting(global::Revit.Elements.Element mepCurve1,
        global::Revit.Elements.Element mepCurve2, global::Revit.Elements.Element mepCurve3)
    {
        TransactionManager.Instance.ForceCloseTransaction();
        Autodesk.Revit.DB.Document doc = mepCurve1.InternalElement.Document;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Connector? connector1 = ConnectorManager.Connector.GetConnectorClosest(mepCurve1, mepCurve2);
        Connector? connector2 = ConnectorManager.Connector.GetConnectorClosest(mepCurve2, mepCurve1);
        Connector? connector3 = ConnectorManager.Connector.GetConnectorClosest(mepCurve3, mepCurve1);
        if(connector1==null||connector2==null||connector3==null) throw new Exception("connector is null");
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
        Autodesk.Revit.DB.FamilyInstance newTeeFitting = doc.Create.NewTeeFitting(c2, c1, c3);
        TransactionManager.Instance.TransactionTaskDone();
        if (newTeeFitting == null) return null;
        return newTeeFitting.ToDynamoType();
    }

    /// <summary>
    /// Create a new tee fitting 90 degrees by two mep curve
    /// </summary>
    /// <param name="MepCurve1">the first element mep curve</param>
    /// <param name="MepCurve2">the second element mep curve</param>
    /// <returns name="familyinstance">the element family instance</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.NewTeeFitting2.png)
    /// </example>
    public static Revit.Elements.Element? NewTeeFitting(Revit.Elements.Element MepCurve1,
        Revit.Elements.Element MepCurve2)
    {
        if(MepCurve1==null) throw new ArgumentNullException(nameof(MepCurve1));
        if(MepCurve2==null) throw new ArgumentNullException(nameof(MepCurve2));
        //review is perpendicular
        Autodesk.Revit.DB.Element e1 = MepCurve1.InternalElement;
        LocationCurve? lc1 = e1.Location as LocationCurve;
        Autodesk.Revit.DB.Curve curve1 = lc1!.Curve;
        Autodesk.Revit.DB.Element e2 = MepCurve2.InternalElement;
        LocationCurve? lc2 = e2.Location as LocationCurve;
        Autodesk.Revit.DB.Curve curve2 = lc2!.Curve;
        Autodesk.Revit.DB.XYZ vector = curve1.GetEndPoint(1) - curve1.GetEndPoint(0);
        Autodesk.Revit.DB.XYZ vector2 = curve2.GetEndPoint(1) - curve2.GetEndPoint(0);
        bool flag = vector.Normalize().DotProduct(vector2.Normalize()).Equals(0);
        if (flag) throw new Exception("The two curves are not perpendicular");
        // review main pipe branch
        XYZ firstPoint = curve1.GetEndPoint(0);
        XYZ secondPoint = curve2.GetEndPoint(0);
        Autodesk.DesignScript.Geometry.Line? protoType = curve2.ToProtoType(false) as Autodesk.DesignScript.Geometry.Line;
        if(protoType==null) throw new Exception("The branch mep curve can't get location");
        Autodesk.DesignScript.Geometry.Point pointProjected = Point.ProjectOnToLine(firstPoint.ToPoint(false),protoType
            );
        Revit.Elements.Element? mainMEPCurve = null;
        Revit.Elements.Element? branchMEPCurve = null;
        bool isOnLine = Point.IsOnLine(pointProjected,protoType);
        if (isOnLine)
        {
            mainMEPCurve = MepCurve2;
            branchMEPCurve = MepCurve1;
        }
        else
        {
            mainMEPCurve = MepCurve1;
            branchMEPCurve = MepCurve2;
            protoType = curve1.ToProtoType(false) as Autodesk.DesignScript.Geometry.Line;
            if(protoType==null) throw new Exception("The main mep curve can't get location");
            pointProjected = Point.ProjectOnToLine(secondPoint.ToPoint(false),protoType);
            isOnLine = Point.IsOnLine(pointProjected,protoType);
            if(!isOnLine) throw new Exception("The two curves are can't create a tee fitting");
        }
        // create tee
        TransactionManager.Instance.ForceCloseTransaction();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        protoType = mainMEPCurve.GetLocation() as Autodesk.DesignScript.Geometry.Line;
        Autodesk.DesignScript.Geometry.Line? line = branchMEPCurve.GetLocation() as Autodesk.DesignScript.Geometry.Line;
        if(line==null) throw new Exception("The branch mep curve can't get location");
        pointProjected = Point.ProjectOnToLine(line.StartPoint,protoType);
        Revit.Elements.Element? newMepCurve = MEPCurve.BreakCurve(mainMEPCurve,pointProjected);
        TransactionManager.Instance.TransactionTaskDone();
        Connector? c1 = ConnectorManager.Connector.GetConnectorClosest(newMepCurve,mainMEPCurve);
        if(c1==null) throw new Exception("Can't find connector in the main mep curve");
        Connector? c2 = ConnectorManager.Connector.GetConnectorClosest(mainMEPCurve,branchMEPCurve);
        List<Connector> connectors = ConnectorManager.Connector.GetConnectors(branchMEPCurve);
        Connector? c3 = ConnectorManager.Connector.GetConnectorClosest(c1.Origin.ToPoint(), connectors);
        Revit.Elements.Element? teeFitting = Fitting.NewTeeFitting(c1, c2, c3);
        return teeFitting;
    }

    /// <summary>
    /// Add a new family instance of an transition fitting into the Autodesk Revit document,
    /// using two connectors.
    /// </summary>
    /// <param name="mepCurve1">the first mepCurve(Pipe/Duct/CableTray)</param>
    /// <param name="mepCurve2">the second mepCurve(Pipe/Duct/CableTray)</param>
    /// <returns name="family instance">new transition</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.NewTransitionFitting.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewTransitionFitting(global::Revit.Elements.Element mepCurve1,
        global::Revit.Elements.Element mepCurve2)
    {
        TransactionManager.Instance.ForceCloseTransaction();
        Autodesk.Revit.DB.Document doc = mepCurve1.InternalElement.Document;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Connector? c1 = ConnectorManager.Connector.GetConnectorClosest(mepCurve1, mepCurve2);
        Connector? c2 = ConnectorManager.Connector.GetConnectorClosest(mepCurve2, mepCurve1);
        Autodesk.Revit.DB.FamilyInstance newTransitionFitting = doc.Create.NewTransitionFitting(c2, c1);
        TransactionManager.Instance.TransactionTaskDone();
        if (newTransitionFitting == null) return null;
        return newTransitionFitting.ToDynamoType();
    }

    /// <summary>
    /// Add a new family instance of an takeoff fitting into the Autodesk Revit document,
    /// using one connector and one MEP curve.
    /// </summary>
    /// <param name="connector">connector to be connector</param>
    /// <param name="mepCurve">mepCurve connect to create Takeoff</param>
    /// <returns name="familyinstance">new takeoff fitting</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.NewTakeoffFitting.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? NewTakeoffFitting(Autodesk.Revit.DB.Connector connector,
        global::Revit.Elements.Element mepCurve)
    {
        TransactionManager.Instance.ForceCloseTransaction();
        Autodesk.Revit.DB.Document doc = mepCurve.InternalElement.Document;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.MEPCurve? internalElement = mepCurve.InternalElement as Autodesk.Revit.DB.MEPCurve;
        Autodesk.Revit.DB.FamilyInstance newTakeoffFitting = doc.Create.NewTakeoffFitting(connector, internalElement);
        TransactionManager.Instance.TransactionTaskDone();
        if (newTakeoffFitting == null) return null;
        return newTakeoffFitting.ToDynamoType();
    }

    /// <summary>The diameter of the MEP curve.</summary>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    /// Thrown when the MEP curve's Shape is not round.
    /// </exception>
    /// <remarks>This property is used to retrieve the diameter of the MEP curve.</remarks>
    /// <returns name="double">diameter of mep curve</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.Diameter.png)
    /// </example>
    [NodeCategory("Query")]
    public static double? Diameter(Revit.Elements.Element mepCurve)
    {
        if (mepCurve == null) throw new ArgumentNullException(nameof(mepCurve));
        Autodesk.Revit.DB.MEPCurve? internalElement = mepCurve.InternalElement as Autodesk.Revit.DB.MEPCurve;
        if(internalElement == null) throw new Exception("The element is not a MEP curve");
        double dia=  internalElement.Diameter;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Length).DisplayUnits;
        double value = UnitUtils.ConvertToInternalUnits(dia, unitTypeId);
        return value;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();
        double value = UnitUtils.ConvertFromInternalUnits(dia, unitTypeId);
        return value;
#endif
        
    }

    /// <summary>The width of the MEP curve.</summary>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    /// Thrown when the MEP curve's shape is not rectangular.
    /// </exception>
    /// <remarks>This property is used to retrieve the width of the MEP curve.</remarks>
    /// <returns name="double">width of mep curve</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.Width.png)
    /// </example>
    [NodeCategory("Query")]
    public static double? Width(Revit.Elements.Element mepCurve)
    {
        if (mepCurve == null) throw new ArgumentNullException(nameof(mepCurve));
        Autodesk.Revit.DB.MEPCurve? internalElement = mepCurve.InternalElement as Autodesk.Revit.DB.MEPCurve;
        if (internalElement == null) throw new Exception("The element is not a MEP curve");
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        double w = internalElement.Width;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Length).DisplayUnits;
        double value = UnitUtils.ConvertToInternalUnits(w, unitTypeId);
        return value;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();
        double value = UnitUtils.ConvertFromInternalUnits(w, unitTypeId);
        return value;
#endif
    }

    /// <summary>The height of the MEP curve.</summary>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    /// Thrown when the MEP curve's Shape is not rectangular.
    /// </exception>
    /// <remarks>This property is used to retrieve the height of the MEP curve.</remarks>
    /// <returns name="double">The height of the MEP curve</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.Height.png)
    /// </example>
    [NodeCategory("Query")]
    public static double? Height(Revit.Elements.Element mepCurve)
    {
        if (mepCurve == null) throw new ArgumentNullException(nameof(mepCurve));
        Autodesk.Revit.DB.MEPCurve? internalElement = mepCurve.InternalElement as Autodesk.Revit.DB.MEPCurve;
        if (internalElement == null) throw new Exception("The element is not a MEP curve");
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        double h = internalElement.Height;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Length).DisplayUnits;
        double value = UnitUtils.ConvertToInternalUnits(h, unitTypeId);
        return value;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();
        double value = UnitUtils.ConvertFromInternalUnits(h, unitTypeId);
        return value;
#endif
        
    }

    /// <summary>The offset of the MEP curve.</summary>
    /// <remarks>This property is used to retrieve the offset of the MEP curve.
    /// If the curve is not in a horizontal plane, this value will be the start point's offset.</remarks>
    ///<returns name="double">Level offset</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.LevelOffset.png)
    /// </example>
    [NodeCategory("Query")]
    public static double? LevelOffset(Revit.Elements.Element mepCurve)
    {
        if (mepCurve == null) throw new ArgumentNullException(nameof(mepCurve));
        Autodesk.Revit.DB.MEPCurve? internalElement = mepCurve.InternalElement as Autodesk.Revit.DB.MEPCurve;
        if (internalElement == null) throw new Exception("The element is not a MEP curve");
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        double offset = internalElement.LevelOffset;
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Length).DisplayUnits;
        double value = UnitUtils.ConvertToInternalUnits(offset, unitTypeId);
        return value;
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();
        double value = UnitUtils.ConvertFromInternalUnits(offset, unitTypeId);
        return value;
#endif
    }

    /// <summary>The reference level of the MEP curve.</summary>
    /// <remarks>This property is used to retrieve the reference level of the MEP curve.
    /// If the curve is not in a horizontal plane, this value will be the start point's reference level.</remarks>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.ReferenceLevel.png)
    /// </example>
    [NodeCategory("Query")]
    public static Revit.Elements.Element? ReferenceLevel(Revit.Elements.Element mepCurve)
    {
        if (mepCurve == null) throw new ArgumentNullException(nameof(mepCurve));
        Autodesk.Revit.DB.MEPCurve? internalElement = mepCurve.InternalElement as Autodesk.Revit.DB.MEPCurve;
        return internalElement?.ReferenceLevel.ToDynamoType();
    }

    /// <summary>The connector manager of this MEP curve</summary>
    /// <returns name="connectorManager">The connector manager of this MEP curve</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.GetConnectorManager.png)
    /// </example>
    [NodeCategory("Query")]
    public static Autodesk.Revit.DB.ConnectorManager? GetConnectorManager(Revit.Elements.Element mepCurve)
    {
        if (mepCurve == null) throw new ArgumentNullException(nameof(mepCurve));
        Autodesk.Revit.DB.MEPCurve? internalElement = mepCurve.InternalElement as Autodesk.Revit.DB.MEPCurve;
        return internalElement?.ConnectorManager;
    }

    /// <summary>
    /// return two connectors closet of two pipes
    /// </summary>
    /// <param name="mepCurve1">the first mepCurve</param>
    /// <param name="mepCurve2">the second mepCurve</param>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.GetTwoConnectorClosest.png)
    /// </example>
    [MultiReturn("Connector1", "Connector2")]
    public static Dictionary<string, object?> GetTwoConnectorClosest(global::Revit.Elements.Element mepCurve1,
        global::Revit.Elements.Element mepCurve2)
    {
        Connector? connector1 = ConnectorManager.Connector.GetConnectorClosest(mepCurve1, mepCurve2);
        Connector? connector2 = ConnectorManager.Connector.GetConnectorClosest(mepCurve2, mepCurve1);
        return new Dictionary<string, object?>()
        {
            {"Connector1", connector1},
            {"Connector2", connector2}
        };
    }

    /// <summary>
    ///  Return three points closest inside three mep curves
    /// </summary>
    /// <param name="mepCurve1">the first mepCurve</param>
    /// <param name="mepCurve2">the second mepCurve</param>
    /// <param name="mepCurve3">the three mepCurve</param>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.GetThreeConnectorsClosest.png)
    /// </example>
    [MultiReturn("Connector1", "Connector2", "Connector3")]
    public static IDictionary<string, object?> GetThreeConnectorsClosest(global::Revit.Elements.Element? mepCurve1,
        global::Revit.Elements.Element? mepCurve2, global::Revit.Elements.Element? mepCurve3)
    {
        if(mepCurve1==null) throw new ArgumentNullException(nameof(mepCurve1));
        if(mepCurve2==null) throw new ArgumentNullException(nameof(mepCurve2));
        if(mepCurve3==null) throw new ArgumentNullException(nameof(mepCurve3));
        List<Connector> connectorsPipe1 = ConnectorManager.Connector.GetConnectors(mepCurve1);
        if (!connectorsPipe1.Any()) throw new ArgumentException(nameof(mepCurve1));
        List<Connector> connectorsPipe2 = ConnectorManager.Connector.GetConnectors(mepCurve2);
        if (!connectorsPipe2.Any()) throw new ArgumentException(nameof(mepCurve2));
        List<Connector> connectorsPipe3 = ConnectorManager.Connector.GetConnectors(mepCurve3);
        if (!connectorsPipe3.Any()) throw new ArgumentException(nameof(mepCurve3));
        Connector? c2 = ConnectorManager.Connector.GetConnectorClosest(mepCurve1, mepCurve2);
        Connector? c1 = ConnectorManager.Connector.GetConnectorClosest(mepCurve2, mepCurve1);
        Connector? c3 = ConnectorManager.Connector.GetConnectorClosest(mepCurve3, mepCurve1);
        return new Dictionary<string, object?>()
        {
            {"Connector1", c1},
            {"Connector2", c2},
            {"Connector3", c3}
        };
    }

    /// <summary>
    ///  Return four connector closest inside four mep curves
    /// </summary>
    /// <param name="mepCurve1">the first mepCurve</param>
    /// <param name="mepCurve2">the second mepCurve</param>
    /// <param name="mepCurve3">the three mepCurve</param>
    /// <param name="mepCurve4">the four mepCurve</param>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.GetFourConnectorsClosest.png)
    /// </example>
    [MultiReturn("Connector1", "Connector2", "Connector3", "Connector4")]
    public static IDictionary<string, object?> GetFourConnectorsClosest(global::Revit.Elements.Element? mepCurve1,
        global::Revit.Elements.Element? mepCurve2, global::Revit.Elements.Element? mepCurve3,
        global::Revit.Elements.Element? mepCurve4)
    {
        if(mepCurve1 == null) throw new ArgumentNullException(nameof(mepCurve1));
        if(mepCurve2 == null) throw new ArgumentNullException(nameof(mepCurve2));
        if(mepCurve3 == null) throw new ArgumentNullException(nameof(mepCurve3));
        if(mepCurve4 == null) throw new ArgumentNullException(nameof(mepCurve4));
        List<Connector> connectors1 = ConnectorManager.Connector.GetConnectors(mepCurve1);
        if (!connectors1.Any()) throw new ArgumentException(nameof(mepCurve1));
        List<Connector> connectors2 = ConnectorManager.Connector.GetConnectors(mepCurve2);
        if (!connectors2.Any()) throw new ArgumentException(nameof(mepCurve2));
        List<Connector> connectors3 = ConnectorManager.Connector.GetConnectors(mepCurve3);
        if (!connectors3.Any()) throw new ArgumentException(nameof(mepCurve3));
        List<Connector> connectors4 = ConnectorManager.Connector.GetConnectors(mepCurve3);
        if (!connectors4.Any()) throw new ArgumentException(nameof(mepCurve4));
        Connector? c2 = ConnectorManager.Connector.GetConnectorClosest(mepCurve1, mepCurve2);
        Connector? c1 = ConnectorManager.Connector.GetConnectorClosest(mepCurve2, mepCurve1);
        Connector? c3 = ConnectorManager.Connector.GetConnectorClosest(mepCurve3, mepCurve2);
        Connector? c4 = ConnectorManager.Connector.GetConnectorClosest(mepCurve4, mepCurve3);
        return new Dictionary<string, object?>()
        {
            {"Connector1", c1},
            {"Connector2", c2},
            {"Connector3", c3},
            {"Connector4", c4}
        };
    }

    /// <summary>
    /// Return the slope of mepCurve
    /// </summary>
    /// <param name="mepCurve">The element mepCurve</param>
    /// <param name="digit">Number of fractional digits in the return value</param>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPCurve.Slope.png)
    /// </example>
    [MultiReturn("Percent", "Degrees", "Ratio")]
    [NodeCategory("Query")]
    public static Dictionary<string, object?> Slope(Revit.Elements.Element mepCurve, double digit = 0)
    {
        if (mepCurve == null) throw new ArgumentNullException(nameof(mepCurve));
        Autodesk.Revit.DB.MEPCurve? internalElement = mepCurve.InternalElement as Autodesk.Revit.DB.MEPCurve;
        LocationCurve? locationCurve = internalElement!.Location as LocationCurve;
        return OpenMEPSandbox.Geometry.Line.Slope(locationCurve?.Curve.ToDynamoType(), digit);
    }
}