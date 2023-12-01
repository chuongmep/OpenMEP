namespace OpenMEPSandbox.Helpers.CLASSES
{
    /// <summary>
    /// Quad class
    /// Use Open Source From lunchbox package : https://bitbucket.org/provingground-io/lunchbox-for-dynamo/src/master/
    /// </summary>
    public class clsQuad
    {
        #region Private Members
        private clsPoint _a;
        private clsPoint _b;
        private clsPoint _c;
        private clsPoint _d;
        #endregion

        #region Public Properties
        /// <summary>
        /// Corner point A
        /// </summary>
        public clsPoint A { get { return _a; } }

        /// <summary>
        /// Corner point B
        /// </summary>
        public clsPoint B { get { return _b; } }

        /// <summary>
        /// Corner point C
        /// </summary>
        public clsPoint C { get { return _c; } }

        /// <summary>
        /// Corner point D
        /// </summary>
        public clsPoint D { get { return _d; } }
        #endregion

        /// <summary>
        /// Create new Quad
        /// </summary>
        /// <param name="a">Corner A</param>
        /// <param name="b">Corner B</param>
        /// <param name="c">Corner C</param>
        /// <param name="d">Corner D</param>
        public clsQuad(clsPoint a, clsPoint b, clsPoint c, clsPoint d)
        {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
        }
    }
}
