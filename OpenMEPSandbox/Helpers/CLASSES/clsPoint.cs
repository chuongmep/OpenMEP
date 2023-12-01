namespace OpenMEPSandbox.Helpers.CLASSES
{
    /// <summary>
    /// Point class
    /// Use Open Source From lunchbox package : https://bitbucket.org/provingground-io/lunchbox-for-dynamo/src/master/
    /// </summary>
    public class clsPoint
    {
        #region Private Members
        private double _x;
        private double _y;
        private double _z;
        #endregion

        #region Public Properties
        /// <summary>
        /// X coordinate
        /// </summary>
        public double X { get { return _x; } }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public double Y { get { return _y; } }

        /// <summary>
        /// Z coordinate
        /// </summary>
        public double Z { get { return _z; } }
        #endregion

        /// <summary>
        /// Create point based on XYZ
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        public clsPoint(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
    }
}
