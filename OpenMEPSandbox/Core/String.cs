using System.Text.RegularExpressions;

namespace OpenMEPSandbox.Core;

public class String
{
    private String()
    {
        
    }
    
    /// <summary>
    /// Cast a string have point format to a point
    /// </summary>
    /// <param name="str">string point format</param>
    /// <returns name="point">the point can cast</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <example>
    /// ![](../OpenMEPPage/core/pic/String.AsPoint.png)
    /// [String.AsPoint.dyn](../OpenMEPPage/core/String.AsPoint.dyn)
    /// </example>
    public static Autodesk.DesignScript.Geometry.Point? AsPoint(string str)
    {
        if (string.IsNullOrEmpty(str)) throw new ArgumentNullException(nameof(str));
        // pattern to match format: Point(X = 0.000, Y = 0.000, Z = 0.000)
        string pattern = @"Point\(X = (?<x>.*), Y = (?<y>.*), Z = (?<z>.*)\)";
        Match match = Regex.Match(str, pattern);
        if (match.Success)
        {
            double x = double.Parse(match.Groups["x"].Value);
            double y = double.Parse(match.Groups["y"].Value);
            double z = double.Parse(match.Groups["z"].Value);
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y, z);
        }

        // pattern to match format: Point(X = 0.000, Y = 0.000)
        pattern = @"Point\(X = (?<x>.*), Y = (?<y>.*)\)";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            double x = double.Parse(match.Groups["x"].Value);
            double y = double.Parse(match.Groups["y"].Value);
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y);
        }

        // pattern to match format: (X = 0.000, Y = 0.000, Z = 0.000)
        pattern = @"\((X = (?<x>.*), Y = (?<y>.*), Z = (?<z>.*))\)";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            double x = double.Parse(match.Groups["x"].Value);
            double y = double.Parse(match.Groups["y"].Value);
            double z = double.Parse(match.Groups["z"].Value);
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y, z);
        }

        // pattern to match format: (X = 0.000, Y = 0.000)
        pattern = @"\((X = (?<x>.*), Y = (?<y>.*))\)";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            double x = double.Parse(match.Groups["x"].Value);
            double y = double.Parse(match.Groups["y"].Value);
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y);
        }

        // pattern to match format: X = 0.000, Y = 0.000, Z = 0.000
        pattern = @"^X\s=\s\d+\.\d+,\sY\s=\s\d+\.\d+,\sZ\s=\s\d+\.\d+$";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            string[] split = str.Split(',');
            double x = double.Parse(split[0].Split('=')[1].Trim());
            double y = double.Parse(split[1].Split('=')[1].Trim());
            double z = double.Parse(split[2].Split('=')[1].Trim());
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y, z);
        }

        // pattern to match format: X = 0.000, Y = 0.000
        pattern = @"^X\s=\s\d+\.\d+,\sY\s=\s\d+\.\d+$";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            string[] split = str.Split(',');
            double x = double.Parse(split[0].Split('=')[1].Trim());
            double y = double.Parse(split[1].Split('=')[1].Trim());
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y);
        }

        // pattern to match format: (5.5,4.5,6.8)
        pattern = @"\((?<x>.*),(?<y>.*),(?<z>.*)\)";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            double x = double.Parse(match.Groups["x"].Value);
            double y = double.Parse(match.Groups["y"].Value);
            double z = double.Parse(match.Groups["z"].Value);
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y, z);
        }

        // pattern to match format: (5.5,4.5)
        pattern = @"\((?<x>.*),(?<y>.*)\)";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            double x = double.Parse(match.Groups["x"].Value);
            double y = double.Parse(match.Groups["y"].Value);
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y);
        }

        //pattern to match format: (5.5,4.5) after that is character or string
        pattern = @"^\(\d+(\.\d+)?,\d+(\.\d+)?\)+$ (?:\s+\S+)";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            string[] split = str.Split(' ');
            double x = double.Parse(split[0].Split(',')[0].Trim('(', ')'));
            double y = double.Parse(split[0].Split(',')[1].Trim('(', ')'));
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y);
        }


        // pattern to match format: [5.5,4.5,6.8]
        pattern = @"\[(?<x>.*),(?<y>.*),(?<z>.*)\]";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            double x = double.Parse(match.Groups["x"].Value);
            double y = double.Parse(match.Groups["y"].Value);
            double z = double.Parse(match.Groups["z"].Value);
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y, z);
        }

        // pattern to match format: [5.5,4.5]
        pattern = @"\[(?<x>.*),(?<y>.*)\]";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            double x = double.Parse(match.Groups["x"].Value);
            double y = double.Parse(match.Groups["y"].Value);
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y);
        }

        // pattern to match format: 5.5,4.5,6.8
        pattern = @"(?<x>.*),(?<y>.*),(?<z>.*)";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            double x = double.Parse(match.Groups["x"].Value);
            double y = double.Parse(match.Groups["y"].Value);
            double z = double.Parse(match.Groups["z"].Value);
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y, z);
        }

        // pattern to match format: 5.5,4.5
        pattern = @"(?<x>.*),(?<y>.*)";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            double x = double.Parse(match.Groups["x"].Value);
            double y = double.Parse(match.Groups["y"].Value);
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y);
        }

        // pattern to match format: (0-9-1)
        pattern = @"^\(\d-\d-\d\)$|^\(\d{1,2}-\d{1,2}-\d{1,2}\)$";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            string[] xyz = str.Split('-');
            double x = double.Parse(xyz[0].Substring(1));
            double y = double.Parse(xyz[1]);
            double z = double.Parse(xyz[2].Substring(0, xyz[2].Length - 1));
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y, z);
        }

        // pattern to match format: (0-9)
        pattern = @"^\(\d-\d\)$|^\(\d{1,2}-\d{1,2}\)$";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            string[] xyz = str.Split('-');
            double x = double.Parse(xyz[0].Substring(1));
            double y = double.Parse(xyz[1].Substring(0, xyz[1].Length - 1));
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y);
        }

        // pattern to match format: (0.5-9.5-1.8)
        pattern = @"^\(\d+(\.\d+)?-\d+(\.\d+)?-\d+(\.\d+)?\)$";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            string[] xyz = str.Split('-');
            double x = double.Parse(xyz[0].Substring(1));
            double y = double.Parse(xyz[1]);
            double z = double.Parse(xyz[2].Substring(0, xyz[2].Length - 1));
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y, z);
        }

        // pattern to match format: (0.5-9.5)
        pattern = @"^\(\d+(\.\d+)?-\d+(\.\d+)?\)$";
        match = Regex.Match(str, pattern);
        if (match.Success)
        {
            string[] xyz = str.Split('-');
            double x = double.Parse(xyz[0].Substring(1));
            double y = double.Parse(xyz[1].Substring(0, xyz[1].Length - 1));
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(x, y);
        }

        return null;
    }
    
    /// <summary>
    /// Indicates whether the specified regular expression finds a match in the specified input string.
    /// </summary>
    /// <param name="input">The string to search for a match.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <returns>
    /// <see langword="true" /> if the regular expression finds a match; otherwise, <see langword="false" />.</returns>
    /// <exception cref="T:System.ArgumentException">A regular expression parsing error occurred.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="input" /> or <paramref name="pattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException">A time-out occurred. For more information about time-outs, see the Remarks section.</exception>
    /// <example>
    /// ![](../OpenMEPPage/core/pic/String.IsMatch.png)
    /// [String.IsMatch.dyn](../OpenMEPPage/core/String.IsMatch.dyn)
    /// </example>
    public static bool IsMatch(string input, string pattern)
    {
        return Regex.IsMatch(input, pattern);
    }
    
    /// <summary>Indicates whether the specified string is <see langword="null" /> or an empty string ("").</summary>
    /// <param name="value">The string to test.</param>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="value" /> parameter is <see langword="null" /> or an empty string (""); otherwise, <see langword="false" />.</returns>
    /// <example>
    /// ![](../OpenMEPPage/core/pic/String.IsNullOrEmpty.png)
    /// [String.IsNullOrEmpty.dyn](../OpenMEPPage/core/String.IsNullOrEmpty.dyn)
    /// </example>
    public static bool IsNullOrEmpty(string? value)
    {
        if (value == null) return true;
        return string.IsNullOrEmpty(value);
    }

}