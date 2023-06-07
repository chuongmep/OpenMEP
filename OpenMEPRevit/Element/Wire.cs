using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using OpenMEPRevit.Helpers;
using RevitServices.Persistence;
using RevitServices.Transactions;
using Point = Autodesk.DesignScript.Geometry.Point;

namespace OpenMEPRevit.Element;
/// <summary>Electrical wire element.</summary>
public class Wire
{
    private Wire()
    {
    }

    /// <summary>Appends one vertex to the end of the wire.</summary>
    /// <param name="wire">the wire</param>
    /// <param name="vertexPoint">The vertex to be appended.</param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The vertex point cannot be added to the wire because there is already a vertex at this position on the view plane (within tolerance).
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    ///    The end point is already connected to an element, so a new endpoint vertex cannot be appended.
    /// </exception>
    /// <since>2015</since>
    /// <returns name="wire">wire</returns>
    public static Revit.Elements.Element AppendVertex(Revit.Elements.Element wire,
        Autodesk.DesignScript.Geometry.Point vertexPoint)
    {
        if (wire == null) throw new ArgumentNullException(nameof(wire));
        if (vertexPoint == null) throw new ArgumentNullException(nameof(vertexPoint));
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Electrical.Wire? internalElement = wire.InternalElement as Autodesk.Revit.DB.Electrical.Wire;
        internalElement!.AppendVertex(vertexPoint.ToRevitType());
        TransactionManager.Instance.TransactionTaskDone();
        return wire;
    }

    /// <summary>Checks if the given vertex points are valid for the wire.</summary>
    /// <remarks>X and Y values are compared of the vertices.</remarks>
    /// <param name="vertexPoints">The vertex points.</param>
    /// <param name="startConnector">The start connector of the wire.</param>
    /// <param name="endConnector">The end connector of the wire.</param>
    /// <returns>True if the given vertex points are valid, false otherwise.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <since>2015</since>
    /// <returns name="bool">true if vertex points is valid</returns>
    [NodeCategory("Query")]
    public static bool AreVertexPointsValid(List<Autodesk.DesignScript.Geometry.Point> vertexPoints,
        Autodesk.Revit.DB.Connector startConnector, Autodesk.Revit.DB.Connector endConnector)
    {
        if (vertexPoints == null) throw new ArgumentNullException(nameof(vertexPoints));
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        return Autodesk.Revit.DB.Electrical.Wire.AreVertexPointsValid(
            vertexPoints.Select(x => x.ToRevitType()).ToList(), startConnector, endConnector);
    }

    /// <summary>Connects the wire to other elements.</summary>
    /// <param name="wire">the wire</param>
    /// <param name="startConnectorTo">
    ///    The connector that the start connector of the wire connects to.
    /// </param>
    /// <param name="endConnectorTo">
    ///    The connector that the end connector of the wire connects to.
    /// </param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    startConnectorTo cannot be connected to a wire, as it is not an electrical connector.
    ///    -or-
    ///    endConnectorTo cannot be connected to a wire, as it is not an electrical connector.
    ///    -or-
    ///    startConnectorTo or/and endConnectorTo cannot be connected to a wire, as wire can't connect both connectors to same wire or same connector.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    ///    Cannot connect the wire to the start connector or the end connector.
    /// </exception>
    /// <since>2015</since>
    public static void ConnectTo(Revit.Elements.Element wire, Autodesk.Revit.DB.Connector startConnectorTo,
        Autodesk.Revit.DB.Connector endConnectorTo)
    {
        if (wire == null) throw new ArgumentNullException(nameof(wire));
        if (startConnectorTo == null) throw new ArgumentNullException(nameof(startConnectorTo));
        if (endConnectorTo == null) throw new ArgumentNullException(nameof(endConnectorTo));
        Autodesk.Revit.DB.Electrical.Wire? internalElement = wire.InternalElement as Autodesk.Revit.DB.Electrical.Wire;
        TransactionManager.Instance.EnsureInTransaction(DocumentManager.Instance.CurrentDBDocument);
        internalElement?.ConnectTo(startConnectorTo, endConnectorTo);
        TransactionManager.Instance.TransactionTaskDone();
    }


    /// <summary>Gets the systems to which the wire belongs.</summary>
    ///<para name="wire">the wire</para>
    /// <remarks>One wire might belong to more than one circuit.</remarks>
    /// <returns>The systems to which the wire belongs.</returns>
    /// <since>2016 Subscription Update</since>
    /// <returns name="mepSystems">Mep Systems</returns>
    public static IEnumerable<Revit.Elements.Element?> GetMEPSystems(Revit.Elements.Element wire)
    {
        if (wire == null) throw new ArgumentNullException(nameof(wire));
        Autodesk.Revit.DB.Electrical.Wire? internalElement = wire.InternalElement as Autodesk.Revit.DB.Electrical.Wire;
        IList<ElementId>? mepSystems = internalElement?.GetMEPSystems();
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        if (mepSystems != null)
            foreach (ElementId mepSystem in mepSystems)
            {
                if (mepSystem != null)
                {
                    yield return doc.GetElement(mepSystem).ToDynamoType();
                }
            }
    }

    /// <summary>Gets the position of an existing vertex.</summary>
    /// <param name="wire">the wire</param>
    /// <param name="index">
    ///    The index of the existing vertex. Should be between 0 and <see cref="P:Autodesk.Revit.DB.Electrical.Wire.NumberOfVertices" />.
    /// </param>
    /// <returns>
    ///    The position of the vertex.
    ///    It is the offset point for the start and end vertex, not the connector point.
    ///    If the wire connects to one device, it may have offset; otherwise, the start and end vertex is same as the connector point.
    /// </returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The index should be between 0 and the number of vertices of the wire.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <since>2015</since>
    /// <returns name="point">vertex</returns>
    public static Point? GetVertex(Revit.Elements.Element wire, double index)
    {
        if (wire == null) throw new ArgumentNullException(nameof(wire));
        Autodesk.Revit.DB.Electrical.Wire? internalElement = wire.InternalElement as Autodesk.Revit.DB.Electrical.Wire;
        internalElement?.GetVertex((int) index);
        return internalElement?.GetVertex((int) index).ToDynamoType();
    }

    /// <summary>Inserts a new vertex before the specified index.</summary>
    /// <remarks>
    ///    To add a new vertex to the end of the wire, use <see cref="M:Autodesk.Revit.DB.Electrical.Wire.AppendVertex(Autodesk.Revit.DB.XYZ)" />.
    /// </remarks>
    /// <param name="wire">the wire</param>
    /// <param name="index">
    ///    The index of the vertex to come after this new vertex. Should be between 0 and <see cref="P:Autodesk.Revit.DB.Electrical.Wire.NumberOfVertices" />.
    /// </param>
    /// <param name="vertexPoint">The point of the new vertex.</param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The index should be between 0 and the number of vertices of the wire.
    ///    -or-
    ///    The vertex point cannot be added to the wire because there is already a vertex at this position on the view plane (within tolerance).
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    ///    Can't insert the vertex before the start vertex if the start point connects to one element.
    /// </exception>
    /// <since>2015</since>
    [NodeCategory("Actions")]
    public static Point? InsertVertex(Revit.Elements.Element wire, double index,
        Autodesk.DesignScript.Geometry.Point vertexPoint)
    {
        if (wire == null) throw new ArgumentNullException(nameof(wire));
        Autodesk.Revit.DB.Electrical.Wire? internalElement = wire.InternalElement as Autodesk.Revit.DB.Electrical.Wire;
        TransactionManager.Instance.EnsureInTransaction(DocumentManager.Instance.CurrentDBDocument);
        internalElement?.InsertVertex((int) index, vertexPoint.ToRevitType());
        TransactionManager.Instance.TransactionTaskDone();
        return internalElement?.GetVertex((int) index).ToDynamoType();
    }

    /// <summary>Checks if the given vertex point can be added to this wire.</summary>
    /// <remarks>Vertices are projected to the view plane for comparison.</remarks>
    /// <param name="wire">the wire</param>
    /// <param name="vertexPoint">The vertex point.</param>
    /// <returns>
    ///    True if the vertex point can be added, false if the point cannot be added because there is already a vertex at this position on the view plane (within tolerance).
    /// </returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <since>2015</since>
    /// <returns name="bool">true if it is vertex point valid</returns>
    [NodeCategory("Query")]
    public static bool IsVertexPointValid(Revit.Elements.Element wire, Autodesk.DesignScript.Geometry.Point vertexPoint)
    {
        if (wire == null) throw new ArgumentNullException(nameof(wire));
        Autodesk.Revit.DB.Electrical.Wire? internalElement = wire.InternalElement as Autodesk.Revit.DB.Electrical.Wire;
        if (vertexPoint == null) throw new ArgumentNullException(nameof(vertexPoint));
        return internalElement != null && internalElement.IsVertexPointValid(vertexPoint.ToRevitType());
    }

    /// <summary>
    ///    Removes the vertex corresponding to the specified index.
    ///    Can not remove the start or end vertex if it already connects to other element.
    /// </summary>
    /// <param name="wire">the wire</param>
    /// <param name="index">The index which should be in [0, NumberOfVertices).</param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The index should be between 0 and the number of vertices of the wire.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    ///    The wire has only 2 vertices, so one cannot be removed.
    ///    -or-
    ///    Can't remove the vertex when the vertex is start or end point and the wire connects to one element.
    /// </exception>
    /// <since>2015</since>
    [NodeCategory("Actions")]
    public static void RemoveVertex(Revit.Elements.Element wire, double index)
    {
        if (wire == null) throw new ArgumentNullException(nameof(wire));
        Autodesk.Revit.DB.Electrical.Wire? internalElement = wire.InternalElement as Autodesk.Revit.DB.Electrical.Wire;
        TransactionManager.Instance.EnsureInTransaction(DocumentManager.Instance.CurrentDBDocument);
        internalElement?.RemoveVertex((int) index);
        TransactionManager.Instance.TransactionTaskDone();
    }

    /// <summary>
    ///    Sets the position of a given vertex.
    ///    If the vertex is start or end point, and the wire connects to electrical device, the wire end offset will be set according to the given vertex.
    ///    If the vertex is start or end point, and the wire connects to other wire, user can't set the vertex and exception will be thrown.
    ///    If the vertex is start or end point, and the wire connects to nothing, the vertex will be set as the given vertex.
    /// </summary>
    /// <param name="wire">the wire</param>
    /// <param name="index">
    ///    The index of the existing vertex. Should be between 0 and <see cref="P:Autodesk.Revit.DB.Electrical.Wire.NumberOfVertices" />.
    /// </param>
    /// <param name="vertexPoint">The new position for the vertex.</param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The index should be between 0 and the number of vertices of the wire.
    ///    -or-
    ///    The vertex point cannot be added to the wire because there is already a vertex at this position on the view plane (within tolerance).
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.DisabledDisciplineException">
    ///    None of the following disciplines is enabled: Mechanical Electrical Piping.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    ///    Can't set the vertex when the vertex is start or end point and the wire connects to other wire.
    /// </exception>
    /// <since>2015</since>
    [NodeCategory("Actions")]
    public static void SetVertex(Revit.Elements.Element wire, double index,
        Autodesk.DesignScript.Geometry.Point vertexPoint)
    {
        if (wire == null) throw new ArgumentNullException(nameof(wire));
        Autodesk.Revit.DB.Electrical.Wire? internalElement = wire.InternalElement as Autodesk.Revit.DB.Electrical.Wire;
        TransactionManager.Instance.EnsureInTransaction(DocumentManager.Instance.CurrentDBDocument);
        internalElement?.SetVertex((int) index, vertexPoint.ToRevitType());
        TransactionManager.Instance.TransactionTaskDone();
    }

    /// <summary>The ground conductor number. Its default value is zero after created.</summary>
    /// <param name="wire">the wire</param>
    /// <returns name="double">ground conductor number</returns>
    [NodeCategory("Query")]
    public static double GroundConductorNumber(Revit.Elements.Element wire)
    {
        if (wire == null) throw new ArgumentNullException(nameof(wire));
        Autodesk.Revit.DB.Electrical.Wire? internalElement = wire.InternalElement as Autodesk.Revit.DB.Electrical.Wire;
        return internalElement?.GroundConductorNum ?? 0;
    }

    /// <summary>The hot conductor number. Its default value is zero after created.</summary>
    /// <para name="wire">the wire</para>
    /// <returns name="double">hot conduct number</returns>
    [NodeCategory("Query")]
    public static double HotConductorNum(Revit.Elements.Element wire)
    {
        if (wire == null) throw new ArgumentNullException(nameof(wire));
        Autodesk.Revit.DB.Electrical.Wire? internalElement = wire.InternalElement as Autodesk.Revit.DB.Electrical.Wire;
        return internalElement?.HotConductorNum ?? 0;
    }

    /// <summary>The neutral conductor number. Its default value is zero after created.</summary>
    /// <para name="wire">the wire</para>
    /// <returns name="double">neutral conductor number</returns>
    [NodeCategory("Query")]
    public static double NeutralConductorNum(Revit.Elements.Element wire)
    {
        if (wire == null) throw new ArgumentNullException(nameof(wire));
        Autodesk.Revit.DB.Electrical.Wire? internalElement = wire.InternalElement as Autodesk.Revit.DB.Electrical.Wire;
        return internalElement?.NeutralConductorNum ?? 0;
    }
    
    /// <summary>The wiring type(arc or chamfer) for the wire.</summary>
    /// <remarks>
    ///    If the WiringType is arc, the shape of the wire depends on the number of points - it may be linear, a circular arc, or a spline.
    /// </remarks>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentOutOfRangeException">
    ///    When setting this property: A value passed for an enumeration argument is not a member of that enumeration
    /// </exception>
    /// <returns name="string">wiring type of wire</returns>
    [NodeCategory("Query")]
    public static string? WiringType(Revit.Elements.Element wire)
    {
        if (wire == null) throw new ArgumentNullException(nameof(wire));
        Autodesk.Revit.DB.Electrical.Wire? internalElement = wire.InternalElement as Autodesk.Revit.DB.Electrical.Wire;
        return internalElement?.WiringType.ToString();
    }
}