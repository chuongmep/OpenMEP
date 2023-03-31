using Autodesk.DesignScript.Runtime;
using OpenMEPSandbox.Math;

namespace OpenMEPSandbox.List;

public class Generate
{
    private Generate()
    {
    }

    /// <summary>
    /// Takes two lists as input and returns two indexed lists.
    /// </summary>
    /// <param name="lst1">The first list to index.</param>
    /// <param name="lst2">The second list to index.</param>
    /// <returns>A Tuple containing the two indexed lists.</returns>
    /// <example>
    /// ![](../OpenMEPPage/list/pic/Generate.IndexTwoListValues.png)
    /// </example>
    [MultiReturn("lst1", "lst2")]
    public static Dictionary<string, object> IndexTwoListValues(List<int> lst1, List<int> lst2)
    {
        // find the unique values in both lists
        List<int> uniqueValues = lst1.Concat(lst2).Distinct().ToList();

        // create a dictionary to map unique values to indices
        Dictionary<int, int> valueToIndex = new Dictionary<int, int>();
        int index = 0;
        foreach (int value in uniqueValues)
        {
            valueToIndex.Add(value, index);
            index++;
        }

        // replace each value in the lists with its corresponding index
        List<int> indexedLst1 = lst1.Select(value => valueToIndex[value]).ToList();
        List<int> indexedLst2 = lst2.Select(value => valueToIndex[value]).ToList();

        return new Dictionary<string, object>()
        {
            {"lst1", indexedLst1},
            {"lst2", indexedLst2}
        };
    }

    /// <summary>
    /// Indexes two string lists based on the values of the first list.
    /// Returns a tuple containing the two indexed lists.
    /// </summary>
    /// <param name="list1">The first list of strings to index.</param>
    /// <param name="list2">The second list of strings to index.</param>
    /// <example>
    /// ![](../OpenMEPPage/list/pic/Generate.IndexTwoListStrings.png)
    /// </example>
    [MultiReturn("list1", "list2")]
    public static Dictionary<string, object> IndexTwoListStrings(List<string> list1, List<string> list2)
    {
        var uniqueItems = new List<string>();
        uniqueItems.AddRange(list1);
        uniqueItems.AddRange(list2);
        uniqueItems = uniqueItems.Distinct().ToList();
        var indexedList1 = new List<int>();
        var indexedList2 = new List<int>();
        for (int i = 0; i < list1.Count; i++)
        {
            indexedList1.Add(uniqueItems.IndexOf(list1[i]));
            indexedList2.Add(uniqueItems.IndexOf(list2[i]));
        }

        return new Dictionary<string, object>()
        {
            {"list1", indexedList1},
            {"list2", indexedList2}
        };
    }

    /// <summary>
    /// Takes a list of doubles and returns a new list where each element is replaced
    /// with the index of its first occurrence in the input list. If an element appears
    /// multiple times in the input list, the index of its first occurrence is used.
    /// </summary>
    /// <param name="strings">The input list of strings to be indexed.</param>
    /// <returns>A new list where each element is replaced with its corresponding index.</returns>
    /// <example>
    /// ![](../OpenMEPPage/list/pic/Generate.IndexListString.png)
    /// </example>
    public static List<int> IndexListString(List<string> strings)
    {
        List<int> indexedList = new List<int>();
        Dictionary<string, int> indexMap = new Dictionary<string, int>();
        int index = 0;

        foreach (string item in strings)
        {
            if (!indexMap.ContainsKey(item))
            {
                indexMap[item] = index;
                index++;
            }

            indexedList.Add(indexMap[item]);
        }

        return indexedList;
    }

    /// <summary>
    /// Takes a list of doubles and returns a new list where each element is replaced
    /// with the index of its first occurrence in the input list. If an element appears
    /// multiple times in the input list, the index of its first occurrence is used.
    /// </summary>
    /// <param name="values">The input list of doubles to be indexed.</param>
    /// <returns>A new list where each element is replaced with its corresponding index.</returns>
    /// <example>
    /// ![](../OpenMEPPage/list/pic/Generate.IndexListValues.png)
    /// </example>
    public static List<int> IndexListValues(List<double> values)
    {
        var distinctList = values.Distinct().ToList();
        var indexList = new List<int>();

        for (int i = 0; i < values.Count; i++)
        {
            indexList.Add(distinctList.IndexOf(values[i]));
        }

        return indexList;
    }

    /// <summary>
    /// Encodes a list of categorical values using one-hot encoding.
    /// </summary>
    /// <param name="labels">A list of categorical values to encode.</param>
    /// <returns>An array of arrays representing the one-hot encoded values.</returns>
    /// <example>
    /// ![](../OpenMEPPage/list/pic/Generate.OneHotEncode.png)
    /// </example>
    public static int[][] OneHotEncode(List<string> labels)
    {
        // Determine the number of unique labels
        var uniqueLabels = new HashSet<string>(labels);
        int numLabels = uniqueLabels.Count;

        // Create an empty 2D array to hold the one-hot encoded vectors
        int[][] oneHotEncoded = new int[labels.Count][];
        for (int i = 0; i < labels.Count; i++)
        {
            oneHotEncoded[i] = new int[numLabels];
        }

        // Encode each label as a one-hot vector
        int index = 0;
        var labelToIndex = new Dictionary<string, int>();
        foreach (var label in uniqueLabels)
        {
            labelToIndex[label] = index;
            index++;
        }

        for (int i = 0; i < labels.Count; i++)
        {
            int labelIndex = labelToIndex[labels[i]];
            oneHotEncoded[i][labelIndex] = 1;
        }

        // Return the one-hot encoded vectors
        return Matrix.Transpose(oneHotEncoded);
    }

    /// <summary>
    /// This method takes a list of strings as input and returns a dictionary
    /// mapping each unique string to its corresponding index in the list.
    /// </summary>
    /// <param name="lst">list of strings input</param>
    /// <returns></returns>
    /// <example>
    /// ![](../OpenMEPPage/list/pic/Generate.UniqueNamesIndex.png)
    /// </example>
    [MultiReturn("name", "index")]
    public static Dictionary<string, object> UniqueNamesIndex(List<string> lst)
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        int index = 0;
        foreach (string elem in lst)
        {
            if (!dict.ContainsKey(elem))
            {
                dict[elem] = index;
                index++;
            }
        }

        return new Dictionary<string, object>()
        {
            {"name", dict.Keys.ToList()},
            {"index", dict.Values.ToList()}
        };
    }

    /// <summary>
    /// Takes a list of doubles as input and returns a dictionary with unique values and indices.
    /// </summary>
    /// <param name="list">The input list of doubles.</param>
    /// <returns>A dictionary with unique values and indices.</returns>
    /// <example>
    /// ![](../OpenMEPPage/list/pic/Generate.UniqueValuesIndex.png)
    /// </example>
    [MultiReturn("value", "index")]
    public static Dictionary<string, object> UniqueValuesIndex(List<double> list)
    {
        Dictionary<double, int> dict = new Dictionary<double, int>();

        for (int i = 0; i < list.Count; i++)
        {
            double value = list[i];

            // If the dictionary doesn't already contain the value, add it to the dictionary
            // and set its index to the current value of the index variable.
            if (!dict.ContainsKey(value))
            {
                dict[value] = i;
            }
        }

        return new Dictionary<string, object>()
        {
            {"value", dict.Keys.ToList()},
            {"index", dict.Values.ToList()}
        };
    }

    /// <summary>
    /// Takes two lists of strings as input, merges them, and returns a dictionary with unique names and indices.
    /// </summary>
    /// <param name="list1">The first list of names.</param>
    /// <param name="list2">The second list of names.</param>
    /// <returns>A dictionary with unique names and indices.</returns>
    /// <example>
    /// ![](../OpenMEPPage/list/pic/Generate.UniqueNamesIndexList.png)
    /// </example>
    [MultiReturn("name", "index")]
    public static Dictionary<string, object> UniqueNamesIndexTwoList(List<string> list1, List<string> list2)
    {
        // Merge the two lists and get the unique values
        var uniqueValues = list1.Union(list2).Distinct().ToList();

        // Create a dictionary to hold the unique values and their corresponding indices
        var uniqueDict = new Dictionary<string, int>();

        // Iterate over the unique values and add their index to the dictionary
        for (int i = 0; i < uniqueValues.Count; i++)
        {
            uniqueDict.Add(uniqueValues[i], i);
        }

        return new Dictionary<string, object>()
        {
            {"name", uniqueDict.Keys.ToList()},
            {"index", uniqueDict.Values.ToList()}
        };
    }

    /// <summary>
    /// Returns a dictionary of unique values and their corresponding indices from two lists of doubles.
    /// </summary>
    /// <param name="list1">The first list of doubles.</param>
    /// <param name="list2">The second list of doubles.</param>
    /// <returns>A dictionary containing unique values and their corresponding indices.</returns>
    /// <example>
    /// ![](../OpenMEPPage/list/pic/Generate.UniqueValuesIndexList.png)
    /// </example>
    [MultiReturn("value", "index")]
    public static Dictionary<string, object> UniqueValuesIndexTwoList(List<double> list1, List<double> list2)
    {
        // Merge the two lists and get the unique values
        var uniqueValues = list1.Union(list2).Distinct().ToList();

        // Create a dictionary to hold the unique values and their corresponding indices
        var uniqueDict = new Dictionary<double, int>();

        // Iterate over the unique values and add their index to the dictionary
        for (int i = 0; i < uniqueValues.Count; i++)
        {
            uniqueDict.Add(uniqueValues[i], i);
        }

        return new Dictionary<string, object>()
        {
            {"value", uniqueDict.Keys.ToList()},
            {"index", uniqueDict.Values.ToList()}
        };
    }
}