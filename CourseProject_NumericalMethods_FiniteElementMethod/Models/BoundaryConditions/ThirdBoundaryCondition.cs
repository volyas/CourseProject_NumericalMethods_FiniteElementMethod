namespace CourseProject_NumericalMethods_FiniteElementMethod.Models.BoundaryConditions;

public class ThirdBoundaryCondition
{
    public int[] GlobalNodesNumbers { get; set; }
    public double Beta { get; set; }
    public double[] Us { get; set; }

    public ThirdBoundaryCondition(int[] globalNodesNumbers, double beta, double[] us)
    {
        GlobalNodesNumbers = globalNodesNumbers;
        Beta = beta;
        Us = us;
    }
}