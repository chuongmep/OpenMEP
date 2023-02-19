using System.Globalization;
using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;

namespace OpenMEP.Element;

public class CableTray
{
    private CableTray()
    {
    }

#if !R20
     /// <summary>
    /// Creates a new instance of cable tray by start point and end point
    /// </summary>
    /// <param name="cableTrayType">The cable tray type. This must be a cable tray type accepted by isValidCableTrayType(). If the input cable tray type is InvalidElementId, the default cable tray type from the document will be used.</param>
    /// <param name="startPoint">The start point of the cable tray location line</param>
    /// <param name="endPoint">The end point of the cable tray location line</param>
    /// <param name="level">The element id of the level which this cable tray based. If the input level id is invalidElementId = -1, the nearest level will be used.</param>
    /// <param name="width">with of cable tray</param>
    /// <param name="height">height of cable tray</param>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? Create(global::Revit.Elements.Element cableTrayType,
        Autodesk.DesignScript.Geometry.Point startPoint, Autodesk.DesignScript.Geometry.Point endPoint,
        global::Revit.Elements.Level level, double width, double height)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Electrical.CableTray familyInstance = Autodesk.Revit.DB.Electrical.CableTray.Create(doc,
            new ElementId(cableTrayType.Id), startPoint.ToRevitType(true), endPoint.ToRevitType(true),
            new ElementId(level.Id));
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            familyInstance.Document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();
        double realWidth = UnitUtils.ConvertFromInternalUnits(width, unitTypeId);
        double realHeight = UnitUtils.ConvertFromInternalUnits(height, unitTypeId);
        familyInstance.get_Parameter(BuiltInParameter.RBS_CABLETRAY_WIDTH_PARAM)
            .SetValueString(realWidth.ToString(CultureInfo.InvariantCulture));
        familyInstance.get_Parameter(BuiltInParameter.RBS_CABLETRAY_HEIGHT_PARAM)
            .SetValueString(realHeight.ToString(CultureInfo.InvariantCulture));
        TransactionManager.Instance.TransactionTaskDone();
        return familyInstance.ToDynamoType();
    }
    
    /// <summary>
    /// create new cable tray by line
    /// </summary>
    /// <param name="cableTrayType">The cable tray type. This must be a cable tray type accepted by isValidCableTrayType(). If the input cable tray type is InvalidElementId, the default cable tray type from the document will be used.</param>
    /// <param name="line">line defined draw pipe</param>
    /// <param name="level">level place cable tray</param>
    /// <param name="width">with value of cable tray</param>
    /// <param name="height">height value of cable tray</param>
    /// <returns></returns>
    [NodeCategory("Create")]
    public static global::Revit.Elements.Element? Create(global::Revit.Elements.Element cableTrayType,
        Autodesk.DesignScript.Geometry.Line line,
        global::Revit.Elements.Level level, double width, double height)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        TransactionManager.Instance.EnsureInTransaction(doc);
        Autodesk.Revit.DB.Electrical.CableTray familyInstance = Autodesk.Revit.DB.Electrical.CableTray.Create(doc,
            new ElementId(cableTrayType.Id), line.StartPoint.ToXyz(), line.EndPoint.ToXyz(),
            new ElementId(level.Id));
        Autodesk.Revit.DB.ForgeTypeId unitTypeId =
            familyInstance.Document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();
        double realWidth = UnitUtils.ConvertFromInternalUnits(width, unitTypeId);
        double realHeight = UnitUtils.ConvertFromInternalUnits(height, unitTypeId);
        familyInstance.get_Parameter(BuiltInParameter.RBS_CABLETRAY_WIDTH_PARAM)
            .SetValueString(realWidth.ToString(CultureInfo.InvariantCulture));
        familyInstance.get_Parameter(BuiltInParameter.RBS_CABLETRAY_HEIGHT_PARAM)
            .SetValueString(realHeight.ToString(CultureInfo.InvariantCulture));
        TransactionManager.Instance.TransactionTaskDone();
        return familyInstance.ToDynamoType();
    }
#endif
    
}