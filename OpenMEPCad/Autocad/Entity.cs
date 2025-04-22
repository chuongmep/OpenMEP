﻿using Autodesk.DesignScript.Runtime;

namespace OpenMEPCad.Autocad
{
    /// <summary>
    /// The base class for all AutoCAD objects.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public abstract class Entity
    {
        public dynamic CadEntity;
        public string ObjectName;
        public long ObjectID;
        protected void BaseInit(dynamic entity)
        {
            CadEntity = entity ?? throw new ArgumentNullException("cad entity is null");
            ObjectName = entity.ObjectName ?? String.Empty;
            ObjectID = entity.ObjectID;
        }


    }
}
