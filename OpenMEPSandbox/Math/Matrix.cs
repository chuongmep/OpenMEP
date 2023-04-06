namespace OpenMEPSandbox.Math;

public class Matrix
{
    private Matrix()
    {
    }
    
    /// <summary>
    /// Transposes a 2D array of integers.
    /// </summary>
    /// <param name="matrix">The 2D array of integers to be transposed.</param>
    /// <returns>The transposed 2D array of integers.</returns>
    /// <example>
    /// ![](../OpenMEPPage/math/pic/Matrix.Transpose.png)
    /// [Matrix.Transpose.dyn](../OpenMEPPage/math/Matrix.Transpose.dyn)
    /// </example>
    public static int[][] Transpose(int[][] matrix)
    {
        int numRows = matrix.Length;
        int numCols = matrix[0].Length;

        int[][] transposed = new int[numCols][];
        for (int i = 0; i < numCols; i++)
        {
            transposed[i] = new int[numRows];
            for (int j = 0; j < numRows; j++)
            {
                transposed[i][j] = matrix[j][i];
            }
        }

        return transposed;
    }
    
    /// <summary>
    /// Applies the softmax function to a 2D array of values.
    /// </summary>
    /// <param name="z">A 2D array of input values.</param>
    /// <returns>A 2D array of output values, where each row has been transformed into a probability distribution.</returns>
    /// <example>
    /// ![](../OpenMEPPage/math/pic/Matrix.Softmax.png)
    /// [Matrix.Softmax.dyn](../OpenMEPPage/math/pic/Matrix.Softmax.dyn)
    /// </example>
    public static double[][] Softmax(double[][]z)
    {
        double[][] result = new double[z.Length][];
        for (int i = 0; i < z.Length; i++)
        {
            result[i] = new double[z[i].Length];
            double sum = 0;
            for (int j = 0; j < z[i].Length; j++)
            {
                result[i][j] = System.Math.Exp(z[i][j]);
                sum += result[i][j];
            }
            for (int j = 0; j < z[i].Length; j++)
            {
                result[i][j] /= sum;
            }
        }
        return result;
    }
}