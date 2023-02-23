using System;
using System.Linq;
using Autodesk.Revit.DB;
using NUnit.Framework;
using RTF.Applications;
using RTF.Framework;

namespace OpenMEPTest;

[TestFixture]
public class ConnectorManagerTest
{
    [SetUp]
    public void SetUp()
    {
        var doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
        Assert.IsNotNull(doc);
    }

    [Test]
    [TestModel("Resources/bricks.rfa")]
    public void ModelHasTheCorrectNumberOfBricks()
    {
        var doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
    
        var fec = new FilteredElementCollector(doc);
        fec.OfClass(typeof(FamilyInstance));
    
        var bricks = fec.ToElements()
            .Cast<FamilyInstance>()
            .Where(fi => fi.Symbol.Family.Name == "brick");
    
        Assert.AreEqual(bricks.Count(), 4);
    }
    
    /// <summary>
    /// This is the Hello World of Revit testing. Here we
    /// simply call the Revit API to create a new ReferencePoint
    /// in the default empty.rfa file.
    /// </summary>
    [Test]
    public void CanCreateAReferencePoint()
    {
        var doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;

        using (var t = new Transaction(doc))
        {
            if (t.Start("Test one.") == TransactionStatus.Started)
            {
                //create a reference point
                var pt = doc.FamilyCreate.NewReferencePoint(new XYZ(5, 5, 5));

                if (t.Commit() != TransactionStatus.Committed)
                {
                    t.RollBack();
                }
            }
            else
            {
                throw new Exception("Transaction could not be started.");
            }
        }
        //verify that the point was created
        var collector = new FilteredElementCollector(doc);
        collector.OfClass(typeof (ReferencePoint));
        Assert.AreEqual(1, collector.ToElements().Count);
    }

    
}


