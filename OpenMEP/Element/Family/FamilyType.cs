using Autodesk.Revit.DB;
using Dynamo.Graph.Nodes;
using OpenMEP.Helpers;
using RevitServices.Transactions;

namespace OpenMEP.Element.Family;

public class FamilyType
{
    private FamilyType()
    {
        
    }
    /// <summary>
        /// The name of the family type.
        /// </summary>
        /// <param name="familyType">family type</param>
        /// <returns></returns>
        public static string Name(Autodesk.Revit.DB.FamilyType familyType)
        {
            return familyType.Name;
        }

        /// <summary>
        /// Provides access to the string contents of the given family parameter.
        /// </summary>
        /// <param name="familyType">family type</param>
        /// <param name="familyParameter">Autodesk.Revit.DB.FamilyParameter</param>
        /// <returns></returns>
        public static string AsString(Autodesk.Revit.DB.FamilyType familyType,
            Autodesk.Revit.DB.FamilyParameter familyParameter)
        {
            return familyType.AsString(familyParameter);
        }

        /// <summary>
        /// Provides access to value as a string with unit in the given family parameter.
        /// </summary>
        /// <param name="familyType">family type</param>
        /// <param name="familyParameter">Autodesk.Revit.DB.FamilyParameter</param>
        /// <returns></returns>
        public static string AsValueString(Autodesk.Revit.DB.FamilyType familyType,
            Autodesk.Revit.DB.FamilyParameter familyParameter)
        {
            return familyType.AsValueString(familyParameter);
        }

        /// <summary>
        /// Provides access to the double precision number of the given family parameter.
        /// </summary>
        /// <param name="familyType">family type</param>
        /// <param name="familyParameter">Autodesk.Revit.DB.FamilyParameter</param>
        /// <returns></returns>
        public static double? AsDouble(Autodesk.Revit.DB.FamilyType familyType,
            Autodesk.Revit.DB.FamilyParameter familyParameter)
        {
            return familyType.AsDouble(familyParameter);
        }

        /// <summary>
        /// Provides access to the Autodesk::Revit::DB::ElementId^ stored in the given family parameter.
        /// </summary>
        /// <param name="familyType">family type</param>
        /// <param name="familyParameter">Autodesk.Revit.DB.FamilyParameter</param>
        /// <returns name="element"></returns>
        public static global::Revit.Elements.Element AsElementId(Autodesk.Revit.DB.FamilyType familyType,
            Autodesk.Revit.DB.FamilyParameter familyParameter)
        {
            return familyType.AsElementId(familyParameter).ToDynamoType();
        }

        /// <summary>
        /// Identifies if the object is read-only or modifiable.
        /// </summary>
        /// <param name="familyType">family type</param>
        /// <returns name="bool">If true, the object may not be modified. If false, the object's contents may be modified.</returns>
        [NodeCategory("Query")]
        public static bool IsReadOnly(Autodesk.Revit.DB.FamilyType familyType)
        {
            return familyType.IsReadOnly;
        }
        /// <summary>
        /// get value of parameter family type
        /// </summary>
        /// <param name="doc">family document</param>
        /// <param name="familyType">family type</param>
        /// <param name="parameterName">parameter name</param>
        /// <returns name="familyParameter">family parameter</returns>
        public static object? GetParameterValue(Autodesk.Revit.DB.Document doc,
            Autodesk.Revit.DB.FamilyType familyType, string parameterName)
        {
            if (!doc.IsFamilyDocument) throw new ArgumentException("request family document environment");
            Autodesk.Revit.DB.FamilyManager familyManager = doc.FamilyManager;
            TransactionManager.Instance.EnsureInTransaction(doc);
            familyManager.CurrentType = familyType;
            TransactionManager.Instance.TransactionTaskDone();
            Autodesk.Revit.DB.FamilyParameter? familyParameter = familyManager.get_Parameter(parameterName);
            switch (familyParameter.StorageType)
            {
                case StorageType.Integer:
                    return familyType.AsInteger(familyParameter);
                case StorageType.Double:
                    return familyType.AsDouble(familyParameter);
                case StorageType.String:
                    return familyType.AsString(familyParameter);
                case StorageType.ElementId:
                    return doc.GetElement(familyType.AsElementId(familyParameter)).ToDynamoType();
                case StorageType.None:
                    return familyType.AsValueString(familyParameter);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
}