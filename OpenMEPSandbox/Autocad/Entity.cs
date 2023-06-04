using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Autocad
{
    [IsVisibleInDynamoLibrary(false)]
    public abstract class Entity
    {
        public dynamic CadEntity;
        public string ObjectName;
        public string Layer;
        protected void BaseInit(dynamic entity)
        {
            CadEntity = entity ?? throw new ArgumentNullException("cad entity is null");
            ObjectName = entity.ObjectName;
            Layer = entity.Layer;
            
        }


    }
}
