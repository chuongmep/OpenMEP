namespace OpenMEPSandbox.Display;

public class Color
{
    private Color()
    {
    }

    /// <summary>
    /// Generate random color
    /// </summary>
    /// <param name="amount">Number of color want to generate</param>
    /// <param name="start">start of range color(0-255)</param>
    /// <param name="end">end of range color(0-255)</param>
    /// <returns name="colors">list colors random</returns>
    public static List<DSCore.Color> Random(double amount, double start = 0, double end = 255)
    {
        // return list color by number of amount with random color shift is 2
        var listColor = new List<DSCore.Color>();
        for (int i = 0; i < amount; i++)
        {
            listColor.Add(DSCore.Color.ByARGB(255, (byte) DSCore.Math.Random(start, end),
                (byte) DSCore.Math.Random(start, end), (byte) DSCore.Math.Random(start, end)));
        }

        return listColor;
    }
}