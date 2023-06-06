using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using OpenMEPUI;

namespace OpenMEPSandbox.Autocad
{
    /// <summary>
    /// The AcadDocument object associated with the object. 
    /// </summary>
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
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Document.Current.png)
        /// [Document.Current.dyn](../OpenMEPPage/autocad/Document.Current.dyn)
        /// </example>
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
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Document.Name.png)
        /// [Document.Name.dyn](../OpenMEPPage/autocad/Document.Name.dyn)
        /// </example>
        public static string Name(dynamic AcadDocument)
        {
            return AcadDocument.Name;
        }
    
        /// <summary>
        /// FullName of Document
        /// </summary>
        /// <param name="AcadDocument"></param>
        /// <returns></returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Document.FullName.png)
        /// [Document.FullName.dyn](../OpenMEPPage/autocad/Document.FullName.dyn)
        /// </example>
        public static string FullName(dynamic AcadDocument)
        {
            return AcadDocument.FullName;
        }
        
        /// <summary>
        /// Database of Document
        /// </summary>
        /// <param name="AcadDocument">AcadDocument</param>
        /// <returns name="AcadDatabase">The Database object contains all of the graphical and most of the non-graphical AutoCAD objects.</returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Document.Database.png)
        /// [Document.Database.dyn](../OpenMEPPage/autocad/Document.Database.dyn)
        /// </example>
        public static dynamic Database(dynamic AcadDocument)
        {
            return AcadDocument.Database;
        }
        /// <summary>
        /// Get All Object in CadDocument By Filter Object Type
        /// </summary>
        /// <param name="AcadDocument">AcadDocument</param>
        /// <param name="CadFilterType">Filter Type of Cad Object</param>
        /// <returns name="CadObjects">The general objects of Autocad by Filter(Block,Text,MText,Line,....)</returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Document.GetCadObjectsByFilters.png)
        /// [Document.GetCadObjectsByFilters.dyn](../OpenMEPPage/autocad/Document.GetCadObjectsByFilters.dyn)
        /// </example>
        public static List<CadObject> GetCadObjectsByFilters(dynamic AcadDocument, int CadFilterType)
        {
            var modelSpace = AcadDocument.ModelSpace;
            var lst = new List<CadObject>();
            CadFilterData filter = (CadFilterData)Enum.ToObject(typeof(CadFilterData), CadFilterType);
            for (int i = 0; i < modelSpace.Count; i++)
            {
                //AcadEntity
                dynamic item = modelSpace.Item(i);
                CadObject cadObj = new CadObject(item);
                if (cadObj.Is(filter))
                {
                    lst.Add(cadObj);
                }
            }
            return lst.Distinct().ToList();
        }
    }
}