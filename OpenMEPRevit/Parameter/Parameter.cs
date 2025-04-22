﻿using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using OpenMEPRevit.Helpers;

namespace OpenMEPRevit.Parameter;
/// <summary>The parameter object contains the value data assigned to that parameter.</summary>
/// <remarks>The piece of data contained within the parameter can be either a Double, Integer,
/// String or ElementId. The parameter object can be retrieved from any Element object using
/// either a built in id, definition object or shared parameter guid. All Elements within
/// Autodesk Revit contain Parameters. These are options that can be accessed in a generic
/// fashion. Revit contains many built in parameter types but users and now developers, via the
/// API, can add their own parameters in the form of shared parameters. The developer should
/// become familiar with the Revit user interface for added and managing parameters and shared
/// parameters before using this API. The user interface components can be found in the following
/// locations: Element Properties dialog, Shared Parameters dialog (available from the File menu),
/// Project Parameters dialog (available from the Settings menu), Family Types dialog (available
/// from the Settings menu when editing a family). There are several relationships between the
/// objects that make up the APIs exposure of parameters. The parameter object contains the data
/// value. Parameter objects can be retrieved from Elements if you know its built-in id,
/// its definition or its shared parameter guid. Each parameter has a definition. New parameters
/// can be added to Elements by adding a ParameterBinding object to the Document object.</remarks>
public class Parameter
{
    private Parameter()
    {
    }

    /// [!Video https://www.youtube.com/embed/Sz1lCeedcPI]
    /// <summary>
    /// The element to which this parameter belongs.
    /// </summary>
    /// <param name="parameter">parameter</param>
    /// <returns name="element">element</returns>
    public static Revit.Elements.Element? Element(Revit.Elements.Parameter? parameter)
    {
        Autodesk.Revit.DB.Parameter? revitPra = parameter?.ToRevitType();
        return revitPra?.Element.ToDynamoType();
    }

#if R20 || R21 || R22
    /// <summary>
    /// Return information value of parameter
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    [MultiReturn("Group", "Name", "ParameterType", "StorageType", "UnitType", "Id", "Value", "IsReadOnly", "HasValue", "IsShared")]
    public static Dictionary<string,object?> GetParameterInformation(Revit.Elements.Parameter parameter)
    {
        string Group = parameter.Group;
        string ParameterType = parameter.ParameterType;
        string UnitType = parameter.UnitType; 
        string Name = parameter.Name;
        string StorageType = parameter.StorageType;
        int Id = parameter.Id;
        object Value = parameter.Value;
        bool IsReadOnly = parameter.IsReadOnly;
        bool HasValue = parameter.HasValue;
        bool IsShared = parameter.IsShared;
        return new Dictionary<string, object?>()
        {
            {nameof(Group), Group},
            {nameof(Name), Name},
            {nameof(ParameterType), ParameterType},
            {nameof(StorageType), StorageType},
            {nameof(UnitType), UnitType},
            {nameof(Id), Id},
            {nameof(Value), Value},
            {nameof(IsReadOnly), IsReadOnly},
            {nameof(HasValue), HasValue},
            {nameof(IsShared), IsShared},
        };
    }
#else
    /// <summary>
    /// Return information value of parameter
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    [MultiReturn("GroupTypeId", "Name", "SpecType", "StorageType", "UnitType", "Id", "Value", "IsReadOnly", "HasValue",
        "IsShared")]
    public static Dictionary<string, object?> GetParameterInformation(Revit.Elements.Parameter parameter)
    {
        string GroupTypeId = parameter.GroupType.TypeId;
        string Name = parameter.Name; 
        var SpecType = parameter.SpecType;
        string StorageType = parameter.StorageType;
        var UnitType = parameter.Unit;
#if R20 || R21 || R22 || R23
        int Id = parameter.Id;
#else
        long Id = parameter.Id;

#endif
        object Value = parameter.Value;
        bool IsReadOnly = parameter.IsReadOnly;
        bool HasValue = parameter.HasValue;
        bool IsShared = parameter.IsShared;
        return new Dictionary<string, object?>()
        {
            {nameof(GroupTypeId), GroupTypeId},
            {nameof(Name), Name},
            {nameof(SpecType), SpecType},
            {nameof(StorageType), StorageType},
            {nameof(UnitType), UnitType},
            {nameof(Id), Id},
            {nameof(Value), Value},
            {nameof(IsReadOnly), IsReadOnly},
            {nameof(HasValue), HasValue},
            {nameof(IsShared), IsShared},
        };
    }
#endif


    /// <summary>
    /// Describes the type that is used internally within the parameter to store its value.
    /// The property will return one of the following possibilities: String, Integer, Double or ElementId. Based on the value of this property the correct access and set methods should be used to retrieve and set the parameter's data value.
    /// </summary>
    /// <param name="parameter">parameter</param>
    /// <returns name="storageType">storageType</returns>
    public static dynamic? StorageType(Revit.Elements.Parameter? parameter)
    {
        Autodesk.Revit.DB.Parameter? revitParameter = parameter?.ToRevitType();
        StorageType? storageType = revitParameter?.StorageType;
        return storageType;
    }

    /// <summary>
    /// Returns the Definition object that describes the data type, name and other details of the parameter.
    /// </summary>
    /// <param name="parameter">parameter</param>
    /// <returns name="definition">definition</returns>
    public static Autodesk.Revit.DB.Definition? Definition(Revit.Elements.Parameter? parameter)
    {
        Autodesk.Revit.DB.Parameter? revitParameter = parameter?.ToRevitType();
        Autodesk.Revit.DB.Definition? storageType = revitParameter?.Definition;
        return storageType;
    }

    /// <summary>
    /// Returns a global parameter, if any, currently associated with this parameter.
    /// </summary>
    /// <param name="parameter">parameter</param>
    /// <returns></returns>
    public static Revit.Elements.Element? GetAssociatedGlobalParameter(Revit.Elements.Parameter? parameter)
    {
        Autodesk.Revit.DB.Parameter? revitParameter = parameter?.ToRevitType();
        Autodesk.Revit.DB.ElementId? storageType = revitParameter?.GetAssociatedGlobalParameter();
        return storageType?.ToDynamoType();
    }

#if !R20
    /// <summary>
    /// Gets the identifier of the unit quantifying the parameter value.
    /// </summary>
    /// <param name="parameter">parameter</param>
    /// <returns name="forgetypeid">forge typeid</returns>
    public static ForgeTypeId? GetUnitTypeId(Revit.Elements.Parameter? parameter)
    {
        Autodesk.Revit.DB.Parameter? revitParameter = parameter?.ToRevitType();
        ForgeTypeId? unitTypeId = revitParameter?.GetUnitTypeId();
        return unitTypeId;
    }
#endif

    /// <summary>
    /// Tests whether this parameter can be associated with the given global parameter.
    /// </summary>
    /// <param name="parameter">parameter</param>
    /// <param name="elementid">global element id</param>
    /// <returns name="bool">True if this parameter can be associated with the given global parameter; False otherwise.</returns>
    public static bool? CanBeAssociatedWithGlobalParameter(Revit.Elements.Parameter? parameter, int? elementid)
    {
        if (parameter == null) throw new ArgumentNullException(nameof(parameter));
        if (elementid == null) throw new ArgumentNullException(nameof(elementid));
        Autodesk.Revit.DB.Parameter? revitParameter = parameter?.ToRevitType();
#if R20 || R21 || R22 || R23
        bool? flag = revitParameter?.CanBeAssociatedWithGlobalParameter(new ElementId((int) elementid));

#else
        bool? flag = revitParameter?.CanBeAssociatedWithGlobalParameter(new ElementId((long)elementid));

#endif
        return flag;
    }

    /// <summary>
    /// Tests whether this parameter can be associated with any global parameter.
    /// </summary>
    /// <param name="parameter">parameter</param>
    /// <returns name="bool">True if the given parameter can be associated (is parametrizable); False otherwise.</returns>
    public static bool? CanBeAssociatedWithGlobalParameters(Revit.Elements.Parameter? parameter)
    {
        Autodesk.Revit.DB.Parameter? revitParameter = parameter?.ToRevitType();
        bool? flag = revitParameter?.CanBeAssociatedWithGlobalParameters();
        return flag;
    }

#if R22 || R23
    /// <summary>
    /// Gets the identifier of the parameter.
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static string? GetTypeId(Revit.Elements.Parameter? parameter)
    {
        Autodesk.Revit.DB.Parameter? revitParameter = parameter?.ToRevitType();
        Autodesk.Revit.DB.ForgeTypeId? storageType = revitParameter?.GetTypeId();
        return storageType?.TypeId;
    }
#endif
}