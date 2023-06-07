using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;
using RevitServices.Persistence;
using RevitServices.Transactions;

namespace OpenMEP.Element;
/// <summary>This class represents a conduit in Autodesk Revit.</summary>
public class Conduit
{
    private Conduit()
    {
    }

    /// <summary>Creates a new instance of conduit.</summary>
    /// <remarks>This method will regenerate the document.</remarks>
    /// <param name="conduitType">
    ///    The conduit type.  This must be a conduit type accepted by isValidConduitType().
    ///    If the input conduit type is InvalidElementId, the default conduit type from the document will be used.
    /// </param>
    /// <param name="startPoint">The start point of the conduit location line.</param>
    /// <param name="endPoint">The end point of the conduit location line.</param>
    /// <param name="level">
    ///    The element of the level which this conduit based.
    ///    If the input level id is invalidElementId = -1, the nearest level will be used.
    /// </param>
    /// <returns>The newly created conduit.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    This conduit type is invalid.
    ///    -or-
    ///    This level id is invalid.
    ///    -or-
    ///    The points of startPoint and endPoint are too close: for MEPCurve, the minimum length is 1/10 inch.
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
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Conduit.CreateByTwoPoint.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreateByTwoPoint(Revit.Elements.Element conduitType,
        Autodesk.DesignScript.Geometry.Point startPoint, Autodesk.DesignScript.Geometry.Point endPoint,
        Revit.Elements.Element level)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.ForceCloseTransaction();
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Electrical.Conduit conduit = Autodesk.Revit.DB.Electrical.Conduit.Create(doc,
            new ElementId(conduitType.Id), startPoint.ToRevitType(),
            endPoint.ToRevitType(), new ElementId(level.Id));
        TransactionManager.Instance.TransactionTaskDone();
        return conduit.ToDynamoType();
    }

    /// <summary>Creates a new instance of conduit.</summary>
    /// <remarks>This method will regenerate the document.</remarks>
    /// <param name="conduitType">
    ///    The conduit type.  This must be a conduit type accepted by isValidConduitType().
    ///    If the input conduit type is InvalidElementId, the default conduit type from the document will be used.
    /// </param>
    /// <param name="startPoint">The start point of the conduit location line.</param>
    /// <param name="endPoint">The end point of the conduit location line.</param>
    /// <param name="level">
    ///    The element of the level which this conduit based.
    ///    If the input level id is invalidElementId = -1, the nearest level will be used.
    /// </param>
    /// <param name="diameter">diameter value of conduit</param>
    /// <returns>The newly created conduit.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    This conduit type is invalid.
    ///    -or-
    ///    This level id is invalid.
    ///    -or-
    ///    The points of startPoint and endPoint are too close: for MEPCurve, the minimum length is 1/10 inch.
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
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Conduit.CreateByTwoPointDiameter.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreateByTwoPoint(Revit.Elements.Element conduitType,
        Autodesk.DesignScript.Geometry.Point startPoint, Autodesk.DesignScript.Geometry.Point endPoint,
        Revit.Elements.Element level,double diameter)
    {
        Revit.Elements.Element? conduit = CreateByTwoPoint(conduitType, startPoint, endPoint, level);
        return SetDiameter(conduit,diameter);
    }

    /// <summary>Creates a new instance of conduit.</summary>
    /// <remarks>This method will regenerate the document.</remarks>
    /// <param name="conduitType">
    ///    The conduit type.  This must be a conduit type accepted by isValidConduitType().
    ///    If the input conduit type is InvalidElementId, the default conduit type from the document will be used.
    /// </param>
    /// <param name="firstConnector">The first connector to get start point</param>
    /// <param name="secondConnector">The second connector to get endpoint</param>
    /// <param name="level">
    ///    The element of the level which this conduit based.
    ///    If the input level id is invalidElementId = -1, the nearest level will be used.
    /// </param>
    /// <returns>The newly created conduit.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    This conduit type is invalid.
    ///    -or-
    ///    This level id is invalid.
    ///    -or-
    ///    The points of startPoint and endPoint are too close: for MEPCurve, the minimum length is 1/10 inch.
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
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Conduit.CreateByTwoConnector.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreateByTwoConnector(Revit.Elements.Element conduitType,
        Autodesk.Revit.DB.Connector firstConnector, Autodesk.Revit.DB.Connector secondConnector,
        Revit.Elements.Element level)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.ForceCloseTransaction();
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Electrical.Conduit conduit = Autodesk.Revit.DB.Electrical.Conduit.Create(doc,
            new ElementId(conduitType.Id), firstConnector.Origin,
            secondConnector.Origin, new ElementId(level.Id));
        TransactionManager.Instance.TransactionTaskDone();
        return conduit.ToDynamoType();
    }

    /// <summary>Creates a new instance of conduit.</summary>
    /// <remarks>This method will regenerate the document.</remarks>
    /// <param name="conduitType">
    ///    The conduit type.  This must be a conduit type accepted by isValidConduitType().
    ///    If the input conduit type is InvalidElementId, the default conduit type from the document will be used.
    /// </param>
    /// <param name="firstConnector">The first connector to get start point</param>
    /// <param name="secondConnector">The second connector to get endpoint</param>
    /// <param name="level">
    ///    The element of the level which this conduit based.
    ///    If the input level id is invalidElementId = -1, the nearest level will be used.
    /// </param>
    /// <param name="diameter">the value diameter of conduit</param>
    /// <returns>The newly created conduit.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    This conduit type is invalid.
    ///    -or-
    ///    This level id is invalid.
    ///    -or-
    ///    The points of startPoint and endPoint are too close: for MEPCurve, the minimum length is 1/10 inch.
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
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Conduit.CreateByTwoConnectorDiameter.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreateByTwoConnector(Revit.Elements.Element conduitType,
        Autodesk.Revit.DB.Connector firstConnector, Autodesk.Revit.DB.Connector secondConnector,
        Revit.Elements.Element level,double diameter)
    {
        var conduit = CreateByTwoConnector(conduitType, firstConnector, secondConnector, level, diameter);
        return SetDiameter(conduit,diameter);
    }

    /// <summary>
    /// Create a conduit by line
    /// </summary>
    /// <param name="conduitType">
    ///    The conduit type.  This must be a conduit type accepted by isValidConduitType().
    ///    If the input conduit type is InvalidElementId, the default conduit type from the document will be used.
    /// </param>
    /// <param name="line">the line define to draw conduit from start point to end point</param>
    /// <param name="level">the element of level</param>
    ///<returns name="conduit">new conduit</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Conduit.CreateByLine.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreateByLine(Revit.Elements.Element conduitType,
        Autodesk.DesignScript.Geometry.Line line, Revit.Elements.Element level)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.ForceCloseTransaction();
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Electrical.Conduit conduit = Autodesk.Revit.DB.Electrical.Conduit.Create(doc,
            new ElementId(conduitType.Id), line.StartPoint.ToRevitType(),
            line.EndPoint.ToRevitType(), new ElementId(level.Id));
        TransactionManager.Instance.TransactionTaskDone();
        return conduit.ToDynamoType();
    }

    ///  <summary>
    ///  Create a conduit by line
    ///  </summary>
    ///  <param name="conduitType">
    ///     The conduit type.  This must be a conduit type accepted by isValidConduitType().
    ///     If the input conduit type is InvalidElementId, the default conduit type from the document will be used.
    ///  </param>
    ///  <param name="line">the line define to draw conduit from start point to end point</param>
    ///  <param name="level">the element of level</param>
    ///  <param name="diameter">the value diameter of conduit</param>
    ///  <returns name="conduit">new conduit</returns>
    ///  <example>
    ///  ![](../OpenMEPPage/element/dyn/pic/Conduit.CreateByLineDiameter.png)
    ///  </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreateByLine(Revit.Elements.Element conduitType,
        Autodesk.DesignScript.Geometry.Line line, Revit.Elements.Element level,double diameter)
    {
        var conduit = CreateByLine(conduitType, line, level);
        return SetDiameter(conduit, diameter);
    }

    /// <summary>
    /// Set diameter of conduit
    /// </summary>
    /// <param name="conduit">conduit need to set</param>
    /// <param name="diameter">diameter to set</param>
    /// <returns name="element">conduit</returns>
    /// <exception cref="Exception"></exception>
    ///  <example>
    ///  ![](../OpenMEPPage/element/dyn/pic/Conduit.SetDiameter.png)
    ///  </example>
    public static Revit.Elements.Element? SetDiameter(Revit.Elements.Element? conduit, double diameter)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Electrical.Conduit? internalElement =
            conduit!.InternalElement as Autodesk.Revit.DB.Electrical.Conduit;
        if (internalElement == null) throw new Exception("element request input is conduit");
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_Electrical_ConduitSize).DisplayUnits;
        double value = UnitUtils.ConvertToInternalUnits(diameter, unitTypeId);
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            doc.GetUnits().GetFormatOptions(SpecTypeId.ConduitSize).GetUnitTypeId();
        double value = UnitUtils.ConvertToInternalUnits(diameter, unitTypeId);
#endif

        ConnectorManager.Connector.GetConnectors(conduit).ForEach(delegate(Connector c) { c!.Radius = value / 2; });
        TransactionManager.Instance.TransactionTaskDone();
        return conduit;
    }
}