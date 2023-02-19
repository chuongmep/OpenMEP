using System.Collections;
using System.IO;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using RevitServices.Persistence;
using RevitServices.Transactions;

namespace OpenMEP.Element.Family
{
    /// <summary>
    /// Re
    /// </summary>
    public class FamilySizeManage
    {
        private FamilySizeManage()
        {
        }

        /// <summary>
        /// Gets a FamilySizeTableManager from a Family
        /// </summary>
        /// <param name="family">family</param>
        /// <returns name="familySizeTableManager">familySizeTableManager</returns>
        [NodeCategory("Query")]
        public static FamilySizeTableManager GetFamilySizeTableManager(global::Revit.Elements.Element family)
        {
            ElementId id = family.InternalElement.Id;
            Autodesk.Revit.DB.Document elementDocument = family.InternalElement.Document;
            FamilySizeTableManager familySizeTableManager =
                FamilySizeTableManager.GetFamilySizeTableManager(elementDocument, id);
            return familySizeTableManager;
            
        }

        /// <summary>
        /// Adds FamilySizeTableManager to a Family.
        /// A FamilySizeTableManager and FamilySizeTables are only needed when
        /// importing, exporting, or removing size data previously stored in CSV files.
        /// </summary>
        /// <param name="doc">Family owned document or project document.</param>
        /// <param name="family"></param>
        /// <returns name="result">True if successful, false otherwise.</returns>
        [NodeCategory("Create")]
        public static bool CreateFamilySizeTableManager([DefaultArgument("null")]Autodesk.Revit.DB.Document doc,global::Revit.Elements.Element family )
        {

            Autodesk.Revit.DB.Document document = doc ?? DocumentManager.Instance.CurrentDBDocument;
            ElementId elementId = new ElementId(family.Id);
            bool flag;
            document.Regenerate();
            using (Autodesk.Revit.DB.Transaction tran = new Autodesk.Revit.DB.Transaction(doc,"CreateFamilySizeTableManager"))
            {
                tran.Start();
                flag = FamilySizeTableManager.CreateFamilySizeTableManager(document, elementId);
                tran.Commit();
            }
            return flag;
        }
        /// <summary>
        /// Get a FamilySizeTable by name.
        /// </summary>
        /// <param name="familySizeTableManager">familySizeTableManager</param>
        /// <param name="tablename">name of table</param>
        /// <returns name="familySizeTable">familySizeTable</returns>
        [NodeCategory("Query")]
        public static FamilySizeTable GetSizeTable(FamilySizeTableManager familySizeTableManager, string tablename)
        {
            FamilySizeTable familySizeTable = familySizeTableManager.GetSizeTable(tablename);
            return familySizeTable;
        }

        /// <summary>
        /// Get the FamilySizeTable names in a family.
        /// </summary>
        /// <param name="familySizeTableManager">Array of size table names.</param>
        [MultiReturn("FamilySizeTableNames", "NumberOfSizeTables")]
        [NodeCategory("Query")]
        public static IDictionary GetAllSizeTableNames(FamilySizeTableManager familySizeTableManager)
        {
            List<string> tableNames = familySizeTableManager.GetAllSizeTableNames().ToList();
            int numberOfSizeTables = familySizeTableManager.NumberOfSizeTables;
            return new Dictionary<string, object>
            {
                {"FamilySizeTableNames", tableNames},
                {"NumberOfSizeTables", numberOfSizeTables}
            };
        }

        /// <summary>
        /// Removes the FamilySizeTable of a given name.
        /// </summary>
        /// <param name="familySizeTableManager">familySizeTableManager</param>
        /// <param name="tablename">The FamilySizeTable name.</param>
        /// <returns name="result">True if successful, false otherwise.</returns>
        [NodeCategory("Action")]
        public static bool RemoveSizeTable(FamilySizeTableManager familySizeTableManager, string tablename)
        {
            Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
            TransactionManager.Instance.EnsureInTransaction(doc);
            bool result = familySizeTableManager.RemoveSizeTable(tablename);
            TransactionManager.Instance.TransactionTaskDone();
            return result;
        }

        /// <summary>
        /// Exports the size table to  aCSV file.
        /// </summary>
        /// <param name="familySizeTableManager"></param>
        /// <param name="tablename"></param>
        /// <param name="filepath"></param>
        /// <returns name="result"></returns>
        public static bool ExportSizeTable(FamilySizeTableManager familySizeTableManager, string tablename,
            string filepath)
        {
            if(!filepath.ToLower().EndsWith(".csv") || !filepath.ToLower().EndsWith(".txt") || !filepath.ToLower().EndsWith(".xls"))
            {
                filepath += ".csv";
                return familySizeTableManager.ExportSizeTable(tablename, filepath);
            }
            return familySizeTableManager.ExportSizeTable(tablename, filepath);
        }

        /// <summary>
        /// Exports the size table to  a CSV file.
        /// </summary>
        /// <param name="familySizeTableManager">familySizeTableManager</param>
        /// <param name="tablename"></param>
        /// <param name="directory">directory output export</param>
        /// <param name="filename">name of file export</param>
        /// <param name="extension">format file export</param>
        /// <returns name="result">True if successful, false otherwise.</returns>
        public static bool ExportSizeTable(FamilySizeTableManager familySizeTableManager, string tablename,
            string directory, string filename,string extension=".csv")
        {
            if(string.IsNullOrEmpty(extension)) throw new ArgumentNullException(nameof(extension));
            string filepath =  Path.Combine(directory, filename+extension);
            bool result = familySizeTableManager.ExportSizeTable(tablename, filepath);
            return result;
        }
        /// <summary>
        /// Imports a FamilySizeTable from a CSV file.
        /// </summary>
        /// <param name="doc">document</param>
        /// <param name="filepath">filepath</param>
        /// <param name="familySizeTableManager"></param>
        /// <returns></returns>
        public static bool ImportSizeTable([DefaultArgument("null")] Autodesk.Revit.DB.Document doc, string filepath,
            FamilySizeTableManager familySizeTableManager)
        {
            Autodesk.Revit.DB.Document newdoc = doc ?? DocumentManager.Instance.CurrentDBDocument;
            FamilySizeTableErrorInfo sizeTableErrorInfo = new FamilySizeTableErrorInfo();
            bool result = familySizeTableManager.ImportSizeTable(newdoc, filepath, sizeTableErrorInfo);
            return result;
        }
    }
}