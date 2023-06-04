using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Autocad
{
    [IsVisibleInDynamoLibrary(false)]
    public enum CadFilterData
    {
        All,
        Point,
        Block,
        BlockReference,
        Attribute,
        Text,
        MText,
        Table,
        Leader,
        MLeader,
        Arc,
        Circle,
        Ellipse,
        Hatch,
        Region,
        Solid,
        Solid3D,
        LWPolyline,
        Polyline,
        Polyline2D,
        Polyline3D,
        Spline,
        Line,
        MLine,
        PolygonMesh,
        PolyFaceMesh,
        SubDMesh,
        Face,
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
                case CadFilterData.Point: return ObjectName.Equals("AcDbPoint");
                case CadFilterData.LWPolyline: return ObjectName.Equals("AcDbPolyline");
                case CadFilterData.Line: return ObjectName.Equals("AcDbLine");
                case CadFilterData.MLine: return ObjectName.Equals("AcDbMline");
                case CadFilterData.Text: return ObjectName.Equals("AcDbText");
                case CadFilterData.MText: return ObjectName.Equals("AcDbMText");
                case CadFilterData.Table: return ObjectName.Equals("AcDbTable");
                case CadFilterData.Leader: return ObjectName.Equals("AcDbLeader");
                case CadFilterData.MLeader: return ObjectName.Equals("AcDbMLeader");
                case CadFilterData.Arc: return ObjectName.Equals("AcDbArc");
                case CadFilterData.Circle: return ObjectName.Equals("AcDbCircle");
                case CadFilterData.Ellipse: return ObjectName.Equals("AcDbEllipse");
                case CadFilterData.Hatch: return ObjectName.Equals("AcDbHatch");
                case CadFilterData.Region: return ObjectName.Equals("AcDbRegion");
                case CadFilterData.Solid: return ObjectName.Equals("AcDbSolid");
                case CadFilterData.Solid3D: return ObjectName.Equals("AcDb3dSolid");
                case CadFilterData.Polyline: return ObjectName.Equals("AcDbPolyline");
                case CadFilterData.Polyline2D: return ObjectName.Equals("AcDb2dPolyline");
                case CadFilterData.Polyline3D: return ObjectName.Equals("AcDb3dPolyline");
                case CadFilterData.Spline: return ObjectName.Equals("Spline");
                case CadFilterData.Block: return ObjectName.Equals("AcDbMInsertBlock");
                case CadFilterData.BlockReference: return ObjectName.Equals("AcDbBlockReference");
                case CadFilterData.Attribute: return ObjectName.Equals("AcDbAttributeDefinition");
                case CadFilterData.PolygonMesh: return ObjectName.Equals("AcDbPolygonMesh");
                case CadFilterData.PolyFaceMesh: return ObjectName.Equals("AcDbPolyFaceMesh");
                case CadFilterData.SubDMesh: return ObjectName.Equals("AcDbSubDMesh");
                case CadFilterData.Face: return ObjectName.Equals("AcDbFace");
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