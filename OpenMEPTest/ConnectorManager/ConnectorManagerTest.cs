using System.Collections.Generic;
using Autodesk.Revit.DB;
using NUnit.Framework;
using Revit.Elements;
using RevitServices.Persistence;
using RTF.Framework;
using Element = Autodesk.Revit.DB.Element;

namespace OpenMEPTest.ConnectorManager;

[TestFixture]
public class ConnectorManagerTest
{
    [SetUp]
    public void SetUp()
    {
        DocumentManager.Instance.CurrentUIApplication =
            RTF.Applications.RevitTestExecutive.CommandData.Application;
        DocumentManager.Instance.CurrentUIDocument =
            RTF.Applications.RevitTestExecutive.CommandData.Application.ActiveUIDocument;
    }
    [Test]
    [TestModel("Resources/ConnectorTestR20.rvt")]
    [TestCase(717849)]
    [TestCase(717853)]
    public void CanGetConnectorManager(int elementid)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Element pipe = doc.GetElement(new ElementId(elementid));
        Assert.IsNotNull(pipe);
        Autodesk.Revit.DB.ConnectorManager? connectorManager = OpenMEP.ConnectorManager.ConnectorManager.GetConnectorManager(pipe.ToDSType(true));
        Assert.IsNotNull(connectorManager);
    }
    
    [Test]
    [TestModel("Resources/ConnectorTestR20.rvt")]
    [TestCase(717849)]
    [TestCase(717853)]
    public void CanGetConnectors(int elementid)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Element pipe = doc.GetElement(new ElementId(elementid));
        Assert.IsNotNull(pipe);
        Autodesk.Revit.DB.ConnectorManager? connectorManager = OpenMEP.ConnectorManager.ConnectorManager.GetConnectorManager(pipe.ToDSType(true));
        Assert.IsNotNull(connectorManager);
        List<Connector> connectors = OpenMEP.ConnectorManager.ConnectorManager.Connectors(connectorManager);
        Assert.IsNotNull(connectors);
    }
    [Test]
    [TestModel("Resources/ConnectorTestR20.rvt")]
    [TestCase(717849)]
    public void CanGetUnusedConnectors(int elementid)
    {
        Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
        Element pipe = doc.GetElement(new ElementId(elementid));
        Assert.IsNotNull(pipe);
        Autodesk.Revit.DB.ConnectorManager? connectorManager = OpenMEP.ConnectorManager.ConnectorManager.GetConnectorManager(pipe.ToDSType(true));
        Assert.IsNotNull(connectorManager);
        List<Connector> connectors = OpenMEP.ConnectorManager.ConnectorManager.UnusedConnectors(connectorManager);
        Assert.IsTrue(connectors.Count==1);
    }
    
}


