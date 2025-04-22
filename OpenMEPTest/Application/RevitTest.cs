using NUnit.Framework;
using RevitServices.Persistence;

namespace OpenMEPTest.Application;

[TestFixture]
public class RevitTest
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
    public void CanGetRevitVersion()
    {
        string version = OpenMEPRevit.Application.Revit.Version();
        Assert.IsTrue(version.Length > 0);
    }
    
    
}


