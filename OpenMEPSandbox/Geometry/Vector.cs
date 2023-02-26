using Dynamo.Graph.Nodes;

namespace OpenMEPSandbox.Geometry;

public class Vector
{
    private Vector()
    {
        
    }
    
    /// <summary>
    /// Check whether two vector is same direction or not
    /// </summary>
    /// <param name="v1">the first vector</param>
    /// <param name="v2">the second vector</param>
    /// <returns name="bool">true if two vector is same direction</returns>
    [NodeCategory("Query")]
    public static bool IsSameDirection(Autodesk.DesignScript.Geometry.Vector v1,Autodesk.DesignScript.Geometry.Vector v2)
    {
        return v1.Normalized().Dot(v2.Normalized()).Equals(1);
    }
    /// <summary>
    /// Check two Vector is opposite direction or not
    /// </summary>
    /// <param name="v1">the first vector</param>
    /// <param name="v2">the second vector</param>
    /// <returns name="bool">true if two vector is opposite</returns>
    [NodeCategory("Query")]
    public static bool IsOppositeDirection(Autodesk.DesignScript.Geometry.Vector v1, Autodesk.DesignScript.Geometry.Vector v2)
    {
        return (v1.Normalized().Dot(v2.Normalized())).Equals(-1);
    }
    
    
}