using System.Runtime.InteropServices;
using System.Text;
using Autodesk.AutoCAD.Interop.Common;

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
        /// <returns name="IAcadDocument">IAcadDocument</returns>
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
        /// <param name="IAcadDocument"></param>
        /// <returns></returns>
        public static string Name(dynamic IAcadDocument)
        {
            return IAcadDocument.Name;
        }
    
        /// <summary>
        /// FullName of Document
        /// </summary>
        /// <param name="IAcadDocument"></param>
        /// <returns></returns>
        public static string FullName(dynamic IAcadDocument)
        {
            return IAcadDocument.FullName;
        }
    
        /// <summary>
        /// Get all blocks in database
        /// </summary>
        /// <param name="IAcadDocument">IAcadDocument</param>
        /// <returns name="AcadBlockReferences">AcadBlockReferences</returns>
        public static dynamic BlockReferences(dynamic IAcadDocument)
        {
            // Return all blocks in database interop 
            var modelSpace = IAcadDocument.ModelSpace;
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
        /// Get all text in database
        /// </summary>
        /// <param name="IAcadDocument">IAcadDocument</param>
        /// <returns name="IAcadTexts">IAcadTexts</returns>
        public static dynamic Texts(dynamic IAcadDocument)
        {
            // Return all blocks in database interop 
            var modelSpace = IAcadDocument.ModelSpace;
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
        /// <param name="IAcadDocument"></param>
        /// <returns name="AcadDatabase">database of document</returns>
        public static dynamic Database(dynamic IAcadDocument)
        {
            return IAcadDocument.Database;
        }
    }
}