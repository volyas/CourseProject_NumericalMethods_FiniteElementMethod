namespace CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;

public class Material : IEquatable<Material>
{
    public int Id { get; set; }
    public double Lambda { get; set; }
    public double Gamma { get; set; }

    public Material(int id, double lambda, double gamma)
    {
        Id = id;
        Lambda = lambda;
        Gamma = gamma;
    }

    public bool Equals(Material? other)
    {
        return Id.Equals(other.Id) && Lambda.Equals(other.Lambda) && Gamma.Equals(other.Gamma);
    }
}