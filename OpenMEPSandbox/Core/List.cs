using Autodesk.DesignScript.Runtime;
using OpenMEPSandbox.Math;

namespace OpenMEPSandbox.Core;

public class List
{
    private List()
    {
        
    }
    /// <summary>
    /// Takes a list of doubles and returns a new list where each element is replaced
    /// with the index of its first occurrence in the input list. If an element appears
    /// multiple times in the input list, the index of its first occurrence is used.
    /// </summary>
    /// <param name="objects">The input list of strings to be indexed.</param>
    /// <returns name="indices">A new list where each element is replaced with its corresponding index.</returns>
    /// <example>
    /// ![](../OpenMEPPage/core/pic/List.IndexObjects.png)
    /// [List.IndexObjects.dyn](../OpenMEPPage/core/List.IndexObjects.dyn)
    /// </example>
    public static List<int> IndexObjects(List<object> objects)
    {
        List<int> indexedList = new List<int>();
        Dictionary<object, int> indexMap = new Dictionary<object, int>();
        int index = 0;

        foreach (object item in objects)
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
    /// Takes two lists as input and returns two indexed lists
    /// where each element is replaced with the index of its first occurrence in the input list.
    /// </summary>
    /// <param name="lst1">The first list to index.</param>
    /// <param name="lst2">The second list to index.</param>
    /// <returns>A Tuple containing the two indexed lists.</returns>
    /// <returns name="lst1">indices of first list</returns>
    /// <returns name="lst2">indices of second list</returns>
    /// <example>
    /// ![](../OpenMEPPage/core/pic/List.IndexTwoListObjects.png)
    /// [List.IndexTwoListObjects.dyn](../OpenMEPPage/core/pic/List.IndexTwoListObjects.dyn)
    /// </example>
    [MultiReturn("lst1", "lst2")]
    public static Dictionary<string, object> IndexTwoListObjects(List<object> lst1, List<object> lst2)
    {
        // find the unique values in both lists
        List<object> uniqueValues = lst1.Concat(lst2).Distinct().ToList();

        // create a dictionary to map unique values to indices
        Dictionary<object, int> valueToIndex = new Dictionary<object, int>();
        int index = 0;
        foreach (object value in uniqueValues)
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
    /// Encodes a list of categorical values using one-hot encoding.
    /// </summary>
    /// <param name="labels">A list of categorical values to encode.</param>
    /// <returns>An array of arrays representing the one-hot encoded values.</returns>
    /// <example>
    /// ![](../OpenMEPPage/core/pic/List.OneHotEncode.png)
    /// [List.OneHotEncode.dyn](../OpenMEPPage/core/pic/List.OneHotEncode.dyn)
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
    /// This method takes a list of objects as input and returns a dictionary
    /// mapping each unique string to its corresponding index in the list.
    /// </summary>
    /// <param name="objects">list of objects input</param>
    /// <returns name="name">name of object</returns>
    /// <returns name="index">indices of object</returns>
    /// <example>
    /// ![](../OpenMEPPage/core/pic/List.IndexUniqueObjects.png)
    /// [List.IndexUniqueObjects.dyn](../OpenMEPPage/core/pic/List.IndexUniqueObjects.dyn)
    /// </example>
    [MultiReturn("name", "index")]
    public static Dictionary<string, object> IndexUniqueObjects(List<object> objects)
    {
        Dictionary<object, int> dict = new Dictionary<object, int>();
        int index = 0;
        foreach (object elem in objects)
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
    /// Takes two lists of strings as input, merges them, and returns a dictionary with unique names and indices.
    /// </summary>
    /// <param name="list1">The first list of objects.</param>
    /// <param name="list2">The second list of objects.</param>
    /// <returns name="name">name of object</returns>
    /// <returns name="index">indices of object</returns>
    /// <example>
    /// ![](../OpenMEPPage/core/pic/List.IndexUniqueTwoListObjects.png)
    /// [List.IndexUniqueTwoListObjects.dyn](../OpenMEPPage/core/pic/List.IndexUniqueTwoListObjects.dyn)
    /// </example>
    [MultiReturn("name", "index")]
    public static Dictionary<string, object> IndexUniqueTwoListObjects(List<object> list1, List<object> list2)
    {
        // Merge the two lists and get the unique values
        var uniqueValues = list1.Union(list2).Distinct().ToList();

        // Create a dictionary to hold the unique values and their corresponding indices
        var uniqueDict = new Dictionary<object, int>();

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
    /// Returns a list of indices for a given list of objects.
    /// </summary>
    /// <param name="objects"></param>
    /// <returns name="indexs">list index of objects</returns>
    /// <example>
    /// ![](../OpenMEPPage/core/pic/List.IndexList.png)
    /// [List.IndexList.dyn](../OpenMEPPage/core/pic/List.IndexList.dyn)
    /// </example>
    public static List<object> IndexList(List<object> objects)
    {
        List<object> indexedList = new List<object>();
        for (int i = 0; i < objects.Count; i++)
        {
            indexedList.Add(i);
        }
        return indexedList;
    }
}