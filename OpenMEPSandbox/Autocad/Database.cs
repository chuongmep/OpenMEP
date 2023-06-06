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
        /// Get all blocks in Document
        /// </summary>
        /// <param name="AcadDatabase">AcadDatabase</param>
        /// <returns name="AcadBlocks">AcadBlocks</returns>
        public static dynamic Blocks(dynamic AcadDatabase)
        {
            List<dynamic> Blocks = new List<dynamic>();
            foreach (dynamic block in AcadDatabase.Blocks)
            {
                var cadObj = new CadObject(block);
                Blocks.Add(cadObj);
            }
            return Blocks;
        }
        
        /// <summary>
        /// Get all text in database
        /// </summary>
        /// <param name="AcadDatabase">IAcadDocument</param>
        /// <returns name="IAcadTexts">IAcadTexts</returns>
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
                    lst.Add(cadObj);
                }
            }
            return lst.Distinct().ToList();
        }
        
        /// <summary>
        /// Get all blocks in database
        /// </summary>
        /// <param name="AcadDatabase">IAcadDocument</param>
        /// <returns name="AcadBlockReferences">AcadBlockReferences</returns>
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
                    lst.Add(cadObj);
                }
            }
            return lst.Distinct().ToList();
        }

        // /// <summary>
        // /// Get User Coordinate Systems Of Autocad Database
        // /// </summary>
        // /// <param name="AcadDatabase"></param>
        // /// <returns name="AcadUCSs">AcadUCSs</returns>
        // public static dynamic UserCoordinateSystems(dynamic AcadDatabase)
        // {
        //     List<dynamic> ucss = new List<dynamic>();
        //     int count = AcadDatabase.UserCoordinateSystems.Count;
        //     for (int i = 0; i < count; i++)
        //     {
        //         dynamic ucs = AcadDatabase.UserCoordinateSystems.Item(i);
        //         ucss.Add(ucs);
        //     }
        //     AcadDatabase database;
        //     return ucss;
        // }

    }
}