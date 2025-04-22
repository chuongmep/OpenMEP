using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Helpers.CLASSES
{
    /// <summary>
    /// A collection of panels (triangles, quads, pentagons, hexagons)
    /// Use Open Source From lunchbox package : https://bitbucket.org/provingground-io/lunchbox-for-dynamo/src/master/
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class clsPanelCollection
    {
        #region Private Members
        private List<clsTriangle>? _tris;
        private List<clsQuad> _quads;
        private List<clsPentagon>? _pents;
        private List<clsHexagon>? _hexs;
        #endregion

        #region Public Properties
        /// <summary>
        /// List of Triangles
        /// </summary>
        public List<clsTriangle>? Triangles { get { return _tris; } }

        /// <summary>
        /// List of Quads
        /// </summary>
        public List<clsQuad> Quads { get { return _quads; } }

        /// <summary>
        /// List of Pentagons
        /// </summary>
        public List<clsPentagon>? Pentagons { get { return _pents; } }

        /// <summary>
        /// List of Hexagons 
        /// </summary>
        public List<clsHexagon>? Hexagons { get { return _hexs; } }
        #endregion

        /// <summary>
        /// Panel collection
        /// </summary>
        /// <param name="tris">List of Triangles</param>
        /// <param name="quads">List of Quads</param>
        /// <param name="hexs">List of Hexagons</param>
        /// <param name="pents">List of Pentagons</param>
        public clsPanelCollection(List<clsTriangle>? tris, List<clsQuad> quads, List<clsPentagon>? pents, List<clsHexagon>? hexs)
        {
            _tris = tris;
            _quads = quads;
            _pents = pents;
            _hexs = hexs;
        }
    }
}
