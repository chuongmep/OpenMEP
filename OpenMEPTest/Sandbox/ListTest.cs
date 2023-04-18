using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB.DirectContext3D;
using NUnit.Framework;
using List = OpenMEPSandbox.Core.List;

namespace OpenMEPTest.Sandbox;

public class ListTest
{
    [SetUp]
    public void SetUp()
    {
    }

    [Test]
    [TestCase(new object[] {1, 2, 3, 4, 5}, new object[] {3, 4, 5, 6, 7})]
    [TestCase(new object[] {"a", "b", "c", "d", "e"}, new object[] {"c", "d", "e", "f", "g"})]
    public void IntersectTest(object[] lst1, object[] lst2)
    {
        List<object> intersect = List.Intersect(lst1.ToList(), lst2.ToList());
        Assert.IsTrue(intersect.Count == 3);
    }

    [Test]
    [TestCase(new object[] {1, 2, 3, 4, 5}, new object[] {3, 4, 5, 6, 7})]
    [TestCase(new object[] {"a", "b", "c", "d", "e"}, new object[] {"c", "d", "e", "f", "g"})]
    public void DifferenceTest(object[] lst1, object[] lst2)
    {
        List<object> difference = List.Difference(lst1.ToList(), lst2.ToList());
        Assert.IsTrue(difference.Count == 2);
    }

    [Test]
    [TestCase(new object[] {"one", "two", "three", "four", "five"})]
    [TestCase(new object[] {1, 2, 3, 4, 5})]
    [TestCase(new object[] {100, 200, 300, 400, 500})]
    public void IndexListTest(object[] lst)
    {
        List<object> indexList = List.IndexList(lst.ToList());
        Assert.IsTrue(indexList.Count == 5);
        Assert.IsTrue(indexList[0].ToString() == "0");
        Assert.IsTrue(indexList[1].ToString() == "1");
        Assert.IsTrue(indexList[2].ToString() == "2");
        Assert.IsTrue(indexList[3].ToString() == "3");
        Assert.IsTrue(indexList[4].ToString() == "4");
    }

    [Test]
    [TestCase(new object[] {"one", "two", "three", "four", "five", "one", "two"},
        new object[] {"one", "one", "two", "three"})]
    public void IndexUniqueTwoListObjectsTest(object[] lst1, object[] lst2)
    {
        Dictionary<string, object> indexList = List.IndexUniqueTwoListObjects(lst1.ToList(), lst2.ToList());
        object keys = indexList.FirstOrDefault().Value;
        object values = indexList.LastOrDefault().Value;
        // keys should be list is one, two, three, four, five
        Assert.IsTrue(((List<object>) keys).Count == 5);
        Assert.IsTrue(((List<object>) keys)[0].ToString() == "one");
        Assert.IsTrue(((List<object>) keys)[1].ToString() == "two");
        Assert.IsTrue(((List<object>) keys)[2].ToString() == "three");
        Assert.IsTrue(((List<object>) keys)[3].ToString() == "four");
        Assert.IsTrue(((List<object>) keys)[4].ToString() == "five");
        // values should be list is 0,1,2,3,4
        Assert.IsTrue(((List<int>) values).Count == 5);
        Assert.IsTrue(((List<int>) values)[0].ToString() == "0");
        Assert.IsTrue(((List<int>) values)[1].ToString() == "1");
        Assert.IsTrue(((List<int>) values)[2].ToString() == "2");
        Assert.IsTrue(((List<int>) values)[3].ToString() == "3");
        Assert.IsTrue(((List<int>) values)[4].ToString() == "4");
    }
}