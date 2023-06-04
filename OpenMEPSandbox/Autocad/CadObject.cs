using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Autocad
{
    [IsVisibleInDynamoLibrary(false)]
    public enum CadFilterData
    {
        All, LWPolyline, Text, Circle, BlockReference, Line,MText
    }
    [IsVisibleInDynamoLibrary(false)]
    public class CadObject : Entity
    {
        public CadObject(dynamic entity)
        {
            BaseInit(entity);
        }

        public bool Is(CadFilterData filter)
        {
            switch (filter)
            {
                case CadFilterData.LWPolyline: return ObjectName.Equals("AcDbPolyline");
                case CadFilterData.Line: return ObjectName.Equals("AcDbLine");
                case CadFilterData.Text: return ObjectName.Equals("AcDbText");
                case CadFilterData.MText: return ObjectName.Equals("AcDbMText");
                case CadFilterData.Circle: return ObjectName.Equals("AcDbCircle");
                case CadFilterData.BlockReference: return ObjectName.Equals("AcDbBlockReference");
                default: return false;
            }
        }

        public T CastEntity<T>()
        {
            // Get all public constructors for types[0]
            var ctors = typeof(T).GetConstructor(new[] {typeof(object)});
            // Create a class of types[0] using the first constructor
            return (T) ctors.Invoke(new object[] {CadEntity});
        }
    }
}