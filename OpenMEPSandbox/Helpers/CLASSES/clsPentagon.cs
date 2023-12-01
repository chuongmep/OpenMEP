namespace OpenMEPSandbox.Helpers.CLASSES
{
    /// <summary>
    /// Pentagon class
    /// Use Open Source From lunchbox package : https://bitbucket.org/provingground-io/lunchbox-for-dynamo/src/master/
    /// </summary>
    public class clsPentagon
    {
        #region Private Members
        private clsPoint _a;
        private clsPoint _b;
        private clsPoint _c;
        private clsPoint _d;
        private clsPoint _e;
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

        /// <summary>
        /// Corner point D
        /// </summary>
        public clsPoint D
        {
            get
            {
                return _d;
            }
            set
            {
                _d = D;
            }
        }

        /// <summary>
        /// Corner point E
        /// </summary>
        public clsPoint E
        {
            get
            {
                return _e;
            }
            set
            {
                _e = E;
            }
        }
        #endregion

        /// <summary>
        /// Create new Quad
        /// </summary>
        /// <param name="a">Corner A</param>
        /// <param name="b">Corner B</param>
        /// <param name="c">Corner C</param>
        /// <param name="d">Corner D</param>
        /// <param name="e">Corner E</param>
        public clsPentagon(clsPoint a, clsPoint b, clsPoint c, clsPoint d, clsPoint e)
        {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
            _e = e;
        }
    }
}
