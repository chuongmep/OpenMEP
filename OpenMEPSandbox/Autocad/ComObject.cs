using System.Reflection;
using System.Security.Cryptography;

namespace OpenMEPSandbox.Autocad;

public class ComObject
{
    private ComObject()
    {
        
    }
    public static List<string> GetMethods(dynamic AcadObject)
    {
        List<string> list = new List<string>();
        var methods = AcadObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
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
    public static List<string> GetProperties(dynamic AcadObject)
    {
        List<string> list = new List<string>();
        var properties = AcadObject.GetType().GetProperties();
        foreach (var property in properties)
        {
            var str = property.Name + " : " + property.PropertyType.Name;
            list.Add(str);
        }
        return list;
    }
}