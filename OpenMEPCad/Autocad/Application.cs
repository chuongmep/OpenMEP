using System.Runtime.InteropServices;
using System.Text;

namespace OpenMEPCad.Autocad
{
    /// <summary>
    /// An instance of the AutoCAD application.
    /// </summary>
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
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Application.Current.png)
        /// [Application.Current.dyn](../OpenMEPPage/autocad/Application.Current.dyn)
        /// </example>
        public static dynamic Current(bool toggle)
        {
            string ProgId = "AutoCAD.Application";
            try
            {
#if R25
                dynamic App = MarshalCore.MarshalForCore.GetActiveObject(ProgId);
#else
                dynamic App = Marshal.GetActiveObject(ProgId);
#endif
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
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Application.OpenDocument.png)
        /// [Application.OpenDocument.dyn](../OpenMEPPage/autocad/Application.OpenDocument.dyn)
        /// </example>
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
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Application.FullName.png)
        /// [Application.FullName.dyn](../OpenMEPPage/autocad/Application.FullName.dyn)
        /// </example>
        public static string FullName(dynamic AcadApplication)
        {
            return AcadApplication.FullName;
        }

        /// <summary>
        /// Return
        /// </summary>
        /// <param name="AcadApplication"></param>
        /// <returns></returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Application.Name.png)
        /// [Application.Name.dyn](../OpenMEPPage/autocad/Application.Name.dyn)
        /// </example>
        public static string Name(dynamic AcadApplication)
        {
            return AcadApplication.Name;
        }

        /// <summary>
        /// Return
        /// </summary>
        /// <param name="AcadApplication"></param>
        /// <returns></returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Application.Path.png)
        /// [Application.Path.dyn](../OpenMEPPage/autocad/Application.Path.dyn)
        /// </example>
        public static string Path(dynamic AcadApplication)
        {
            return AcadApplication.Path;
        }

        /// <summary>
        /// Return
        /// </summary>
        /// <param name="AcadApplication"></param>
        /// <returns></returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Application.Version.png)
        /// [Application.Version.dyn](../OpenMEPPage/autocad/Application.Version.dyn)
        /// </example>
        public static string Version(dynamic AcadApplication)
        {
            return AcadApplication.Version;
        }

        /// <summary>
        /// Return All Document Opening In Autocad Application
        /// </summary>
        /// <param name="AcadApplication"></param>
        /// <returns name="AcadDocuments">All documents opening In Autocad</returns>
        /// <example>
        /// ![](../OpenMEPPage/autocad/pic/Application.Documents.png)
        /// [Application.Documents.dyn](../OpenMEPPage/autocad/Application.Documents.dyn)
        /// </example>
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