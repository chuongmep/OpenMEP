using Core;

namespace Element;

public static class MEPSystem
{
    /// <summary>
    /// Terminal elements in the system.
    /// </summary>
    /// <param name="mepSystem">mep system</param>
    /// <returns name="elements">The return value is a read only collection and doesn't include the base equipment or panel.</returns>
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
    public static Revit.Elements.Element? BaseEquipment(Revit.Elements.Element? mepSystem)
    {
        Autodesk.Revit.DB.Element? element = mepSystem?.InternalElement;
        if (element is Autodesk.Revit.DB.MEPSystem m)
        {
            return m.BaseEquipment.ToDynamoType();
        }
        return null;
    }
}