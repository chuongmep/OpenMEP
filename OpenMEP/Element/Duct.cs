using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;

namespace OpenMEP.Element;

/// <summary>A duct in the Autodesk Revit MEP product.</summary>
/// <remarks>The duct is only available in the Autodesk Revit MEP product.</remarks>
public class Duct
{
    private Duct()
    {
    }

    /// <summary>Creates a new duct that connects to two connectors.</summary>
    /// <remarks>
    ///    The new duct will have the same diameter and system type as the start connector. The creation will also connect the new duct
    ///    to two component who owns the specified connectors. If necessary, additional fitting(s) are included to make a valid connection.
    ///    If the new duct can not be connected to the next component (e.g., mismatched direction, no valid fitting, and etc), the new duct
    ///    will still be created at the specified connector position, and an InvalidOperationException is thrown.
    /// </remarks>
    /// <param name="ductType">The Element of the new duct type.</param>
    /// <param name="level">The level Element for the new duct.</param>
    /// <param name="startConnector">The first connector where the new duct starts.</param>
    /// <param name="endConnector">The second point of the new duct.</param>
    /// <returns name="element">The created duct.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The duct type ductTypeId is not valid duct type.
    ///    -or-
    ///    The ElementId levelId is not a Level.
    ///    -or-
    ///    The connector's domain is not Domain.â€‹DomainHvac.
    ///    -or-
    ///    The points of startConnector and endConnector are too close: for MEPCurve, the minimum length is 1/10 inch.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    ///    Thrown when the new duct fails to connect with the connector.
    /// </exception>
    /// <since>2017</since>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.CreateByTwoConnector.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreateByTwoConnector(global::Revit.Elements.Element ductType,
        global::Revit.Elements.Element level,
        Autodesk.Revit.DB.Connector startConnector, Autodesk.Revit.DB.Connector endConnector)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Mechanical.Duct duct = Autodesk.Revit.DB.Mechanical.Duct.Create(doc,
            new ElementId(ductType.Id), new ElementId(level.Id),
            startConnector, endConnector);
        TransactionManager.Instance.TransactionTaskDone();
        return duct.ToDynamoType();
    }

    /// <summary>Creates a new duct that connects to two connectors.</summary>
    /// <remarks>
    ///    The new duct will have the same diameter and system type as the start connector. The creation will also connect the new duct
    ///    to two component who owns the specified connectors. If necessary, additional fitting(s) are included to make a valid connection.
    ///    If the new duct can not be connected to the next component (e.g., mismatched direction, no valid fitting, and etc), the new duct
    ///    will still be created at the specified connector position, and an InvalidOperationException is thrown.
    /// </remarks>
    /// <param name="ductType">The Element of the new duct type.</param>
    /// <param name="level">The level Element for the new duct.</param>
    /// <param name="startConnector">The first connector where the new duct starts.</param>
    /// <param name="endConnector">The second point of the new duct.</param>
    /// <param name="width">new value width of duct</param>
    /// <param name="height">new value height of duct</param>
    /// <returns name="element">The created duct.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The duct type ductTypeId is not valid duct type.
    ///    -or-
    ///    The ElementId levelId is not a Level.
    ///    -or-
    ///    The connector's domain is not Domain.â€‹DomainHvac.
    ///    -or-
    ///    The points of startConnector and endConnector are too close: for MEPCurve, the minimum length is 1/10 inch.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    ///    Thrown when the new duct fails to connect with the connector.
    /// </exception>
    /// <since>2017</since>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.CreateByTwoConnectorSize.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreateByTwoConnector(global::Revit.Elements.Element ductType,
        global::Revit.Elements.Element level,
        Autodesk.Revit.DB.Connector startConnector, Autodesk.Revit.DB.Connector endConnector, double width,
        double height)
    {
        Revit.Elements.Element? element = CreateByTwoConnector(ductType, level, startConnector, endConnector);
        if (element != null) SetDiameter(element, width, height);
        return element;
    }


    /// <summary>Creates a new duct that connects to the connector.</summary>
    /// <remarks>
    ///    The new duct will have the same diameter and system type as the specified connector. The creation will also connect the new duct
    ///    to the component who owns the specified connector. If necessary, additional fitting(s) are included to make a valid connection.
    ///    If the new duct can not be connected to the next component (e.g., mismatched direction, no valid fitting, and etc), the new duct
    ///    will still be created at the specified connector position, and an InvalidOperationException is thrown.
    /// </remarks>
    /// <param name="ductType">The Element of the new duct type.</param>
    /// <param name="level">The level for the new duct.</param>
    /// <param name="startConnector">The first connector where the new duct starts.</param>
    /// <param name="endPoint">The second point of the new duct.</param>
    /// <returns name="element">The created duct.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The duct type ductTypeId is not valid duct type.
    ///    -or-
    ///    The ElementId levelId is not a Level.
    ///    -or-
    ///    The connector's domain is not Domain.â€‹DomainHvac.
    ///    -or-
    ///    The points of startConnector and endPoint are too close: for MEPCurve, the minimum length is 1/10 inch.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    ///    Thrown when the new duct fails to connect with the connector.
    /// </exception>
    /// <since>2017</since>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.CreateByConnectorAndPoint.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreateByConnectorAndPoint(global::Revit.Elements.Element ductType,
        global::Revit.Elements.Element level,
        Autodesk.Revit.DB.Connector startConnector, Autodesk.DesignScript.Geometry.Point endPoint)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Revit.Elements.Element? element = Autodesk.Revit.DB.Mechanical.Duct.Create(doc, new ElementId(ductType.Id),
            new ElementId(level.Id),
            startConnector, endPoint.ToXyz()).ToDynamoType();
        TransactionManager.Instance.TransactionTaskDone();
        return element;
    }

    /// <summary>Creates a new duct that connects to the connector.</summary>
    /// <remarks>
    ///    The new duct will have the same diameter and system type as the specified connector. The creation will also connect the new duct
    ///    to the component who owns the specified connector. If necessary, additional fitting(s) are included to make a valid connection.
    ///    If the new duct can not be connected to the next component (e.g., mismatched direction, no valid fitting, and etc), the new duct
    ///    will still be created at the specified connector position, and an InvalidOperationException is thrown.
    /// </remarks>
    /// <param name="ductType">The Element of the new duct type.</param>
    /// <param name="level">The level for the new duct.</param>
    /// <param name="startConnector">The first connector where the new duct starts.</param>
    /// <param name="endPoint">The second point of the new duct.</param>
    /// <param name="width">new value width of duct</param>
    /// <param name="height">new value height of duct</param>
    /// <returns name="element">The created duct.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The duct type ductTypeId is not valid duct type.
    ///    -or-
    ///    The ElementId levelId is not a Level.
    ///    -or-
    ///    The connector's domain is not Domain.â€‹DomainHvac.
    ///    -or-
    ///    The points of startConnector and endPoint are too close: for MEPCurve, the minimum length is 1/10 inch.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    ///    Thrown when the new duct fails to connect with the connector.
    /// </exception>
    /// <since>2017</since>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.CreateByConnectorAndPointSize.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreateByConnectorAndPoint(global::Revit.Elements.Element ductType,
        global::Revit.Elements.Element level,
        Autodesk.Revit.DB.Connector startConnector, Autodesk.DesignScript.Geometry.Point endPoint, double width,
        double height)
    {
        Revit.Elements.Element? element = CreateByConnectorAndPoint(ductType, level, startConnector, endPoint);
        if (element != null) SetDiameter(element, width, height);
        return element;
    }
    
   

    /// <summary>Creates a new duct from two points.</summary>
    /// <param name="systemType">The element of the HVAC system type.</param>
    /// <param name="ductType">The element of the duct type.</param>
    /// <param name="level">The level for the duct.</param>
    /// <param name="startPoint">The start point of the duct.</param>
    /// <param name="endPoint">The end point of the duct.</param>
    /// <returns name="element">The created duct.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The systemType is not valid HVAC system type.
    ///    -or-
    ///    The duct type ductType is not valid duct type.
    ///    -or-
    ///    The Element level is not a Level.
    ///    -or-
    ///    The points of startPoint and endPoint are too close: for MEPCurve, the minimum length is 1/10 inch.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <since>2014</since>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.CreateByTwoPoint.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreateByTwoPoint(global::Revit.Elements.Element systemType,
        global::Revit.Elements.Element ductType, global::Revit.Elements.Element level,
        Autodesk.DesignScript.Geometry.Point startPoint, Autodesk.DesignScript.Geometry.Point endPoint)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Revit.Elements.Element? duct = Autodesk.Revit.DB.Mechanical.Duct.Create(doc, new ElementId(systemType.Id),
            new ElementId(ductType.Id), new ElementId(level.Id),
            startPoint.ToXyz(), endPoint.ToXyz()).ToDynamoType();
        TransactionManager.Instance.TransactionTaskDone();
        return duct;
    }

    /// <summary>Creates a new duct from two points.</summary>
    /// <param name="systemType">The element of the HVAC system type.</param>
    /// <param name="ductType">The element of the duct type.</param>
    /// <param name="level">The level for the duct.</param>
    /// <param name="startPoint">The start point of the duct.</param>
    /// <param name="endPoint">The end point of the duct.</param>
    /// <param name="width">new value width of duct</param>
    /// <param name="height">new value height of duct</param>
    /// <returns name="element">The created duct.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The systemType is not valid HVAC system type.
    ///    -or-
    ///    The duct type ductType is not valid duct type.
    ///    -or-
    ///    The Element level is not a Level.
    ///    -or-
    ///    The points of startPoint and endPoint are too close: for MEPCurve, the minimum length is 1/10 inch.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <since>2014</since>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.CreateByTwoPointSize.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreateByTwoPoint(global::Revit.Elements.Element systemType,
        global::Revit.Elements.Element ductType, global::Revit.Elements.Element level,
        Autodesk.DesignScript.Geometry.Point startPoint, Autodesk.DesignScript.Geometry.Point endPoint, double width,
        double height)
    {
        Revit.Elements.Element? element = CreateByTwoPoint(systemType, ductType, level, startPoint, endPoint);
        if (element != null) SetDiameter(element, width, height);
        return element;
    }

    /// <summary>Creates a new duct from Line.</summary>
    /// <param name="systemType">The element of the HVAC system type.</param>
    /// <param name="ductType">The element of the duct type.</param>
    /// <param name="level">The level for the duct.</param>
    /// <param name="line">the line to draw new duct</param>
    /// <param name="width">new value width of duct</param>
    /// <param name="height">new value height of duct</param>
    /// <returns name="element">The created duct.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The systemType is not valid HVAC system type.
    ///    -or-
    ///    The duct type ductType is not valid duct type.
    ///    -or-
    ///    The Element level is not a Level.
    ///    -or-
    ///    The points of startPoint and endPoint are too close: for MEPCurve, the minimum length is 1/10 inch.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <since>2014</since>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.CreateByTwoPointSize.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreateByTwoPoint(global::Revit.Elements.Element systemType,
        global::Revit.Elements.Element ductType, global::Revit.Elements.Element level,
        Autodesk.DesignScript.Geometry.Line line, double width,
        double height)
    {
        Revit.Elements.Element? element = CreateByTwoPoint(systemType, ductType, level, line.StartPoint, line.EndPoint);
        if (element != null) SetDiameter(element, width, height);
        return element;
    }

    /// <summary>Creates a new placeholder duct.</summary>
    /// <param name="systemType">The element of the HVAC system type.</param>
    /// <param name="ductType">The element of the duct type.</param>
    /// <param name="level">The element level for the duct.</param>
    /// <param name="startPoint">The first point of the placeholder line.</param>
    /// <param name="endPoint">The second point of the placeholder line.</param>
    /// <returns name="element">The created placeholder duct.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The systemType is not valid HVAC system type.
    ///    -or-
    ///    The ductType is not valid duct type.
    ///    -or-
    ///    The Element level is not a Level.
    ///    -or-
    ///    The points of startPoint and endPoint are too close: for MEPCurve, the minimum length is 1/10 inch.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <since>2014</since>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.CreatePlaceholderByTwoPoint.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreatePlaceholder(global::Revit.Elements.Element systemType,
        global::Revit.Elements.Element ductType, global::Revit.Elements.Element level,
        Autodesk.DesignScript.Geometry.Point startPoint, Autodesk.DesignScript.Geometry.Point endPoint)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Mechanical.Duct duct = Autodesk.Revit.DB.Mechanical.Duct.CreatePlaceholder(doc,
            new ElementId(systemType.Id), new ElementId(ductType.Id), new ElementId(level.Id),
            startPoint.ToXyz(), endPoint.ToXyz());
        TransactionManager.Instance.TransactionTaskDone();
        return duct.ToDynamoType();
    }

    /// <summary>Creates a new placeholder duct.</summary>
    /// <param name="systemType">The element of the HVAC system type.</param>
    /// <param name="ductType">The element of the duct type.</param>
    /// <param name="level">The element level for the duct.</param>
    /// <param name="line">the line to draw duct from start point to end point</param>
    /// <returns name="element">The created placeholder duct.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The systemType is not valid HVAC system type.
    ///    -or-
    ///    The ductType is not valid duct type.
    ///    -or-
    ///    The Element level is not a Level.
    ///    -or-
    ///    The points of startPoint and endPoint are too close: for MEPCurve, the minimum length is 1/10 inch.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <since>2014</since>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.CreatePlaceholderByLine.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreatePlaceholder(global::Revit.Elements.Element systemType,
        global::Revit.Elements.Element ductType, global::Revit.Elements.Element level,
        Autodesk.DesignScript.Geometry.Line line)
    {
        return CreatePlaceholder(systemType, ductType, level, line.StartPoint, line.EndPoint);
    }

    /// <summary>Creates a new placeholder duct.</summary>
    /// <param name="systemType">The element of the HVAC system type.</param>
    /// <param name="ductType">The element of the duct type.</param>
    /// <param name="level">The element level for the duct.</param>
    /// <param name="width">new value width of duct</param>
    /// <param name="height">new value height of duct</param>
    /// <param name="startPoint">The first point of the placeholder line.</param>
    /// <param name="endPoint">The second point of the placeholder line.</param>
    /// <returns name="element">The created placeholder duct.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The systemType is not valid HVAC system type.
    ///    -or-
    ///    The ductType is not valid duct type.
    ///    -or-
    ///    The Element level is not a Level.
    ///    -or-
    ///    The points of startPoint and endPoint are too close: for MEPCurve, the minimum length is 1/10 inch.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <since>2014</since>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.CreatePlaceholderByTwoPointSize.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreatePlaceholder(global::Revit.Elements.Element systemType,
        global::Revit.Elements.Element ductType, global::Revit.Elements.Element level,
        Autodesk.DesignScript.Geometry.Point startPoint, Autodesk.DesignScript.Geometry.Point endPoint, double width,
        double height)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        var placeholder = Autodesk.Revit.DB.Mechanical.Duct.CreatePlaceholder(doc, new ElementId(systemType.Id),
            new ElementId(ductType.Id), new ElementId(level.Id), startPoint.ToXyz(), endPoint.ToXyz());
        if (placeholder != null) SetDiameter(placeholder.ToDynamoType(), width, height);
        TransactionManager.Instance.TransactionTaskDone();
        return placeholder.ToDynamoType();
    }

    /// <summary>Creates a new placeholder duct.</summary>
    /// <param name="systemType">The element of the HVAC system type.</param>
    /// <param name="ductType">The element of the duct type.</param>
    /// <param name="level">The element level for the duct.</param>
    /// <param name="line">the line to draw duct from start point to end point</param>
    /// <param name="width">new value width of duct</param>
    /// <param name="height">new value height of duct</param>
    /// <returns name="element">The created placeholder duct.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The systemType is not valid HVAC system type.
    ///    -or-
    ///    The ductType is not valid duct type.
    ///    -or-
    ///    The Element level is not a Level.
    ///    -or-
    ///    The points of startPoint and endPoint are too close: for MEPCurve, the minimum length is 1/10 inch.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <since>2014</since>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.CreatePlaceholderByLineSize.png)
    /// </example>
    [NodeCategory("Create")]
    public static Revit.Elements.Element? CreatePlaceholder(global::Revit.Elements.Element systemType,
        global::Revit.Elements.Element ductType, global::Revit.Elements.Element level,
        Autodesk.DesignScript.Geometry.Line line, double width,
        double height)
    {
        Revit.Elements.Element? element = CreatePlaceholder(systemType, ductType, level, line.StartPoint, line.EndPoint);
        if (element != null) SetDiameter(element, width, height);
        return element;
    }

    /// <summary>
    /// Set new diameter for rectangular duct
    /// </summary>
    /// <param name="duct">the duct to set diameter</param>
    /// <param name="width">new value width of duct</param>
    /// <param name="height">new value height of duct</param>
    /// <returns name="element">duct with new parameter diameter</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.SetDiameterSize.png)
    /// </example>
    public static Revit.Elements.Element? SetDiameter(Revit.Elements.Element? duct, double width, double height)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Mechanical.Duct? internalElement = duct.InternalElement as Autodesk.Revit.DB.Mechanical.Duct;
        // Set the diameter of the duct. 
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_HVAC_DuctSize).DisplayUnits;
        double widthValue = UnitUtils.ConvertToInternalUnits(width, unitTypeId);
        double heightValue = UnitUtils.ConvertToInternalUnits(height, unitTypeId);
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.DuctSize).GetUnitTypeId();
        double widthValue = UnitUtils.ConvertToInternalUnits(width, unitTypeId);
        double heightValue = UnitUtils.ConvertToInternalUnits(height, unitTypeId);
#endif
        internalElement?.get_Parameter(BuiltInParameter.RBS_CURVE_WIDTH_PARAM)?.Set(widthValue);
        internalElement?.get_Parameter(BuiltInParameter.RBS_CURVE_HEIGHT_PARAM)?.Set(heightValue);
        TransactionManager.Instance.TransactionTaskDone();
        return duct;
    }

    /// <summary>
    /// Set new diameter for round duct
    /// </summary>
    /// <param name="duct">the duct to set diameter</param>
    /// <param name="diameter">new value diameter of duct</param>
    /// <returns name="element">duct with new parameter diameter</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.SetDiameter.png)
    /// </example>
    public static Revit.Elements.Element? SetDiameter(Revit.Elements.Element duct, double diameter)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Mechanical.Duct? internalElement = duct.InternalElement as Autodesk.Revit.DB.Mechanical.Duct;
        // Set the diameter of the duct. 
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_HVAC_DuctSize).DisplayUnits;
        double diameterValue = UnitUtils.ConvertToInternalUnits(diameter, unitTypeId);
#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.DuctSize).GetUnitTypeId();
        double diameterValue = UnitUtils.ConvertToInternalUnits(diameter, unitTypeId);
#endif
        internalElement?.get_Parameter(BuiltInParameter.RBS_CURVE_DIAMETER_PARAM)?.Set(diameterValue);
        TransactionManager.Instance.TransactionTaskDone();
        return duct;
    }

    /// <summary>
    /// return diameter of duct
    /// </summary>
    /// <param name="duct">the duct</param>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.GetDiameter.png)
    /// </example>
    [MultiReturn("width", "height", "diameter")]
    [NodeCategory("Query")]
    public static Dictionary<string, object?> GetDiameter(Revit.Elements.Element duct)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        doc.Regenerate();
        var internalElement = duct.InternalElement as Autodesk.Revit.DB.Mechanical.Duct;
        if (internalElement == null) throw new ArgumentNullException(nameof(duct));
#if R20
        DisplayUnitType unitTypeId = doc.GetUnits().GetFormatOptions(UnitType.UT_HVAC_DuctSize).DisplayUnits;
        double? width = internalElement.get_Parameter(BuiltInParameter.RBS_CURVE_WIDTH_PARAM)?.AsDouble();
        double? height = internalElement.get_Parameter(BuiltInParameter.RBS_CURVE_HEIGHT_PARAM)?.AsDouble();
        double? diameter = internalElement.get_Parameter(BuiltInParameter.RBS_CURVE_DIAMETER_PARAM)?.AsDouble();
        if (height == null || width == null  && diameter!=null)
        {
            diameter = UnitUtils.ConvertFromInternalUnits((double) diameter, unitTypeId);
        }

        if (height != null && width != null  && diameter==null)
        {
            width = UnitUtils.ConvertFromInternalUnits((double) width, unitTypeId);
            height = UnitUtils.ConvertFromInternalUnits((double) height, unitTypeId);
        }

#else
        Autodesk.Revit.DB.ForgeTypeId unitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.DuctSize).GetUnitTypeId();
        double? width = internalElement.get_Parameter(BuiltInParameter.RBS_CURVE_WIDTH_PARAM)?.AsDouble();
        double? height = internalElement.get_Parameter(BuiltInParameter.RBS_CURVE_HEIGHT_PARAM)?.AsDouble();
        double? diameter = internalElement.get_Parameter(BuiltInParameter.RBS_CURVE_DIAMETER_PARAM)?.AsDouble();
        if (height == null || width == null && diameter != null)
        {
            diameter = UnitUtils.ConvertFromInternalUnits((double) diameter, unitTypeId);
        }

        if (height != null && width != null && diameter == null)
        {
            width = UnitUtils.ConvertFromInternalUnits((double) width, unitTypeId);
            height = UnitUtils.ConvertFromInternalUnits((double) height, unitTypeId);
        }
#endif
        return new Dictionary<string, object?>()
        {
            {"width", width},
            {"height", height},
            {"diameter", diameter}
        };
    }

    /// <summary>Updates the associated system type for the duct.</summary>
    /// <remarks>
    ///    If the duct previously did not have a system associated to it, this will create a new system.
    /// </remarks>
    /// <param name="systemType">The Element of the hvac system type.</param>
    /// <param name="duct">The Element of the duct</param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The systemTypeId is not valid HVAC system type.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <since>2017</since>
    /// <returns name="duct">duct changed systemType</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.SetSystemType.png)
    /// </example>
    [NodeCategory("Action")]
    public static global::Revit.Elements.Element SetSystemType(global::Revit.Elements.Element duct,
        global::Revit.Elements.Element systemType)
    {
        Autodesk.Revit.DB.Mechanical.Duct? ductInternalElement =
            duct.InternalElement as Autodesk.Revit.DB.Mechanical.Duct;
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        ductInternalElement!.SetSystemType(new ElementId(systemType.Id));
        TransactionManager.Instance.TransactionTaskDone();
        return duct;
    }

    /// <summary>
    /// Get Shape Type of duct
    /// </summary>
    /// <param name="duct">the duct</param>
    /// <returns name="shapeType">shape type of duct</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.Shape.png)
    /// </example>
    [NodeCategory("Query")]
    public static dynamic Shape(Revit.Elements.Element duct)
    {
        if (duct == null) throw new ArgumentNullException(nameof(duct));
        Connector? connector = ConnectorManager.Connector.GetConnectors(duct).FirstOrDefault();
        if (connector == null) return string.Empty;
        return connector.Shape;
    }

    /// <summary>
    ///  Check if the element of duct is a valid system type
    /// </summary>
    /// <param name="systemType">the element of system type</param>
    /// <returns name="bool">true if is HvacSystemTypeId</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.IsHvacSystemTypeId.png)
    /// </example>
    [NodeCategory("Query")]
    public static bool IsHvacSystemTypeId(global::Revit.Elements.Element systemType)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        bool isHvacSystemTypeId =
            Autodesk.Revit.DB.Mechanical.Duct.IsHvacSystemTypeId(doc, new ElementId(systemType.Id));
        return isHvacSystemTypeId;
    }

    /// <summary>
    /// Check if the element of duct is a valid duct type
    /// </summary>
    /// <param name="systemType">the system type of duct</param>
    /// <returns name="bool">true if is duct type id</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/Duct.IsDuctTypeId.png)
    /// </example>
    [NodeCategory("Query")]
    public static bool IsDuctTypeId(global::Revit.Elements.Element systemType)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        return Autodesk.Revit.DB.Mechanical.Duct.IsDuctTypeId(doc, new ElementId(systemType.Id));
    }

    /// <summary>
    ///    Connects an air terminal to a duct directly (without the need for a tee or takeoff).
    /// </summary>
    /// <remarks>
    ///    The current location of the air terminal will be projected to the duct centerline, and if the point can be successfully projected,
    ///    the air terminal will be placed on the most suitable face of the duct.
    /// </remarks>
    /// <param name="airTerminal">The air terminal element.</param>
    /// <param name="duct">The duct curve element.</param>
    /// <returns name="bool">True if connection succeeds, false otherwise.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The familyinstance is not air terminal.
    ///    -or-
    ///    The element is not duct curve.
    ///    -or-
    ///    The air terminal already has physical connection.
    ///    -or-
    ///    The air terminal connector origin doesn't project within the center line of the duct.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <since>2014</since>
    public static bool ConnectAirTerminalOnDuct(Revit.Elements.Element airTerminal, Revit.Elements.Element duct)
    {
        if (airTerminal == null) throw new ArgumentNullException(nameof(airTerminal));
        if (duct == null) throw new ArgumentNullException(nameof(duct));
        var doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        bool result =
            Autodesk.Revit.DB.Mechanical.MechanicalUtils.ConnectAirTerminalOnDuct(doc, airTerminal.InternalElement.Id,
                duct.InternalElement.Id);
        TransactionManager.Instance.TransactionTaskDone();
        return result;
    }

    /// <summary>Converts a collection of duct placeholder elements into duct elements.</summary>
    /// <remarks>
    ///    Once conversion succeeds, the duct placeholder elements are deleted.
    ///    The new duct and fitting elements are created and connections are established.
    /// </remarks>
    /// <param name="ductPlaceholder">A collection of element of duct placeholder.</param>
    /// <returns>A collection of element IDs of ducts and fittings.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The given element id set is empty.
    ///    -or-
    ///    The given element IDs (placeholderIds) are not duct placeholders.
    ///    -or-
    ///    The elements belong to different types of system.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <since>2012</since>
    public static IEnumerable<Revit.Elements.Element?> ConvertPlaceholdersToDucts(Revit.Elements.Element ductPlaceholder)
    {
        var doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        ICollection<ElementId> elementIds =
            Autodesk.Revit.DB.Mechanical.MechanicalUtils.ConvertDuctPlaceholders(doc,
                new List<ElementId>() {ductPlaceholder.InternalElement.Id});
        foreach (ElementId elementId in elementIds)
        {
            yield return doc.GetElement(elementId).ToDynamoType();
        }
        TransactionManager.Instance.TransactionTaskDone();
    }
}