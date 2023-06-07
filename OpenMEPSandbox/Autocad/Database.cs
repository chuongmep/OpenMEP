using Autodesk.DesignScript.Runtime;
using OpenMEPUI;

namespace OpenMEPSandbox.Autocad
{
    /// <summary>
    /// The Database object contains all of the graphical and most of the non-graphical AutoCAD objects. Some of the objects contained in the database are entities, symbol tables, and named dictionaries. Entities in the database represent graphical objects within a drawing. Lines, circles, arcs, text, hatch, and polylines are examples of entities.
    /// A user can see an entity on the screen and can manipulate it.
    /// </summary>
    public class Database
    {
        private Database()
        {
        
        }
        /// <summary>
        /// Get all blocks in database of Document
        /// </summary>
        /// <param name="AcadDatabase">AcadDatabase</param>
        /// <returns name="AcadBlocks">AcadBlocks</returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Database.Blocks.png)
        /// [Database.Blocks.dyn](../OpenMEPPage/autocad/Database.Blocks.dyn)
        /// </example>
        public static dynamic Blocks(dynamic AcadDatabase)
        {
            List<dynamic> Blocks = new List<dynamic>();
            foreach (dynamic block in AcadDatabase.Blocks)
            {
                var cadObj = new CadObject(block);
                Blocks.Add(cadObj.CadEntity);
            }
            return Blocks;
        }
        
        /// <summary>
        /// Get all texts in database of Document
        /// </summary>
        /// <param name="AcadDatabase">AcadDatabase</param>
        /// <returns name="AcadTexts">AcadTexts</returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Database.Texts.png)
        /// [Database.Texts.dyn](../OpenMEPPage/autocad/Database.Texts.dyn)
        /// </example>
        public static dynamic Texts(dynamic AcadDatabase)
        {
            // Return all blocks in database interop 
            var modelSpace = AcadDatabase.ModelSpace;
            var lst = new List<dynamic>();
            for (int i = 0; i < modelSpace.Count; i++)
            {
                //AcadEntity
                dynamic item = modelSpace.Item(i);
                var cadObj = new CadObject(item);
                if (cadObj.Is(CadFilterData.Text))
                {
                    lst.Add(cadObj.CadEntity);
                }
            }
            return lst.Distinct().ToList();
        }
        
        /// <summary>
        /// Get all blocks in database of Document
        /// </summary>
        /// <param name="AcadDatabase">AcadDatabase</param>
        /// <returns name="AcadBlockReferences">AcadBlockReferences</returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Database.BlockReferences.png)
        /// [Database.BlockReferences.dyn](../OpenMEPPage/autocad/Database.BlockReferences.dyn)
        /// </example>
        public static dynamic BlockReferences(dynamic AcadDatabase)
        {
            // Return all blocks in database interop 
            var modelSpace = AcadDatabase.ModelSpace;
            var lst = new List<dynamic>();
            for (int i = 0; i < modelSpace.Count; i++)
            {
                //AcadEntity
                dynamic item = modelSpace.Item(i);
                var cadObj = new CadObject(item);
                if (cadObj.Is(CadFilterData.BlockReference))
                {
                    lst.Add(cadObj.CadEntity);
                }
            }
            return lst.Distinct().ToList();
        }
        
        /// <summary>
        /// Get all layers in database
        /// </summary>
        /// <param name="AcadDatabase">AcadDatabase</param>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Database.Layers.png)
        /// [Database.Layers.dyn](../OpenMEPPage/autocad/Database.Layers.dyn)
        /// </example>
        [MultiReturn("Names","AcadLayers")]
        public static Dictionary<string,object> Layers(dynamic AcadDatabase)
        {
            List<object> layers = new List<object>();
            List<object> names = new List<object>();
            foreach (dynamic layer in AcadDatabase.Layers)
            {
                layers.Add(layer);
                names.Add(layer.Name);
            }
            return new Dictionary<string, object>()
            {
                {"Names",names},
                {"AcadLayers",layers},
              
            };
        }

        

    }
}