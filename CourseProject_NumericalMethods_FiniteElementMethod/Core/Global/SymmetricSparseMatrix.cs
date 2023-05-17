namespace CourseProject.Core.Global;

public class SymmetricSparseMatrix
{
    public double[] Diagonal { get; set; }
    public double[] Values { get; set; }
    public int[] RowsIndexes { get; }
    public int[] ColumnsIndexes { get; }

    public int CountRows => Diagonal.Length;
    public int CountColumns => Diagonal.Length;
    public int this[int rowIndex, int columnIndex] =>
        Array.IndexOf(ColumnsIndexes, columnIndex, RowsIndexes[rowIndex],
            RowsIndexes[rowIndex + 1] - RowsIndexes[rowIndex]);

    public SymmetricSparseMatrix(int[] rowsIndexes, int[] columnsIndexes)
    {
        Diagonal = new double[rowsIndexes.Length - 1];
        Values = new double[rowsIndexes[^1]];
        RowsIndexes = rowsIndexes;
        ColumnsIndexes = columnsIndexes;
    }

    public SymmetricSparseMatrix
    (
        int[] rowsIndexes,
        int[] columnsIndexes,
        double[] diagonal,
        double[] values
    )
    {
        RowsIndexes = rowsIndexes;
        ColumnsIndexes = columnsIndexes;
        Diagonal = diagonal;
        Values = values;
    }

    public static GlobalVector operator *(SymmetricSparseMatrix matrix, GlobalVector vector)
    {
        var rowsIndexes = matrix.RowsIndexes;
        var columnsIndexes = matrix.ColumnsIndexes;
        var di = matrix.Diagonal;
        var values = matrix.Values;

        var result = new GlobalVector(matrix.CountRows);

        for (var i = 0; i < matrix.CountRows; i++)
        {
            result[i] += di[i] * vector[i];

            for (var j = rowsIndexes[i]; j < rowsIndexes[i + 1]; j++)
            {
                result[i] += values[j] * vector[columnsIndexes[j]];
                result[columnsIndexes[j]] += values[j] * vector[i];
            }
        }

        return result;
    }

    public static SymmetricSparseMatrix operator *(double number, SymmetricSparseMatrix matrix)
    {
        var result = matrix.Clone();

        for (var i = 0; i < matrix.CountRows; i++)
        {
            result.Diagonal[i] *= number;

            for (var j = matrix.RowsIndexes[i]; j < matrix.RowsIndexes[i + 1]; j++)
            {
                result.Values[j] *= number;
            }
        }

        return result;
    }

    public SymmetricSparseMatrix Clone()
    {
        var rowIndexes = new int[RowsIndexes.Length];
        var columnIndexes = new int[ColumnsIndexes.Length];
        var diagonal = new double[Diagonal.Length];
        var values = new double[Values.Length];

        Array.Copy(RowsIndexes, rowIndexes, RowsIndexes.Length);
        Array.Copy(ColumnsIndexes, columnIndexes, ColumnsIndexes.Length);
        Array.Copy(Diagonal, diagonal, Diagonal.Length);
        Array.Copy(Values, values, Values.Length);

        return new SymmetricSparseMatrix(rowIndexes, columnIndexes, diagonal, values);
    }
}