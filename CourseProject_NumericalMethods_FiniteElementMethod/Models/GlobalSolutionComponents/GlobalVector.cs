using CourseProject_NumericalMethods_FiniteElementMethod.Models.LocalSolutionComponents;

namespace CourseProject_NumericalMethods_FiniteElementMethod.Models.GlobalSolutionComponents;

public class GlobalVector : LocalVector, ICloneable
{
    public GlobalVector(int size) : base(size) { }

    public GlobalVector(double[] vector)
    {
        Vector = vector;
    }

    public static GlobalVector operator +(GlobalVector vector1, GlobalVector vector2)
    {
        var length = vector1.Count;
        var sum = new GlobalVector(length);

        if (length != vector2.Count) throw new Exception("Can't sum vectors different lengths!");

        for (var i = 0; i < length; i++)
        {
            sum[i] = vector1[i] + vector2[i];
        }

        return sum;
    }

    public static GlobalVector operator -(GlobalVector vector1, GlobalVector vector2)
    {
        var length = vector1.Count;
        var sub = new GlobalVector(length);

        if (length != vector2.Count) throw new Exception("Can't sub vectors different lengths!");

        for (var i = 0; i < length; i++)
        {
            sub[i] = vector1[i] - vector2[i];
        }

        return sub;
    }

    public static GlobalVector operator *(GlobalVector localVector, double constant)
    {
        var length = localVector.Count;
        var vector = new GlobalVector(length);

        for (var i = 0; i < length; i++)
        {
            vector[i] = constant * localVector[i];
        }

        return vector;
    }

    public double CalcNorm()
    {
        return Math.Sqrt(Vector.Sum(element => element * element));
    }

    public void PlaceLocalVector(LocalVector rightPart, int[] globalNodesNumbers)
    {
        var length = rightPart.Count;
        for (var i = 0; i < length; i++)
        {
            Vector[globalNodesNumbers[i]] += rightPart[i];
        }
    }

    
    public object Clone()
    {
        var clone = new double[Count];
        Array.Copy(Vector, clone, Count);

        return new GlobalVector(Count)
        {
            Vector = clone
        };
    }
}