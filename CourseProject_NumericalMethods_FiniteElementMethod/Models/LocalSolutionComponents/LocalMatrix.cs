namespace CourseProject_NumericalMethods_FiniteElementMethod.Models.LocalSolutionComponents;

public class LocalMatrix
{
    public double[,] Matrix { get; set; }
    public LocalMatrix(double[,] matrix)
    {
        Matrix = matrix;
    }
    public LocalMatrix()
    {
        Matrix = new double[0, 0];
    }
    public LocalMatrix(int n, int m)
    {
        Matrix = new double[n, m];
    }

    public double this[int i, int j]
    {
        get => Matrix[i, j];
        set => Matrix[i, j] = value;
    }

    public int CountRows()
    {
        return Matrix.GetLength(0);
    }

    public int CountColumns()
    {
        return Matrix.GetLength(1);
    }

    public static LocalMatrix operator +(LocalMatrix matrix1, LocalMatrix matrix2)
    {
        var localMatrix = new LocalMatrix(matrix1.CountRows(), matrix1.CountColumns());
        var rows = matrix1.CountRows();
        var columns = matrix1.CountColumns();

        if (rows != matrix2.CountRows() && columns != matrix2.CountColumns())
        {
            throw new Exception("Can't sum matrix different sizes!");
        }

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                localMatrix[i, j] = matrix1[i, j] + matrix2[i, j];
            }
        }

        return localMatrix;
    }

    public static LocalMatrix operator *(LocalMatrix matrix, double constant)
    {
        var localMatrix = new LocalMatrix(matrix.CountRows(), matrix.CountColumns());
        var rows = matrix.CountRows();
        var columns = matrix.CountColumns();

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                localMatrix[i, j] = constant * matrix[i, j];
            }
        }

        return localMatrix;
    }

    public static LocalVector operator *(LocalMatrix matrix, LocalVector vector)
    {
        var localVector = new LocalVector(vector.Count);
        var rows = matrix.CountRows();
        var columns = matrix.CountColumns();

        if (rows != vector.Count)
        {
            throw new Exception("Can't multiply matrix by a vector!");
        }

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                localVector[i] += matrix[i, j] * vector[j];
            }
        }

        return localVector;
    }
}