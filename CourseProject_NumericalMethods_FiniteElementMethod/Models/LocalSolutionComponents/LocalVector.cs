namespace CourseProject_NumericalMethods_FiniteElementMethod.Models.LocalSolutionComponents;

public class LocalVector
{
    public double[] Vector { get; set; }
    public LocalVector()
    {
        Vector = Array.Empty<double>();
    }
    public LocalVector(int size)
    {
        Vector = new double[size];
    }
    public LocalVector(double[] vector)
    {
        Vector = vector;
    }
    public double this[int index]
    {
        get => Vector[index];
        set => Vector[index] = value;
    }
    public int Count => Vector.Length;

    public static LocalVector operator +(LocalVector vector1, LocalVector vector2)
    {
        var length1 = vector1.Count;
        var sum = new LocalVector(length1);

        if (length1 != vector2.Count) throw new Exception("Can't sum vectors of different lengths!");

        for (var i = 0; i < length1; i++)
        {
            sum[i] = vector1[i] + vector2[i];
        }

        return sum;
    }

    public static LocalVector operator *(LocalVector localVector, double constant)
    {
        var length = localVector.Count;
        var vector = new LocalVector(length);

        for (var i = 0; i < length; i++)
        {
            vector[i] = constant * localVector[i];
        }

        return vector;
    }

    public IEnumerator<double> GetEnumerator() => (IEnumerator<double>)Vector.GetEnumerator();
}