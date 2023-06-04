using System.Runtime.InteropServices;
using System.Text;
using Autodesk.AutoCAD.Interop;

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
        /// Return AcadDocument Opened by file path
        /// </summary>
        /// <param name="AcadApplication"></param>
        /// <param name="filePath"></param>
        /// <returns name="AcadDocument">AcadDocument</returns>
        /// <exception cref="ArgumentException"></exception>
        public static dynamic OpenDocument(dynamic AcadApplication, string filePath)
        {
            try
            {
                List<dynamic> documents = Documents(AcadApplication);
                foreach (dynamic document in documents)
                {
                    if (document.FullName == filePath)
                    {
                        return document;
                    }
                }
                return AcadApplication.Documents.Open(filePath);
            }
            catch (Exception)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("This process request Autocad Application opening :");
                sb.AppendLine("Step 1: Open Autocad Application");
                sb.AppendLine("Step 2: Assign file path to node");
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