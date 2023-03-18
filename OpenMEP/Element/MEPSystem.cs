using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;

namespace OpenMEP.Element;

/// <summary>A system in the Autodesk Revit MEP product.</summary>
/// <remarks>
///    This is the base class for electrical, mechanical and piping systems,
///    available only in the Autodesk Revit MEP product.
/// </remarks>
/// <since>2011</since>
public class MEPSystem
{
    private MEPSystem()
    {
        
    }
    /// <summary>
    /// Terminal elements in the system.
    /// </summary>
    /// <param name="mepSystem">mep system</param>
    /// 
    /// <returns name="elements">The return value is a read only collection and doesn't include the base equipment or panel.</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPSystem.Elements.png)
    /// </example>
    [NodeCategory("Query")]
    public static List<Revit.Elements.Element?> Elements(Revit.Elements.Element mepSystem)
    {
        List<Revit.Elements.Element?> elements = new List<Revit.Elements.Element?>();
        Autodesk.Revit.DB.Element element = mepSystem.InternalElement;
        if (element is Autodesk.Revit.DB.MEPSystem m)
        {
            List<Autodesk.Revit.DB.Element> elementsets = m.Elements.Cast<Autodesk.Revit.DB.Element>().ToList();
            foreach (Autodesk.Revit.DB.Element elementset in elementsets)
            {
                elements.Add(elementset.ToDynamoType());
            }
        }

        return elements;
    }
    
    /// <summary>
    /// get The base panel or equipment of the system.
    /// </summary>
    /// <param name="mepSystem">mep system</param>
    /// <returns name="BaseEquipment">The base panel or equipment of the system.</returns>
    /// <example>
    /// ![](../OpenMEPPage/element/dyn/pic/MEPSystem.BaseEquipment.png)
    /// </example>
    public static Revit.Elements.Element? BaseEquipment(Revit.Elements.Element? mepSystem)
    {
        Autodesk.Revit.DB.Element? element = mepSystem?.InternalElement;
        if (element is Autodesk.Revit.DB.MEPSystem m)
        {
            Autodesk.Revit.DB.FamilyInstance baseEquipment = m.BaseEquipment;
            if (baseEquipment == null) return null;
            return baseEquipment.ToDynamoType();
        }
        return null;
    }
}