using CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;

namespace CourseProject_NumericalMethods_FiniteElementMethod.Factories;

public class MaterialFactory
{
    public readonly Dictionary<int, double> LambdasData;
    public readonly Dictionary<int, double> GammasData;

    public MaterialFactory(IEnumerable<double> lambdas, IEnumerable<double> gammas)
    {
        LambdasData = lambdas.Select((value, index) => new KeyValuePair<int, double>(index, value))
            .ToDictionary(index => index.Key, value => value.Value);
        GammasData = gammas.Select((value, index) => new KeyValuePair<int, double>(index, value)).ToDictionary(index => index.Key, value => value.Value);
    }

    public Material CreateMaterial(int id)
    {
        var material = new Material(id, LambdasData[id], GammasData[id]);

        return material;
    }
}