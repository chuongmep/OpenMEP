using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;

namespace OpenMEP.Element;

public class FlexPipe
{
    private FlexPipe()
    {
        
    }
    
    /// <summary>Adds a new flexible pipe into the document,
    /// using two connector, and flexible pipe type.</summary>
    /// <param name="connector"> The first connector to be connected to the pipe. </param>
    /// <param name="connector2"> The second connector to be connected to the pipe. </param>
    /// <param name="pipeType"> The type of the flexible pipe. </param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    /// Thrown when the input argument connector1 or connector2 is <see langword="null" />.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    /// Thrown when the flexible pipe cannot be created or regenerate fails.
    /// </exception>
    /// <returns> If creation was successful then a new flexible pipe is returned,
    /// otherwise an exception with failure information will be thrown.</returns>
    /// <remarks>If the connectors are fitting or equipment connectors of the correct domain,
    /// and if the connectors' direction match the direction of the flexible pipe to be created,
    /// the connectors will be automatically connected. A transition fitting will be added
    /// at the connector(s) if necessary. If the connector's type, domain,
    /// does not match the one of the input connectors, no connection will be established.</remarks>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">Thrown if the flexible pipe type does not exist in the given document.</exception>
    /// <returns name="element">flex pipe</returns>
    public static global::Revit.Elements.Element? NewFlexPipe(Autodesk.Revit.DB.Connector connector,Autodesk.Revit.DB.Connector connector2,global::Revit.Elements.Element pipeType)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Plumbing.FlexPipeType? flexPipeType = pipeType.InternalElement as Autodesk.Revit.DB.Plumbing.FlexPipeType;
        Autodesk.Revit.DB.Plumbing.FlexPipe newUnionFitting = doc.Create.NewFlexPipe(connector,connector2,flexPipeType);
        TransactionManager.Instance.TransactionTaskDone();
        if (newUnionFitting == null) return null;
        return newUnionFitting.ToDynamoType();
    }

    /// <summary>Adds a new flexible pipe into the document,
    /// using a connector, point array and pipe type.</summary>
    /// <param name="connector"> The connector to be connected to the flexible pipe, including the end points. </param>
    /// <param name="points">The point array indicating the path of the flexible pipe.</param>
    /// <param name="pipeType"> The type of the flexible pipe. </param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    /// Thrown when the input argument connector or points is <see langword="null" />.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    /// Thrown when the flexible pipe cannot be created or regenerate fails.
    /// </exception>
    /// <returns> If creation was successful then a new flexible pipe is returned,
    /// otherwise an exception with failure information will be thrown.</returns>
    /// <remarks>If the connector is a fitting or equipment connector of the correct domain,
    /// and if the connector's direction matches the direction of the flexible pipe to be created,
    /// the connectors will be automatically connected. A transition fitting will be added
    /// at the connector(s) if necessary. If the connector's type, domain,
    /// does not match the one of the input connector, no connection will be established.</remarks>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">Thrown if the flexible pipe type does not exist in the given document.</exception>
    /// <returns name="element">flex pipe</returns>
    public static global::Revit.Elements.Element? NewFlexPipe(Autodesk.Revit.DB.Connector connector,List<Autodesk.DesignScript.Geometry.Point> points,global::Revit.Elements.Element pipeType)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Plumbing.FlexPipeType? flexPipeType = pipeType.InternalElement as Autodesk.Revit.DB.Plumbing.FlexPipeType;
        List<XYZ> xyzes = points.Select(x => x.ToXyz()).ToList();
        Autodesk.Revit.DB.Plumbing.FlexPipe newUnionFitting = doc.Create.NewFlexPipe(connector,xyzes,flexPipeType);
        TransactionManager.Instance.TransactionTaskDone();
        if (newUnionFitting == null) return null;
        return newUnionFitting.ToDynamoType();
    }
    
    /// <overloads>Adds a new flexible pipe into the document.</overloads>
    /// <summary>Adds a new flexible pipe into the document,
    /// using a point array and pipe type.</summary>
    /// <param name="points">The point array indicating the path of the flexible pipe, including the end points.</param>
    /// <param name="pipeType"> The type of the flexible pipe. </param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    /// Thrown when the input argument points is <see langword="null" />.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    /// Thrown when the flexible pipe cannot be created or regenerate fails.
    /// </exception>
    /// <returns> If creation was successful then a new flexible pipe is returned,
    /// otherwise an exception with failure information will be thrown.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">Thrown if the flexible pipe type does not exist in the given document.</exception>
    /// <returns name="element">flex pipe</returns>
    public static global::Revit.Elements.Element? NewFlexPipe(List<Autodesk.DesignScript.Geometry.Point> points,global::Revit.Elements.Element pipeType)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Plumbing.FlexPipeType? flexPipeType = pipeType.InternalElement as Autodesk.Revit.DB.Plumbing.FlexPipeType;
        List<XYZ> xyzes = points.Select(x => x.ToXyz()).ToList();
        Autodesk.Revit.DB.Plumbing.FlexPipe newUnionFitting = doc.Create.NewFlexPipe(xyzes,flexPipeType);
        TransactionManager.Instance.TransactionTaskDone();
        if (newUnionFitting == null) return null;
        return newUnionFitting.ToDynamoType();
    }
    
    /// <summary>The points of the flex pipe.</summary>
    /// <remarks>This property is used to retrieve the points of flex pipe, including the end points.
    /// If the end points are changed, the connection will be maintained by Revit automatically.
    /// The set operation will fail if the modification makes the connection invalid.
    /// </remarks>
    ///<returns name="points">list point of flex pipe</returns>
    [NodeCategory("Query")]
    public static List<Autodesk.DesignScript.Geometry.Point> Points(Autodesk.Revit.DB.Plumbing.FlexPipe flexPipe)
    {
        return flexPipe.Points.Select(x=>x.ToPoint()).ToList();
    }
}