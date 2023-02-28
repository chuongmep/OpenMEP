using NUnit.Framework;
using RevitServices.Persistence;

namespace OpenMEPTest.Application;

[TestFixture]
public class OpenMEPTest
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
    public void CanGetOpenMEPVersion()
    {
        string version = OpenMEP.Application.Dynamo.Version();
        Assert.IsTrue(version.Length > 0);
    }
}