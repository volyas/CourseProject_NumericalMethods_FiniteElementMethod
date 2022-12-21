using CourseProject_NumericalMethods_FiniteElementMethod.Models.LocalSolutionComponents;
using CourseProject_NumericalMethods_FiniteElementMethod.Tools;
using CourseProject_NumericalMethods_FiniteElementMethod.Tools.Producers;
namespace CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;

public class Element : IEquatable<Element>
{
    public int[] GlobalNodesNumbers { get; set; }
    public Material Material { get; set; }
    public LocalBasisFunction[] LocalBasisFunctions { get; set; }
    public LocalMatrix StiffnessMatrix { get; set; }
    public LocalMatrix MassMatrix { get; set; }
    public LocalMatrix LocalMatrixA { get; set; }
    public LocalVector RightPart { get; set; }

    public Element(int[] globalNodesNumbers, Material material, LocalBasisFunction[] localBasisFunctions)
    {
        GlobalNodesNumbers = globalNodesNumbers;
        Material = material;
        LocalBasisFunctions = localBasisFunctions;
    }
    public void CalcStiffnessMatrix(NodeFinder nodeFinder, LocalMatrix gx, LocalMatrix gy, LocalMatrix gz)
    {
        StiffnessMatrix = new LocalMatrix(GlobalNodesNumbers.Length, GlobalNodesNumbers.Length);

        var hx = nodeFinder.FindNode(GlobalNodesNumbers[1]).X - nodeFinder.FindNode(GlobalNodesNumbers[0]).X;
        var hy = nodeFinder.FindNode(GlobalNodesNumbers[2]).Y - nodeFinder.FindNode(GlobalNodesNumbers[0]).Y;
        var hz = nodeFinder.FindNode(GlobalNodesNumbers[4]).Z - nodeFinder.FindNode(GlobalNodesNumbers[0]).Z;

        StiffnessMatrix = (gx * (1.0/(hx*hx)) + gy * (1.0/(hy*hy)) + gz * (1.0/(hz*hz))) * Material.Lambda * ((hx * hy * hz) / 36.0) ;
    }

    public void CalcMassMatrix(NodeFinder nodeFinder, LocalMatrix m)
    {
        MassMatrix = new LocalMatrix(GlobalNodesNumbers.Length, GlobalNodesNumbers.Length);

        var hx = nodeFinder.FindNode(GlobalNodesNumbers[1]).X - nodeFinder.FindNode(GlobalNodesNumbers[0]).X;
        var hy = nodeFinder.FindNode(GlobalNodesNumbers[2]).Y - nodeFinder.FindNode(GlobalNodesNumbers[0]).Y;
        var hz = nodeFinder.FindNode(GlobalNodesNumbers[4]).Z - nodeFinder.FindNode(GlobalNodesNumbers[0]).Z;

        MassMatrix = m * ((hx * hy * hz) / 216.0);
    }

    public void CalcRightPart(PComponentsProducer pComponentsProducer)
    {
        var rightPart = new LocalVector(GlobalNodesNumbers.Length);

        for (var i = 0; i < GlobalNodesNumbers.Length; i++)
        {
            rightPart[i] = pComponentsProducer.CalcRightPart(GlobalNodesNumbers[i]);
        }

        RightPart = MassMatrix * rightPart;
    }

    public void CalcAMatrix()
    {
        LocalMatrixA = StiffnessMatrix + MassMatrix * Material.Gamma;
    }
    public bool Equals(Element? other)
    {
        return GlobalNodesNumbers.SequenceEqual(other.GlobalNodesNumbers) && Material.Equals(other.Material);
    }
}

