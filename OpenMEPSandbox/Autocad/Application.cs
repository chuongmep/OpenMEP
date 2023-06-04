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
        /// <returns name="IAcadApplication">IAcadApplication</returns>
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
        /// <param name="IAcadApplication"></param>
        /// <returns></returns>
        public static string FullName(dynamic IAcadApplication)
        {
            return IAcadApplication.FullName;
        }

        /// <summary>
        /// Return
        /// </summary>
        /// <param name="IAcadApplication"></param>
        /// <returns></returns>
        public static string Name(dynamic IAcadApplication)
        {
            return IAcadApplication.Name;
        }

        /// <summary>
        /// Return
        /// </summary>
        /// <param name="IAcadApplication"></param>
        /// <returns></returns>
        public static string Path(dynamic IAcadApplication)
        {
            return IAcadApplication.Path;
        }

        /// <summary>
        /// Return
        /// </summary>
        /// <param name="IAcadApplication"></param>
        /// <returns></returns>
        public static string Version(dynamic IAcadApplication)
        {
            return IAcadApplication.Version;
        }

        /// <summary>
        /// Return
        /// </summary>
        /// <param name="IAcadApplication"></param>
        /// <returns></returns>
        public static List<dynamic> Documents(dynamic IAcadApplication)
        {
            List<dynamic> AcadDocuments = new List<dynamic>();
            var acadDocuments = IAcadApplication.Documents;
            for (int i = 0; i < IAcadApplication.Documents.Count; i++)
            {
                dynamic acadDocument = acadDocuments.Item(i);
                AcadDocuments.Add(acadDocument);
            }

            return AcadDocuments;
        }
    }
}