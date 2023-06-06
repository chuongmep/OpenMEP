using System.Runtime.InteropServices;
using System.Text;

namespace OpenMEPSandbox.Autocad
{
    public class Document
    {
        private Document()
        {
        
        }
        /// <summary>
        /// Get Current Document Of Autocad Application
        /// </summary>
        /// <para name="toggle">input true false to fresh data</para>
        /// <returns name="AcadDocument">AcadDocument</returns>
        public static dynamic? Current(bool toggle)
        {
            string ProgId = "AutoCAD.Application";
            try
            {
                dynamic App = Marshal.GetActiveObject(ProgId);
                return App.ActiveDocument;
            }
            catch (Exception)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("This process request Autocad Document opening :");
                sb.AppendLine("Step 1: Open Autocad Application");
                sb.AppendLine("Step 2: Open File Autocad");
                throw new ArgumentException(sb.ToString());
            }
        }

       
        /// <summary>
        /// Name of Document
        /// </summary>
        /// <param name="AcadDocument">AcadDocument</param>
        /// <returns></returns>
        public static string Name(dynamic AcadDocument)
        {
            return AcadDocument.Name;
        }
    
        /// <summary>
        /// FullName of Document
        /// </summary>
        /// <param name="AcadDocument"></param>
        /// <returns></returns>
        public static string FullName(dynamic AcadDocument)
        {
            return AcadDocument.FullName;
        }
    
        /// <summary>
        /// Get all blocks in database
        /// </summary>
        /// <param name="AcadDocument">IAcadDocument</param>
        /// <returns name="AcadBlockReferences">AcadBlockReferences</returns>
        public static dynamic BlockReferences(dynamic AcadDocument)
        {
            // Return all blocks in database interop 
            var modelSpace = AcadDocument.ModelSpace;
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
        
        /// <summary>
        /// Get all blocks in Document
        /// </summary>
        /// <param name="acadDocument"></param>
        /// <returns name="AcadBlocks">AcadBlocks</returns>
        public static dynamic Blocks(dynamic acadDocument)
        {
            List<dynamic> Blocks = new List<dynamic>();
            foreach (dynamic block in acadDocument.Blocks)
            {
                var cadObj = new CadObject(block);
                Blocks.Add(cadObj);
            }
            return Blocks;
        }
        
        /// <summary>
        /// Get all text in database
        /// </summary>
        /// <param name="AcadDocument">IAcadDocument</param>
        /// <returns name="IAcadTexts">IAcadTexts</returns>
        public static dynamic Texts(dynamic AcadDocument)
        {
            // Return all blocks in database interop 
            var modelSpace = AcadDocument.ModelSpace;
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
        /// Database of Document
        /// </summary>
        /// <param name="AcadDocument"></param>
        /// <returns name="AcadDatabase">database of document</returns>
        public static dynamic Database(dynamic AcadDocument)
        {
            return AcadDocument.Database;
        }
    }
}