using System.Reflection;

namespace OpenMEPSandbox.Autocad;

/// <summary>
/// Class Presenting For Entity Object Autocad
/// </summary>
public class Object
{
    private Object()
    {
        
    }
    
    /// <summary>
    /// Specifies the name of the object. 
    /// </summary>
    /// <param name="object">cad object</param>
    /// <returns name="string">Name of the object.</returns>
    public static string Name(dynamic @object)
    {
        return @object.CadEntity.Name;
    }
    
    /// <summary>
    /// Gets the AutoCAD class name of the object. 
    /// </summary>
    /// <param name="object">cad object</param>
    /// <returns name="string">The AutoCAD class name of an object. </returns>
    public static string ObjectName(dynamic @object)
    {
        return @object.CadEntity.ObjectName;
    }
    
    /// <summary>
    /// Gets the object ID of the owner (parent) object. 
    /// </summary>
    /// <param name="object">AcadBlock</param>
    /// <returns name="ownerID">The object ID of an object's owner. </returns>
    public static dynamic OwnerID(dynamic @object)
    {
        return @object.CadEntity.OwnerID;
    }
    /// <summary>
    /// Gets the object ID of the owner (parent) object. 
    /// </summary>
    /// <param name="object">AcadBlock</param>
    /// <returns name="ownerID">The object ID of an object's owner. </returns>
    public static dynamic Handle(dynamic @object)
    {
        return @object.CadEntity.Handle;
    }
    /// <summary>
    /// Snoop Object Methods
    /// </summary>
    /// <param name="Object"></param>
    /// <returns name="methods"></returns>
    public static List<string> GetMethods(dynamic Object)
    {
        List<string> list = new List<string>();
        var methods = Object.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            var str = method.Name + "(";
            foreach (var parameter in parameters)
            {
                str += parameter.ParameterType.Name + " " + parameter.Name + ", ";
            }
            str += ")";
            list.Add(str);
        }
        return list;
    }
    /// <summary>
    /// Snoops Object Properties
    /// </summary>
    /// <param name="Object">Object need to snoop</param>
    /// <returns name="Properties"></returns>
    public static List<string> GetProperties(dynamic Object)
    {
        List<string> list = new List<string>();
        var properties = Object.GetType().GetProperties();
        foreach (var property in properties)
        {
            var str = property.Name + " : " + property.PropertyType.Name;
            list.Add(str);
        }
        return list;
    }
}