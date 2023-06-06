using Autodesk.DesignScript.Runtime;
using CoreNodeModels;
using Dynamo.Graph.Nodes;
using Dynamo.Utilities;
using Newtonsoft.Json;
using ProtoCore.AST.AssociativeAST;

namespace OpenMEPUI;

[NodeName("CadFilterTypes")]
[NodeDescription("List filter types object in AutoCAD")]
[NodeCategory("OpenMEP.Autocad.Selection")]
[IsDesignScriptCompatible]
public class CadFilterDropDown : DSDropDownBase
{
    public CadFilterDropDown() : base("CadFilterType")
    {
    }

    // Starting with Dynamo v2.0 you must add Json constructors for all nodeModel
    // dervived nodes to support the move from an Xml to Json file format.  Failing to
    // do so will result in incorrect ports being generated upon serialization/deserialization.
    // This constructor is called when opening a Json graph. We must also pass the deserialized 
    // ports with the json constructor and then call the base class passing the ports as parameters.
    [JsonConstructor]
    public CadFilterDropDown(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base("CadFilterType", inPorts,
        outPorts)
    {
    }

    protected override SelectionState PopulateItemsCore(string currentSelection)
    {
        // The Items collection contains the elements
        // that appear in the list. For this example, we
        // clear the list before adding new items, but you
        // can also use the PopulateItems method to add items
        // to the list.

        Items.Clear();

        // Create a number of DynamoDropDownItem objects 
        // to store the items that we want to appear in our list.

        // add list CadFilterData to Items
        var newItems = new List<DynamoDropDownItem>();
        var filterTypes = Enum.GetValues(typeof(CadFilterData));
        foreach (var filterType in filterTypes)
        {
            newItems.Add(new DynamoDropDownItem(filterType.ToString(), filterType));
        }
        Items.AddRange(newItems);

        // Set the selected index to something other
        // than -1, the default, so that your list
        // has a pre-selection.

        SelectedIndex = 0;
        return SelectionState.Restore;
    }

    public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
    {
        // Build an AST node for the type of object contained in your Items collection.

#if R20 || R21 || R22 || R23
        var intNode = AstFactory.BuildIntNode((int) Items[SelectedIndex].Item);
#else
        var intNode = AstFactory.BuildIntNode((int) Items[SelectedIndex].Item);
#endif
        var assign = AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), intNode);

        return new List<AssociativeNode> {assign};
    }
}
[IsVisibleInDynamoLibrary(false)]
public enum CadFilterData
{
    Point =0,
    Block =1,
    BlockReference =2,
    Attribute =3,
    Text =4,
    MText =5,
    Table =6,
    Leader =7,
    MLeader =8,
    Arc =9,
    Circle =10,
    Ellipse =11,
    Hatch =12,
    Region = 13,
    Solid =14,
    Solid3D =15,
    LWPolyline =16,
    Polyline=17,
    Polyline2D =18,
    Polyline3D =19,
    Spline =20,
    Line =21,
    XLine =22,
    MLine =23,
    Ray =24,
    PolygonMesh =25,
    PolyFaceMesh =26,
    SubDMesh =27,
    Face =28,
    WipeOut =29,
    RotatedDimension =30,
    AngularDimension3Point =31,
    AngularDimension2Line = 32,
    RadialDimension =33,
    DiametricDimension = 34,
    OrdinateDimension =35,
    AlignedDimension =36,
    All =-1,
}