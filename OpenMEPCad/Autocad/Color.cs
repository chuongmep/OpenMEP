namespace OpenMEPCad.Autocad;

public class Color
{
    private Color()
    {
        
    }
    public static dynamic EntityColor(dynamic AcadColor)
    {
        return AcadColor.EntityColor;
    }
    public dynamic ColorName(dynamic AcadColor)
    {
        return AcadColor.ColorName;
    }
    public dynamic BookName(dynamic AcadColor)
    {
        return AcadColor.BookName;
    }
    public dynamic ColorIndex(dynamic AcadColor)
    {
        return AcadColor.ColorIndex;
    }
    public dynamic Red(dynamic AcadColor)
    {
        return AcadColor.Red;
    }
    public dynamic Green(dynamic AcadColor)
    {
        return AcadColor.Green;
    }
    public dynamic Blue(dynamic AcadColor)
    {
        return AcadColor.Blue;
    }
}