using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Helpers.CLASSES
{
    /// <summary>
    /// Line class
    /// Use Open Source From lunchbox package : https://bitbucket.org/provingground-io/lunchbox-for-dynamo/src/master/
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class clsLine
    {
        #region Private Mebers
        private clsPoint _a;
        private clsPoint _b;
        #endregion

        #region Public Properties
        /// <summary>
        /// Corner point A
        /// </summary>
        public clsPoint A
        {
            get
            {
                return _a;
            }
            set
            {
                _a = A;
            }
        }

        /// <summary>
        /// Corner point B
        /// </summary>
        public clsPoint B
        {
            get
            {
                return _b;
            }
            set
            {
                _b = B;
            }
        }
        #endregion

        /// <summary>
        /// Create new Quad
        /// </summary>
        /// <param name="a">Corner A</param>
        /// <param name="b">Corner B</param>
        public clsLine(clsPoint a, clsPoint b)
        {
            _a = a;
            _b = b;
        }
    }
}
