using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Helpers.CLASSES
{
    /// <summary>
    /// Triangle class
    /// Use Open Source From lunchbox package : https://bitbucket.org/provingground-io/lunchbox-for-dynamo/src/master/
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class clsTriangle
    {
        #region Private Members
        private clsPoint _a;
        private clsPoint _b;
        private clsPoint _c;
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

        /// <summary>
        /// Corner point C
        /// </summary>
        public clsPoint C
        {
            get
            {
                return _c;
            }
            set
            {
                _c = C;
            }
        }
        #endregion

        /// <summary>
        /// Create new Quad
        /// </summary>
        /// <param name="a">Corner A</param>
        /// <param name="b">Corner B</param>
        /// <param name="c">Corner C</param>
        public clsTriangle(clsPoint a, clsPoint b, clsPoint c)
        {
            _a = a;
            _b = b;
            _c = c;
        }
    }
}
