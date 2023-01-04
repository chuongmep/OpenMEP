using Autodesk.Revit.DB;

namespace Utils;

public class FamilyUtils
{
    private FamilyUtils(){}

    public static void ConvertFamilyToFaceHostBased(Autodesk.Revit.DB.Document doc,int familyId)
    {
        using Transaction tran = new Autodesk.Revit.DB.Transaction (doc, "convert");
        tran.Start();
        Autodesk.Revit.DB.FamilyUtils.ConvertFamilyToFaceHostBased(doc,new ElementId(familyId));
        tran.Commit();
    }

    public static void FamilyCanConvertToFaceHostBased(Autodesk.Revit.DB.Document doc,int familyId)
    {
        using Transaction tran = new Autodesk.Revit.DB.Transaction (doc, "convert");
        tran.Start();
        Autodesk.Revit.DB.FamilyUtils.FamilyCanConvertToFaceHostBased(doc,new ElementId(familyId));
        tran.Commit();
    }
    
    // public static IEnumerable<Element> GetProfileSymbols(Autodesk.Revit.DB.Document doc,ProfileFamilyUsage profileFamilyUsage,bool oneCurveLoopOnly)
    // {
    //     using Transaction tran = new Autodesk.Revit.DB.Transaction (doc, "convert");
    //     tran.Start();
    //     ICollection<ElementId> profileSymbols = Autodesk.Revit.DB.FamilyUtils.GetProfileSymbols(doc,profileFamilyUsage,oneCurveLoopOnly);
    //     foreach (ElementId elementId in profileSymbols)
    //     {
    //         yield return doc.GetElement(elementId).ToDSType(true);
    //     }
    //     tran.Commit();
    // }
}