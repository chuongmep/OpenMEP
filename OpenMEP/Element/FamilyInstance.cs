using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using OpenMEPSandbox.Geometry;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;

namespace OpenMEP.Element;

public class FamilyInstance
{
    private FamilyInstance()
    {
    }
    /// <summary>
    /// Retrieves the MEP model for the family instance.</summary>
    /// <remarks>If the family instance has a MEP model it is returned by this method, otherwise <see langword="null" /> is
    /// returned. Different types of MEP model will be returned based on the type of the instance, for
    /// example - if the instance is a lighting device then a lighting device model will be returned.
    /// This property will only function with the Autodesk Revit MEP product.</remarks>
    /// <param name="familyInstance">the element to get MepModel</param>
    /// <returns name="mepModel">Autodesk.Revit.DB.MEPModel</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/FamilyInstance.GetMEPModel.png)
    /// </example>
    public static Autodesk.Revit.DB.MEPModel? GetMEPModel(global::Revit.Elements.Element familyInstance)
    {
        Autodesk.Revit.DB.Element internalElement = familyInstance.InternalElement;
        if (internalElement is Autodesk.Revit.DB.FamilyInstance fam)
        {
            return fam.MEPModel;
        }

        return null;
    }

    /// <summary>
    /// Shows scalable lines representing the CoordinateSystem of family instance axes and rectangles for the planes
    /// </summary>
    /// <param name="familyInstance">the family instance</param>
    /// <param name="length">double</param>
    /// <returns name="Display">GeometryColor</returns>
    /// <returns name="Origin">Point</returns>
    /// <returns name="XAxis">Vector</returns>
    /// <returns name="YAxis">Vector</returns>
    /// <returns name="ZAxis">Vector</returns>
    /// <returns name="XYPlane">Plane</returns>
    /// <returns name="YZPlane">Plane</returns>
    /// <returns name="ZXPlane">Plane</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/FamilyInstance.Display.png)
    /// </example>
    [MultiReturn(new[] {"Display", "Origin", "XAxis", "YAxis", "ZAxis", "XYPlane", "YZPlane", "ZXPlane"})]
    public static Dictionary<string, object?> Display(Revit.Elements.Element familyInstance, double length = 1000)
    {
        Autodesk.Revit.DB.FamilyInstance? internalElement =
            familyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
        if (internalElement == null) throw new ArgumentNullException(nameof(familyInstance));
        Transform transform = internalElement.GetTotalTransform();
        return CoordinateSystem.Display(transform.ToCoordinateSystem(), length);
    }
}