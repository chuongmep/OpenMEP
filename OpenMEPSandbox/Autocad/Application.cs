using System.Runtime.InteropServices;
using System.Text;

namespace OpenMEPSandbox.Autocad
{
    public class Application
    {
        private Application()
        {
        }

        /// <summary>
        /// Get Current Application Of Autocad Application Opening
        /// </summary>
        /// <para name="toggle">true false to fresh data</para>
        /// <returns name="AcadApplication">AcadApplication</returns>
        public static dynamic Current(bool toggle)
        {
            string ProgId = "AutoCAD.Application";
            try
            {
                dynamic App = Marshal.GetActiveObject(ProgId);
                return App;
            }
            catch (Exception)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("This process request Autocad Application opening :");
                sb.AppendLine("Step 1: Open Autocad Application");
                sb.AppendLine("Step 2: Open File Autocad");
                throw new ArgumentException(sb.ToString());
            }
        }
        
        /// <summary>
        /// Return
        /// </summary>
        /// <param name="AcadApplication"></param>
        /// <returns></returns>
        public static string FullName(dynamic AcadApplication)
        {
            return AcadApplication.FullName;
        }

        /// <summary>
        /// Return
        /// </summary>
        /// <param name="AcadApplication"></param>
        /// <returns></returns>
        public static string Name(dynamic AcadApplication)
        {
            return AcadApplication.Name;
        }

        /// <summary>
        /// Return
        /// </summary>
        /// <param name="AcadApplication"></param>
        /// <returns></returns>
        public static string Path(dynamic AcadApplication)
        {
            return AcadApplication.Path;
        }

        /// <summary>
        /// Return
        /// </summary>
        /// <param name="AcadApplication"></param>
        /// <returns></returns>
        public static string Version(dynamic AcadApplication)
        {
            return AcadApplication.Version;
        }

        /// <summary>
        /// Return
        /// </summary>
        /// <param name="AcadApplication"></param>
        /// <returns></returns>
        public static List<dynamic> Documents(dynamic AcadApplication)
        {
            List<dynamic> AcadDocuments = new List<dynamic>();
            var acadDocuments = AcadApplication.Documents;
            for (int i = 0; i < AcadApplication.Documents.Count; i++)
            {
                dynamic acadDocument = acadDocuments.Item(i);
                AcadDocuments.Add(acadDocument);
            }

            return AcadDocuments;
        }
    }
}