using System.Collections;
using Autodesk.Revit.DB;
using Core;
using RevitServices.Persistence;
using RevitServices.Transactions;

namespace Element;

public class MEPCurve
{
   private MEPCurve()
   {
      
   }
   /// <summary>
   /// create a elbow fitting from two pipe
   /// </summary>
   /// <param name="mepCurve1">first pipe</param>
   /// <param name="mepCurve2">second pipe</param>
   /// <returns name="family instance">elbow fitting</returns>
   public static Revit.Elements.Element? NewElbowFitting( Revit.Elements.Element? mepCurve1,
      Revit.Elements.Element? mepCurve2)
   {
      Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
      TransactionManager.Instance.EnsureInTransaction(doc);
      Connector? c1 = ConnectorManager.Connector.GetConnectorCloset(mepCurve1, mepCurve2);
      Connector? c2 = ConnectorManager.Connector.GetConnectorCloset(mepCurve2,mepCurve1);
      Autodesk.Revit.DB.FamilyInstance newUnionFitting = doc.Create.NewElbowFitting(c2, c1);
      TransactionManager.Instance.TransactionTaskDone();
      if (newUnionFitting == null) return null;
      return newUnionFitting.ToDynamoType();
   }
   
   /// <summary>
   /// create a union fitting from two pipe
   /// </summary>
   /// <param name="mepCurve1">first element</param>
   /// <param name="mepCurve2">second element</param>
   /// <returns name="family instance">union fitting</returns>
   public static Revit.Elements.Element? NewUnionFitting(Revit.Elements.Element mepCurve1,
      Revit.Elements.Element mepCurve2)
   {
      Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;
      TransactionManager.Instance.EnsureInTransaction(doc);
      Connector? c1 = ConnectorManager.Connector.GetConnectorCloset(mepCurve1,mepCurve2);
      Connector? c2 = ConnectorManager.Connector.GetConnectorCloset(mepCurve2,mepCurve1);
      Autodesk.Revit.DB.FamilyInstance newUnionFitting = doc.Create.NewUnionFitting(c2, c1);
      TransactionManager.Instance.TransactionTaskDone();
      if (newUnionFitting == null)
      {
         List<Connector?> connectors = ConnectorManager.Connector.GetConnectors(mepCurve1);
         Connector? c11 = ConnectorManager.Connector.GetConnectorCloset(c1,connectors);
         ConnectorSet connectorSet = c11!.AllRefs;
         IEnumerator enumerator = connectorSet.GetEnumerator();
         while (enumerator.MoveNext())
         {
            Connector? current = enumerator.Current as Autodesk.Revit.DB.Connector;
            if (current == null) continue;
            if (current.Owner.Id.IntegerValue == mepCurve1.Id) continue;
            if (current.Owner is Autodesk.Revit.DB.Plumbing.PipingSystem) continue;
            Revit.Elements.Element? dynamoType = current.Owner.ToDynamoType();
            return dynamoType;
         }
      }
      return newUnionFitting.ToDynamoType();
   }
}