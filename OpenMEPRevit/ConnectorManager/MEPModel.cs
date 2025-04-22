﻿using Autodesk.Revit.DB.Electrical;
using OpenMEPRevit.Helpers;

namespace OpenMEPRevit.ConnectorManager;

/// <summary>
///    Supports all MEP models that are persistent within the Autodesk Revit project.
/// </summary>
public class MEPModel
{
    private MEPModel()
    {
        
    }

    /// <summary>
    /// The part type of the mechanical fitting.
    /// </summary>
    /// <param name="mepModel">Autodesk.Revit.DB.MEPModel</param>
    /// <returns name="partType">part type</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/MEPModel.PartType.png)
    /// </example>
    public static dynamic? PartType(Autodesk.Revit.DB.MEPModel mepModel)
    {
        if (mepModel is Autodesk.Revit.DB.Mechanical.MechanicalFitting m)
        {
            return m.PartType;
        }
        return null;
    }

#if !R20
    /// <summary>
    ///    Retrieves the electrical systems that are currently created using this MEPModel.
    /// </summary>
    /// <remarks>
    ///    This property returns a set of Electrical Systems. If there are no electrical systems created
    ///    for this model, this property will be an empty set.
    ///    This method supersedes an older <i>ElectricalSystems</i> property which has been deprecated.
    /// </remarks>
    /// <para name="mepModel">Autodesk.Revit.DB.MEPModel</para>
    /// <returns name="electricSystems">electricSystems</returns>
    /// <since>2021</since>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/MEPModel.GetElectricalSystems.png)
    /// </example>
    public static List<Revit.Elements.Element?> GetElectricalSystems(Autodesk.Revit.DB.MEPModel? mepModel)
    {
        List<ElectricalSystem>? electricalSystems = mepModel?.GetElectricalSystems().ToList();
        List<Revit.Elements.Element?> electricSystems = new List<Revit.Elements.Element?>();
        foreach (ElectricalSystem electricalSystem in electricalSystems!)
        {
            electricSystems.Add(electricalSystem.ToDynamoType());
        }
        return electricSystems;
    }
#endif

#if !R20
     /// <summary>
    ///    Retrieves the electrical systems this electrical panel currently is assigned to.
    /// </summary>
    /// <para name="mepModel">Autodesk.Revit.DB.MEPModel</para>
    /// <remarks>
    ///    This property returns a set of Electrical Systems. If there are no electrical systems created
    ///    for this model, this property will be an empty set.
    ///    This method supersedes an older <i>AssignedElectricalSystems</i> property which has been deprecated.
    /// </remarks>
    /// <returns name="assignedElectricalSystems">assignedElectricalSystems</returns>
    /// <since>2021</since>
     /// <example>
     /// ![](../OpenMEPPage/connectormanager/dyn/pic/MEPModel.GetAssignedElectricalSystems.png)
     /// </example>
     public static List<Revit.Elements.Element?> GetAssignedElectricalSystems(Autodesk.Revit.DB.MEPModel? mepModel)
    {
        List<ElectricalSystem>? assignedElectricalSystems = mepModel?.GetAssignedElectricalSystems().ToList();
        List<Revit.Elements.Element?> electricSystems = new List<Revit.Elements.Element?>();
        foreach (ElectricalSystem system in assignedElectricalSystems!)
        {
            electricSystems.Add(system.ToDynamoType());
        }
        return electricSystems;
    }
#endif
    
    /// <summary>
    /// Retrieves the Connector Manager from this MEPModel.
    /// </summary>
    /// <param name="mepModel">Autodesk.Revit.DB.MEPModel</param>
    /// <returns name="ConnectorManager">ConnectorManager</returns>
    /// <example>
    /// ![](../OpenMEPPage/connectormanager/dyn/pic/MEPModel.ConnectorManager.png)
    /// </example>
    public static Autodesk.Revit.DB.ConnectorManager? ConnectorManager(Autodesk.Revit.DB.MEPModel? mepModel)
    {
        return mepModel?.ConnectorManager;
    }
}