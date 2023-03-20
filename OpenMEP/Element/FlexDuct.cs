using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;
using Point = Autodesk.DesignScript.Geometry.Point;

namespace OpenMEP.Element;
/// <summary>A flex duct in the Autodesk Revit MEP product.</summary>
public class FlexDuct
{
    private FlexDuct()
    {
        
    }
    /// <summary>Adds a new flexible duct into the document,
    /// using two connector, and duct type.</summary>
    /// <param name="connector1"> The first connector to be connected to the duct. </param>
    /// <param name="connector2"> The second connector to be connected to the duct. </param>
    /// <param name="ductType"> The type of the flexible duct. </param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    /// Thrown when the input argument connector1 or connector2 is <see langword="null" />.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    /// Thrown when the flexible duct cannot be created or regenerate fails.
    /// </exception>
    /// <returns> If creation was successful then a new flexible duct is returned,
    /// otherwise an exception with failure information will be thrown.</returns>
    /// <remarks>If the connectors are fitting or equipment connectors of the correct domain,
    /// and if the connectors' direction match the direction of the flexible duct to be created,
    /// the connectors will be automatically connected. A transition fitting will be added
    /// at the connector(s) if necessary. If the connector's type, domain,
    /// does not match the one of the input connector, no connection will be established.</remarks>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">Thrown if the flexible duct type does not exist in the given document.</exception>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/FlexDuct.NewFlexDuctByTwoConnector.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? CreateByTwoConnector(global::Revit.Elements.Element ductType,Autodesk.Revit.DB.Connector connector1,Autodesk.Revit.DB.Connector connector2)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        FlexDuctType? flexDuctType = ductType.InternalElement as Autodesk.Revit.DB.Mechanical.FlexDuctType;
        Autodesk.Revit.DB.Mechanical.FlexDuct newFlexDuct = doc.Create.NewFlexDuct(connector1,connector2,flexDuctType);
        TransactionManager.Instance.TransactionTaskDone();
        if (newFlexDuct == null) return null;
        return newFlexDuct.ToDynamoType();
    }
    
    /// <overloads>Adds a new flexible duct into the document.</overloads>
    /// <summary>Adds a new flexible duct into the document,
    /// using a point array and duct type.</summary>
    /// <param name="points">The point array indicating the path of the flexible duct, including the end points.</param>
    /// <param name="ductType"> The type of the flexible duct. </param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    /// Thrown when the input argument points is <see langword="null" />.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    /// Thrown when the flexible duct cannot be created or regenerate fails.
    /// </exception>
    /// <returns> If creation was successful then a new flexible duct is returned,
    /// otherwise an exception with failure information will be thrown.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">Thrown if the flexible duct type does not exist in the given document.</exception>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/FlexDuct.NewFlexDuctByListPoint.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? CreateByListPoint(global::Revit.Elements.Element ductType,List<Autodesk.DesignScript.Geometry.Point> points)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        FlexDuctType? flexDuctType = ductType.InternalElement as Autodesk.Revit.DB.Mechanical.FlexDuctType;
        List<XYZ> xyzes = points.Select(x => x.ToXyz()).ToList();
        Autodesk.Revit.DB.Mechanical.FlexDuct newFlexDuct = doc.Create.NewFlexDuct(xyzes,flexDuctType);
        TransactionManager.Instance.TransactionTaskDone();
        if (newFlexDuct == null) return null;
        return newFlexDuct.ToDynamoType();
    }
    
    /// <summary>Adds a new flexible duct into the document,
    /// using a connector, point array and duct type.</summary>
    /// <param name="connector"> The connector to be connected to the duct, including the end points. </param>
    /// <param name="points">The point array indicating the path of the flexible duct.</param>
    /// <param name="ductType"> The type of the flexible duct. </param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    /// Thrown when the input argument connector or points is <see langword="null" />.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    /// Thrown when the flexible duct cannot be created or regenerate fails.
    /// </exception>
    /// <returns> If creation was successful then a new flexible duct is returned,
    /// otherwise an exception with failure information will be thrown.</returns>
    /// <remarks>If the connector is a fitting or equipment connector of the correct domain,
    /// and if the connector's direction matches the direction of the flexible duct to be created,
    /// the connectors will be automatically connected. A transition fitting will be added
    /// at the connector(s) if necessary. If the connector's type, domain,
    /// does not match the one of the input connector, no connection will be established.</remarks>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">Thrown if the flexible duct type does not exist in the given document.</exception>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/FlexDuct.NewFlexDuctByConnectorAndPoints.png)
    /// </example>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? CreateByConnectorAndPoints(Autodesk.Revit.DB.Connector connector,List<Autodesk.DesignScript.Geometry.Point> points,global::Revit.Elements.Element ductType)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        FlexDuctType? flexDuctType = ductType.InternalElement as Autodesk.Revit.DB.Mechanical.FlexDuctType;
        List<XYZ> xyzes = points.Select(x => x.ToXyz()).ToList();
        Autodesk.Revit.DB.Mechanical.FlexDuct newFlexDuct = doc.Create.NewFlexDuct(connector,xyzes,flexDuctType);
        TransactionManager.Instance.TransactionTaskDone();
        if (newFlexDuct == null) return null;
        return newFlexDuct.ToDynamoType();
    }

    /// <summary>The points of the flex duct.</summary>
    /// <remarks>This property is used to retrieve the points of flex duct, including the end points.
    /// If the end points are changed, the connection will be maintained by Revit automatically.
    /// The set operation will fail if the modification makes the connection invalid.
    ///<para name="flexDuct">flex Duct</para>
    /// </remarks>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/FlexDuct.Points.png)
    /// </example>
    [NodeCategory("Query")]
    public static IEnumerable<Point> Points(global::Revit.Elements.Element flexDuct)
    {
        Autodesk.Revit.DB.Mechanical.FlexDuct? internalElement = flexDuct.InternalElement as Autodesk.Revit.DB.Mechanical.FlexDuct;
        foreach (XYZ xyz in internalElement!.Points)
        {
            yield return xyz.ToPoint();
        }
    }
}