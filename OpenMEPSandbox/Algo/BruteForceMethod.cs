using System.Diagnostics;
using Autodesk.DesignScript.Runtime;

namespace OpenMEPSandbox.Algo;

[IsVisibleInDynamoLibrary(false)]
public class BruteForceMethod
{
    // /// <summary>
    // /// calculate the minimum cost of a permutation
    // /// Return Value Permutation and rows with minimum cost
    // /// </summary>
    // /// <param name="matrix"></param>
    // public (int minCost, List<int> rows) BruteForce(int[,] matrix)
    // {
    //     // n is the number of columns, test with 14 columns (matrix 3x14)
    //     int n = matrix.GetLength(1);
    //     int[] permutation = new int[n];
    //     for (int i = 0; i < n; i++)
    //     {
    //         permutation[i] = i;
    //     }
    //     int[] minPermutation = new int[n];
    //     List<int> rows = new List<int>();
    //     int minCost = int.MaxValue;
    //     BruteForceMethod bruteForce = new BruteForceMethod();
    //     // Generate all permutations and find the minimum cost permutation
    //     bruteForce.Permute(permutation, 0, n, matrix, ref minCost, ref minPermutation,ref rows);
    //     // Output the minimum cost and the corresponding permutation
    //     Trace.WriteLine("Minimum Cost: " + minCost);
    //     Trace.WriteLine("Rows: " + string.Join(",", rows));
    //     Trace.Write("Permutation: ");
    //     for (int i = 0; i < n; i++)
    //     {
    //         Trace.Write(minPermutation[i] + " ");
    //     }
    //     return (minCost,rows);
    // }

    public (int minCost, IEnumerable<int> minAssignment) BruteForce(int[,] costMatrix)
    {
        int numRows = costMatrix.GetLength(0);
        int numCols = costMatrix.GetLength(1);

        // generate all possible assignments
        var assignments = GetPermutations(Enumerable.Range(0, numCols), numRows).ToList();

        // calculate the cost of each assignment
        int minCost = int.MaxValue;
        IEnumerable<int> minAssignment = new List<int>();
        foreach (IEnumerable<int>? assignment in assignments)
        {
            int cost = 0;
            for (int i = 0; i < numRows; i++)
            {
                cost += costMatrix[i,assignment.ToArray()[i]];
                
            }
            Trace.WriteLine($"Assignment: {string.Join(", ", assignment)}, Cost: {cost}");
            if (cost < minCost)
            {
                minCost = cost;
                minAssignment = assignment;
            }
        }
        Trace.WriteLine($"\nMinimum cost: {minCost}, Assignment: {string.Join(", ", minAssignment)}");
        return (minCost,minAssignment);
    }
    static int SumAssignment(int[,] matrix, int[] assignment)
    {
        int sum = 0;
        for (int i = 0; i < assignment.Length; i++)
        {
            sum += matrix[i, assignment[i]];
        }
        return sum;
    }
    static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
    {
        if (length == 1)
        {
            return list.Select(x => new[] { x });
        }

        return GetPermutations(list, length - 1)
            .SelectMany(x => list.Where(y => !x.Contains(y)),
                (t1, t2) => t1.Concat(new[] { t2 }));
    }
    
    // Function to calculate the total cost of a permutation
    static (int cost, List<int> rows) CalculateCost(int[,] matrix, int[] permutation)
    {
        int cost = 0;
        // n is sum of rows
        int n = matrix.GetLength(0);
        int m = matrix.GetLength(1);
        List<int> rows = new List<int>();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < permutation.Length; j++)
            {
                cost += matrix[i, permutation[j]];
                rows.Add(matrix[i,permutation[j]]);
                j++;
            }
            i++;
        }
        string totalCalc = string.Join(",", rows);
        Trace.WriteLine($"Calc: {totalCalc} =");
        Trace.WriteLine($"Value Min Cost:{cost}");
        return (cost,rows);
    }
    static int SumRowMatrix(int[,] matrix, int row)
    {
        int sum = 0;
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            sum += matrix[row, i];
        }
        return sum;
    }
    static int SumColumnMatrix(int[,] matrix, int column)
    {
        int sum = 0;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            sum += matrix[i, column];
        }
        return sum;
    }

    // Function to generate all possible permutations
    public void Permute(int[] permutation, int start, int n, int[,] matrix, ref int minCost, ref int[] minPermutation,ref List<int> rows)
    {
        if (start == n - 1)
        {
            // int cost = CalculateCost(matrix, permutation);
            (int cost, List<int> rows) cost = CalculateCost(matrix, permutation);
            if (cost.cost < minCost)
            {
                minCost = cost.cost;
                rows = cost.rows;
                Array.Copy(permutation, minPermutation, n);
            }
        }
        else
        {
            for (int i = start; i < n; i++)
            {
                int temp = permutation[start];
                permutation[start] = permutation[i];
                permutation[i] = temp;
                Permute(permutation, start + 1, n, matrix, ref minCost, ref minPermutation,ref rows);
                temp = permutation[start];
                permutation[start] = permutation[i];
                permutation[i] = temp;
            }
        }
    }
}