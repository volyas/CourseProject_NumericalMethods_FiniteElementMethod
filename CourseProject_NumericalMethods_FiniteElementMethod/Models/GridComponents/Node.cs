namespace CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;

public class Node : IEquatable<Node>
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Node(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public bool Equals(Node? other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    }
}