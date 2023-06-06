using System.Reflection;

namespace OpenMEPSandbox.Autocad;

public class Object
{
    private Object()
    {
        
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