namespace OpenMEPSandbox.Autocad;

/// <summary>
/// Class for Text object
/// </summary>
public class Text
{
    private Text()
    {
    }
    
    /// <summary>
    /// Return the text string of the text object
    /// </summary>
    /// <param name="AcadText"></param>
    /// <returns></returns>
    public static string TextString(dynamic AcadText)
    {
        return AcadText.CadEntity.TextString;
    }

    /// <summary>
    /// Return Layer name of the text object
    /// </summary>
    /// <param name="AcadText"></param>
    /// <returns></returns>
    public static string Layer(dynamic AcadText)
    {
        return AcadText.CadEntity.Layer;
    }

    /// <summary>
    /// Return Rotation of the text object
    /// </summary>
    /// <param name="AcadText"></param>
    /// <returns></returns>
    public static double Rotation(dynamic AcadText)
    {
        return AcadText.CadEntity.Rotation;
    }
}