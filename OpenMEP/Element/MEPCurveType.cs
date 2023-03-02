using System.Security.Policy;
using OpenMEP.Helpers;

namespace OpenMEP.Element;

/// <summary>
/// A class can use for DuctType, PipeType, CableTrayType, ConduitType, WireType, MEPCurveType
/// </summary>
public class MEPCurveType
{
    private MEPCurveType()
    {
        
    }

    /// <summary>The default cross fitting of the MEP curve type.</summary>
    /// <para name="mepcurvetype">type of mep curve</para>
    /// <remarks>This property is used to retrieve the default cross fitting of the MEP curve type,
    /// and can be <see langword="null" /> if there is no default value.
    /// Use <see cref="T:Autodesk.Revit.DB.RoutingPreferenceManager" /> to set this property for PipeType MEPCurves.
    /// </remarks>
    /// <returns name="familyType">Cross fitting</returns>
    public static Revit.Elements.Element? Cross(Revit.Elements.Element mepcurvetype)
    {
        if (mepcurvetype == null) throw new ArgumentNullException(nameof(mepcurvetype));
        var mepCurveType = mepcurvetype.InternalElement as Autodesk.Revit.DB.MEPCurveType;
        if(mepCurveType!.Cross== null) return null;
        return mepCurveType!.Cross.ToDynamoType();
    }
    
    /// <summary>The default elbow fitting of the MEP curve type.</summary>
    /// <para name="mepcurvetype">type of mep curve</para>
    /// <remarks>This property is used to retrieve the default elbow fitting of the MEP curve type,
    /// and can be <see langword="null" /> if there is no default value.
    /// Use <see cref="T:Autodesk.Revit.DB.RoutingPreferenceManager" /> to set this property for PipeType MEPCurves.
    /// </remarks>
    /// <returns name="familyType">Elbow fitting</returns>
    public static Revit.Elements.Element? Elbow(Revit.Elements.Element mepcurvetype)
    {
        if (mepcurvetype == null) throw new ArgumentNullException(nameof(mepcurvetype));
        var mepCurveType = mepcurvetype.InternalElement as Autodesk.Revit.DB.MEPCurveType;
        if(mepCurveType!.Elbow== null) return null;
        return mepCurveType!.Elbow.ToDynamoType();
    }
    
    /// <summary>The default tap fitting of the MEP curve type.</summary>
    /// <para name="mepcurvetype">type of mep curve</para>
    /// <remarks>This property is used to retrieve the default tap fitting of the MEP curve type,
    /// and can be <see langword="null" /> if there is no default value.
    /// Use <see cref="T:Autodesk.Revit.DB.RoutingPreferenceManager" /> to set this property for PipeType MEPCurves.
    /// </remarks>
    /// <returns name="familyType">Tap fitting</returns>
    public static Revit.Elements.Element? Tap(Revit.Elements.Element mepcurvetype)
    {
        if (mepcurvetype == null) throw new ArgumentNullException(nameof(mepcurvetype));
        var mepCurveType = mepcurvetype.InternalElement as Autodesk.Revit.DB.MEPCurveType;
        if(mepCurveType!.Tap== null) return null;
        return mepCurveType!.Tap.ToDynamoType();
    }
    
    /// <summary>The default union fitting of the MEP curve type.</summary>
    /// <para name="mepcurvetype">type of mep curve</para>
    /// <remarks>This property is used to retrieve the default union fitting of the MEP curve type,
    /// and can be <see langword="null" /> if there is no default value.
    /// Use <see cref="T:Autodesk.Revit.DB.RoutingPreferenceManager" /> to set this property for PipeType MEPCurves.
    /// </remarks>
    /// <returns name="familyType">union fitting</returns>
    public static Revit.Elements.Element? Union(Revit.Elements.Element mepcurvetype)
    {
        if (mepcurvetype == null) throw new ArgumentNullException(nameof(mepcurvetype));
        var mepCurveType = mepcurvetype.InternalElement as Autodesk.Revit.DB.MEPCurveType;
        if(mepCurveType!.Union== null) return null;
        return mepCurveType!.Union.ToDynamoType();
    }
    
    /// <summary>The default transition fitting of the MEP curve type.</summary>
    /// <para name="mepcurvetype">type of mep curve</para>
    /// <remarks>This property is used to retrieve the default transition fitting of the MEP curve type,
    /// and can be <see langword="null" /> if there is no default value.
    /// Use <see cref="T:Autodesk.Revit.DB.RoutingPreferenceManager" /> to set this property for PipeType MEPCurves.
    /// </remarks>
    /// <returns name="familyType">transition fitting</returns>
    public static Revit.Elements.Element? Transition(Revit.Elements.Element mepcurvetype)
    {
        if (mepcurvetype == null) throw new ArgumentNullException(nameof(mepcurvetype));
        var mepCurveType = mepcurvetype.InternalElement as Autodesk.Revit.DB.MEPCurveType;
        if(mepCurveType!.Transition== null) return null;
        return mepCurveType!.Transition.ToDynamoType();
    }
    
    /// <summary>The default multi shape transition fitting of the MEP curve type.</summary>
    /// <para name="mepcurvetype">type of mep curve</para>
    /// <remarks>This property is used to retrieve the default multi shape transition fitting of the MEP curve type,
    /// and can be <see langword="null" /> if there is no default value.
    /// Use <see cref="T:Autodesk.Revit.DB.RoutingPreferenceManager" /> to set this property for PipeType MEPCurves.
    /// </remarks>
    /// <returns name="familyType"> multi shape transition fitting</returns>
    public static Revit.Elements.Element? MultiShapeTransition(Revit.Elements.Element mepcurvetype)
    {
        if (mepcurvetype == null) throw new ArgumentNullException(nameof(mepcurvetype));
        var mepCurveType = mepcurvetype.InternalElement as Autodesk.Revit.DB.MEPCurveType;
        if (mepCurveType!.MultiShapeTransition == null) return null;
        return mepCurveType.MultiShapeTransition.ToDynamoType();
    }
    
    /// <summary>The shape of the profile.</summary>
    /// <para name="mepcurvetype">type of mep curve</para>
    /// <since>2019</since>
    /// <returns name="Autodesk.Revit.DB.ConnectorProfileType">ConnectorProfileType</returns>
    public static dynamic Shape(Revit.Elements.Element mepcurvetype)
    {
        if (mepcurvetype == null) throw new ArgumentNullException(nameof(mepcurvetype));
        var mepCurveType = mepcurvetype.InternalElement as Autodesk.Revit.DB.MEPCurveType;
        return mepCurveType!.Shape;
    }
    
    /// <summary>The roughness of the MEP curve type.  For PipeTypes, please use Segment::Roughness</summary>
    /// <para name="mepcurvetype">type of mep curve</para>
    /// <returns name="double">Roughness</returns>
    public static double Roughness(Revit.Elements.Element mepcurvetype)
    {
        if (mepcurvetype == null) throw new ArgumentNullException(nameof(mepcurvetype));
        var mepCurveType = mepcurvetype.InternalElement as Autodesk.Revit.DB.MEPCurveType;
        return mepCurveType!.Roughness;
    }
    
    /// <summary>The preferred junction type of the MEP curve type.</summary>
    /// <para name="mepcurvetype">type of mep curve</para>
    /// <remarks>Use <see cref="T:Autodesk.Revit.DB.RoutingPreferenceManager" /> to set this property for PipeType MEPCurves.</remarks>
    public static dynamic PreferredJunctionType(Revit.Elements.Element mepcurvetype)
    {
        if (mepcurvetype == null) throw new ArgumentNullException(nameof(mepcurvetype));
        var mepCurveType = mepcurvetype.InternalElement as Autodesk.Revit.DB.MEPCurveType;
        return mepCurveType!.PreferredJunctionType;
    }
}