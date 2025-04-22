using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Helpers.CLASSES
{
    /// <summary>
    /// Class for Quad with variable number of points
    /// Use Open Source From lunchbox package : https://bitbucket.org/provingground-io/lunchbox-for-dynamo/src/master/
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class clsQuadVar
    {
        #region Private Members
        private List<clsPoint> _pts;
        private int _count;
        #endregion

        #region Public Properties
        /// <summary>
        /// List of points
        /// </summary>
        public List<clsPoint> Points { get { return _pts; } }

        /// <summary>
        /// Points per row
        /// </summary>
        public int RowCount { get { return _count; } }
        #endregion

        /// <summary>
        /// Quad Class with multiple points on an edge.
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="count"></param>
        public clsQuadVar(List<clsPoint> pts, int count)
        {
            _pts = pts;
            _count = count;
        }
    }
}
