using Autodesk.Revit.DB;
using Revit.Elements;

namespace OpenMEPRevit.Utils;

/// <summary>Contains utilities related to family operations.</summary>
public class FamilyUtils
{
    private FamilyUtils(){}

    /// <summary>Converts a family to be face host based.</summary>
    /// <remarks>
    ///    Converts a family hosted by some element other than a face to be hosted by a face. This is done by replacing the existing host (wall, roof, ceiling, floor) with a face.
    ///    Conversion can succeed only if FamilyUtils.FamilyCanConvertToFaceHostBased() returns true.
    /// </remarks>
    /// <param name="doc">The document containing the family to be converted.</param>
    /// <param name="familyId">The family id.</param>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentException">
    ///    The input familyId cannot be converted to face host based.
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.InvalidOperationException">
    ///    Failed to convert the family to face host based.
    ///    -or-
    ///    The family is already unhosted.
    /// </exception>
    public static void ConvertFamilyToFaceHostBased(Autodesk.Revit.DB.Document doc,int familyId)
    {
        using Transaction tran = new Autodesk.Revit.DB.Transaction (doc, "convert");
        tran.Start();
        Autodesk.Revit.DB.FamilyUtils.ConvertFamilyToFaceHostBased(doc,new ElementId(familyId));
        tran.Commit();
    }

    /// <summary>Indicates whether the family can be converted to face host based.</summary>
    /// <param name="doc">The document.</param>
    /// <param name="familyId">The element id of the family.</param>
    /// <returns>
    /// True if the family can be converted to face-based.
    /// Otherwise false, which will be returned if there any family instances exist in the project, the family is already face-based, or the family does not have a host.
    /// Also, false is returned if the family does not belong to one of the following categories:
    /// <list type="bullet"><item>OST_CommunicationDevices</item><item>OST_DataDevices</item><item>OST_DuctTerminal</item><item>OST_ElectricalEquipment</item><item>OST_ElectricalFixtures</item><item>OST_FireAlarmDevices</item><item>OST_LightingDevices</item><item>OST_LightingFixtures</item><item>OST_MechanicalEquipment</item><item>OST_NurseCallDevices</item><item>OST_PlumbingFixtures</item><item>OST_SecurityDevices</item><item>OST_Sprinklers</item><item>OST_TelephoneDevices</item></list></returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <returns name="bool">return true if family can convert to face host based</returns>
    public static bool FamilyCanConvertToFaceHostBased(Autodesk.Revit.DB.Document doc,int familyId)
    {
        using Transaction tran = new Autodesk.Revit.DB.Transaction (doc, "convert");
        tran.Start();
        bool result = Autodesk.Revit.DB.FamilyUtils.FamilyCanConvertToFaceHostBased(doc,new ElementId(familyId));
        tran.Commit();
        return result;
    }
    
    
    /// <summary>Gets the profile Family Symbols of the document.</summary>
    /// <param name="doc">The document.</param>
    /// <param name="profileFamilyUsage">The profile family usage.</param>
    /// <param name="oneCurveLoopOnly">
    ///    Whether or not to return only profiles with one curve loop.
    /// </param>
    /// <returns>The set of profile Family Symbol element ids.</returns>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">
    ///    A non-optional argument was null
    /// </exception>
    /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentOutOfRangeException">
    ///    A value passed for an enumeration argument is not a member of that enumeration
    /// </exception>
    /// <since>2014</since>
    public static IEnumerable<Revit.Elements.Element> GetProfileSymbols(Autodesk.Revit.DB.Document doc,object profileFamilyUsage,bool oneCurveLoopOnly)
    {
        using Transaction tran = new Autodesk.Revit.DB.Transaction (doc, "GetProfileSymbols");
        tran.Start();
        ProfileFamilyUsage profile = (ProfileFamilyUsage)profileFamilyUsage;
        ICollection<ElementId> profileSymbols = Autodesk.Revit.DB.FamilyUtils.GetProfileSymbols(doc,profile,oneCurveLoopOnly);
        foreach (ElementId elementId in profileSymbols)
        {
            yield return doc.GetElement(elementId).ToDSType(true);
        }
        tran.Commit();
    }
}